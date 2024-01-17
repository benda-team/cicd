using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Infrastructure.Tp;
using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.Budget;
using ODTradePromotion.API.Models.CheckBudget;
using ODTradePromotion.API.Models.External;
using ODTradePromotion.API.Services.Base;
using Sys.Common.Constants;
using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ODTradePromotion.API.Services.ExternalCheckBudgetService
{
    public class ExternalCheckBudgetService : IExternalCheckBudgetService
    {
        #region Property
        private readonly ILogger<ExternalCheckBudgetService> _logger;
        private readonly IBaseRepository<TpBudget> _serviceBudget;
        private readonly IBaseRepository<TpBudgetUsed> _serviceBudgetUsed;
        private readonly IBaseRepository<TpBudgetAllotment> _serviceBudgetAllotment;
        private readonly IMapper _mapper;
        private readonly IDapperRepositories _dapper;

        #endregion

        public ExternalCheckBudgetService(ILogger<ExternalCheckBudgetService> logger, IBaseRepository<TpBudget> serviceBudget,
            IMapper mapper, IBaseRepository<TpBudgetUsed> serviceBudgetUsed,
            IDapperRepositories dapper, IBaseRepository<TpBudgetAllotment> serviceBudgetAllotment
            )
        {
            _logger = logger;
            _serviceBudget = serviceBudget;
            _serviceBudgetUsed = serviceBudgetUsed;
            _mapper = mapper;
            _dapper = dapper;
            _serviceBudgetAllotment = serviceBudgetAllotment;
        }

        public List<BudgetRemainAndCustomerBudgetModel> SyncBudget(FilterBudgetRemainAndCustomerBudget request)
        {
            List<string> arrayRequest = new();
            foreach (var item in request.BudgetCode)
            {
                arrayRequest.Add("'" + item + "'");
            }
            var lstBudgetCode = "(" + string.Join(", ", arrayRequest.ToArray()) + ")";

            // BudgetRemain
            string sqlQueryBudgetRemain = @$"SELECT 
                                bu.""Code"" as ""BudgetCode"", 
                                bu.""BudgetType"",
                                'TMBudget' as ""Type"",
                                null as ""CustomerCode"",
                                null as ""CustomerShiptoCode"",
                                bu.""BudgetAllocationLevel"",
                                al.""SalesTerritoryValueCode"",
                                CASE
                                     WHEN bu.""BudgetAllocationLevel"" = 'NW' THEN bu.""BudgetAvailable""
                                     ELSE
                                         al.""BudgetQuantityWait""
                                END ""BudgetRemains"",
                                CASE 
                                     WHEN bu.""BudgetAllocationLevel"" = 'NW' THEN bu.""LimitBudgetPerCustomer""
                                     ELSE
                                         al.""LimitBudgetPerCustomer""
                                END ""CustomerBudget"",
                                null ::FLOAT  as ""BudgetBooked""
                                FROM ""TpBudgets"" bu
                                LEFT JOIN ""TpBudgetAllotments"" al ON al.""BudgetCode"" = bu.""Code""
                                WHERE bu.""Code"" IN " + lstBudgetCode + @" AND bu.""SaleOrg"" = '" + request.SalesOrgCode + "'";
            var dataBudgetRemain = _dapper.QueryWithoutSync<BudgetRemainAndCustomerBudgetModel>(sqlQueryBudgetRemain);
            var budgetRemain = (List<BudgetRemainAndCustomerBudgetModel>)dataBudgetRemain;

            var budgetRemainResult = new List<BudgetRemainAndCustomerBudgetModel>();

            foreach (var item in budgetRemain.ToList())
            {
                if (item.BudgetAllocationLevel == "NW")
                {
                    budgetRemainResult.Add(item);
                }
                else if (item.BudgetAllocationLevel == "TL01" && !string.IsNullOrEmpty(request.BranchCode) && item.SalesTerritoryValueCode.Equals(request.BranchCode))
                {
                    budgetRemainResult.Add(item);
                }
                else if (item.BudgetAllocationLevel == "TL02" && !string.IsNullOrEmpty(request.RegionCode) && item.SalesTerritoryValueCode.Equals(request.RegionCode))
                {
                    budgetRemainResult.Add(item);
                }
                else if (item.BudgetAllocationLevel == "TL03" && !string.IsNullOrEmpty(request.SubRegionCode) && item.SalesTerritoryValueCode.Equals(request.SubRegionCode))
                {
                    budgetRemainResult.Add(item);
                }
                else if (item.BudgetAllocationLevel == "TL04" && !string.IsNullOrEmpty(request.AreaCode) && item.SalesTerritoryValueCode.Equals(request.AreaCode))
                {
                    budgetRemainResult.Add(item);
                }
                else if (item.BudgetAllocationLevel == "TL05" && !string.IsNullOrEmpty(request.SubAreaCode) && item.SalesTerritoryValueCode.Equals(request.SubAreaCode))
                {
                    budgetRemainResult.Add(item);
                }
                else if (item.BudgetAllocationLevel == "DSA" && !string.IsNullOrEmpty(request.DSACode) && item.SalesTerritoryValueCode.Equals(request.DSACode))
                {
                    budgetRemainResult.Add(item);
                }
            }

            // CustomerBudget 
            string sqlQueryCustomerBudget = @$"
                   WITH ""SUM_BookedBudget"" as
                    (
                        SELECT
                        used.""BudgetCode"",
                        SUM(used.""QuantityUsed"") ::FLOAT as ""QuantityUsed"",
                        SUM(used.""AmountUsed"") ::FLOAT as ""AmountUsed"",
                        used.""CustomerCode"", used.""ShiptoCode"" as ""CustomerShiptoCode""
                        FROM ""TpBudgetUseds"" used
                        WHERE used.""BudgetCode"" IN " + lstBudgetCode + @$" AND used.""RouteZoneCode"" = '" + request.RouteZoneCode + @"'
                        GROUP BY used.""CustomerCode"", used.""ShiptoCode"", used.""BudgetCode""
                    ),
                    ""CusBudget"" as
                     (
                     SELECT
                     sumb.""BudgetCode"", 
                    bu.""BudgetType"",
                    'CusBudget' as ""Type"",

                    sumb.""CustomerCode"", sumb.""CustomerShiptoCode"",
                    null as ""BudgetAllocationLevel "", null as ""SalesTerritoryValueCode"",
                    null ::FLOAT as ""BudgetRemains"", 
                    null ::FLOAT as ""CustomerBudget"",
                    CASE
                      WHEN bu.""BudgetType"" = '01' THEN  sumb.""QuantityUsed"" 
                      WHEN bu.""BudgetType"" = '02' THEN sumb.""AmountUsed"" 
                    END ""BudgetBooked""
                    FROM ""SUM_BookedBudget"" sumb
                    LEFT JOIN ""TpBudgets"" bu ON bu.""Code"" = sumb.""BudgetCode""
                    )
                    SELECT * FROM ""CusBudget""
                    ";
            var dataCustomerBudget = _dapper.QueryWithoutSync<BudgetRemainAndCustomerBudgetModel>(sqlQueryCustomerBudget);
            var customerBudget = (List<BudgetRemainAndCustomerBudgetModel>)dataCustomerBudget;
            if (customerBudget.ToList().Count > 0)
            {
                foreach (var item in customerBudget.ToList())
                {
                    budgetRemainResult.Add(item);
                }
            }

            return budgetRemainResult;
        }

        public List<ConfigBudgetModel> GetConfigBudget(FilterConfigBudget request)
        {
            string sqlQueryConfigBudget = @$"
                                SELECT bu.""Code"", bu.""SaleOrg"", bu.""BudgetType"", 
                                bu.""BudgetAllocationForm"",  
                                bu.""TotalBudget"", bu.""BudgetAvailable"", bu.""BudgetUsed"", 
                                bu.""LimitBudgetPerCustomer"",

                                bu.""BudgetAllocationLevel"", alot.""SalesTerritoryValueCode"", 
                                alot.""BudgetQuantityDetail"" as ""STTotalBudget"", 
                                alot.""BudgetQuantityUsed"" as ""STBudgetUsed"", alot.""BudgetQuantityWait"" as ""STBudgetAvailable"", 
                                alot.""LimitBudgetPerCustomer"" as ""STLimitBudgetPerCustomer""
                                FROM ""TpBudgets"" bu
                                LEFT JOIN ""TpBudgetAllotments"" alot ON alot.""BudgetCode"" = bu.""Code""
                                WHERE bu.""Code"" = '" + request.BudgetCode + @"' AND bu.""SaleOrg"" = '" + request.SalesOrgCode + @"'";

            if (request.BudgetAllocationLevel != "NW")
            {
                sqlQueryConfigBudget = sqlQueryConfigBudget + @" AND alot.""SalesTerritoryValueCode"" = '" + request.BudgetAllocationLevel + "'";
            }

            var data = _dapper.QueryWithoutSync<ConfigBudgetModel>(sqlQueryConfigBudget);
            var configBudget = (List<ConfigBudgetModel>)data;
            var configBudgetResult = new List<ConfigBudgetModel>();

            foreach (var item in configBudget.ToList())
            {
                if (item.BudgetAllocationLevel == "TL01" && !string.IsNullOrEmpty(request.BranchCode) && item.SalesTerritoryValueCode.Equals(request.BranchCode))
                {
                    configBudgetResult.Add(item);
                }
                else if (item.BudgetAllocationLevel == "TL02" && !string.IsNullOrEmpty(request.RegionCode) && item.SalesTerritoryValueCode.Equals(request.RegionCode))
                {
                    configBudgetResult.Add(item);
                }
                else if (item.BudgetAllocationLevel == "TL03" && !string.IsNullOrEmpty(request.SubRegionCode) && item.SalesTerritoryValueCode.Equals(request.SubRegionCode))
                {
                    configBudgetResult.Add(item);
                }
                else if (item.BudgetAllocationLevel == "TL04" && !string.IsNullOrEmpty(request.AreaCode) && item.SalesTerritoryValueCode.Equals(request.AreaCode))
                {
                    configBudgetResult.Add(item);
                }
                else if (item.BudgetAllocationLevel == "TL05" && !string.IsNullOrEmpty(request.SubAreaCode) && item.SalesTerritoryValueCode.Equals(request.SubAreaCode))
                {
                    configBudgetResult.Add(item);
                }
                else if (item.BudgetAllocationLevel == "DSA" && !string.IsNullOrEmpty(request.DSACode) && item.SalesTerritoryValueCode.Equals(request.DSACode))
                {
                    configBudgetResult.Add(item);
                }
                else if (item.BudgetAllocationLevel == "NW")
                {
                    configBudgetResult.Add(item);
                }
            }

            return configBudgetResult;
        }

        public Result<CheckBudgetOutput> CheckBudget(SyncCheckBudgetModel input)
        {
            Result<CheckBudgetOutput> result = new Result<CheckBudgetOutput>();
            try
            {
                 CheckBudgetOutput output = new CheckBudgetOutput()
                {
                    CustomerCode = input!.CustomerCode,
                    BudgetCode = input!.BudgetCode,
                    BudgetType = input!.BudgetType,
                    PromotionCode = input!.PromotionCode,
                    CustomerShiptoCode = input!.CustomerShipTo,
                    BudgetBook = input!.BudgetBook,
                    PromotionLevel = input!.PromotionLevel,
                    ReferalCode = input!.ReferalCode
                };


                switch (input!.BudgetAllocationLevel)
                {
                    case "TL01":
                        input!.SalesTerritoryValueCode = input!.BranchCode;
                        break;
                    case "TL02":
                        input!.SalesTerritoryValueCode = input!.RegionCode;
                        break;
                    case "TL03":
                        input!.SalesTerritoryValueCode = input!.SubRegionCode;
                        break;
                    case "TL04":
                        input!.SalesTerritoryValueCode = input!.AreaCode;
                        break;
                    case "TL05":
                        input!.SalesTerritoryValueCode = input!.SubAreaCode;
                        break;
                    case "DSA":
                        input!.SalesTerritoryValueCode = input!.DSACode;
                        break;
                    default:
                        input!.SalesTerritoryValueCode = null;
                        break;
                };

                #region Get Raws data
                var query5 = _dapper.Query<TpBudget>(@"SELECT bu.""Id"", bu.""Code"", bu.""SaleOrg"", bu.""BudgetType"", 
																						bu.""BudgetAllocationLevel"",
																						bu.""BudgetAllocationForm"",  
                                                                                        bu.""FlagOverBudget"",
																						bu.""TotalBudget"", bu.""BudgetAvailable"", bu.""BudgetUsed"", 
																						bu.""LimitBudgetPerCustomer""
																						FROM ""TpBudgets"" bu
																						WHERE bu.""Code"" = '" + input!.BudgetCode + @"' AND bu.""SaleOrg"" = '" + input!.SaleOrg + "'").Result?.FirstOrDefault();


                #region Set input
                if (string.IsNullOrEmpty(input!.BudgetAllocationLevel))
                {


                    input.BudgetAllocationLevel = query5.BudgetAllocationLevel;
                    input.BudgetType = string.IsNullOrEmpty(input.BudgetType) ? query5.BudgetType : input.BudgetType;


                    switch (input!.BudgetAllocationLevel)
                    {
                        case "TL01":
                            input!.SalesTerritoryValueCode = input!.BranchCode;
                            break;
                        case "TL02":
                            input!.SalesTerritoryValueCode = input!.RegionCode;
                            break;
                        case "TL03":
                            input!.SalesTerritoryValueCode = input!.SubRegionCode;
                            break;
                        case "TL04":
                            input!.SalesTerritoryValueCode = input!.AreaCode;
                            break;
                        case "TL05":
                            input!.SalesTerritoryValueCode = input!.SubAreaCode;
                            break;
                        case "DSA":
                            input!.SalesTerritoryValueCode = input!.DSACode;
                            break;
                        default:
                            input!.SalesTerritoryValueCode = null;
                            break;
                    };
                }
                else if (query5 is null)
                {
                    output.Message = "NO_FOUND_BUDGET";
                    return result;
                }

                #endregion


                var query3 = _dapper.Query<QueryReturn>(@"SELECT
                                                    used.""BudgetCode"",
                                                    used.""CustomerCode"", used.""ShiptoCode"" as ""CustomerShiptoCode"",
                                                    SUM(used.""QuantityUsed"") ::FLOAT as ""QuantityUsed"",
                                                    SUM(used.""AmountUsed"") ::FLOAT as ""AmountUsed""        
                                                    FROM ""TpBudgetUseds"" used
                                                    WHERE used.""BudgetCode"" = '" + input!.BudgetCode + @"' AND used.""CustomerCode"" = '" + input!.CustomerCode + @"' AND used.""ShiptoCode"" = '" + input!.CustomerShipTo + @"'
                                                    GROUP BY used.""CustomerCode"", used.""ShiptoCode"", used.""BudgetCode""").Result?.FirstOrDefault();

                var query4 = _dapper.Query<QueryFourReturn>(@"SELECT bu.""Id"",bu.""Code"", bu.""SaleOrg"", bu.""BudgetType"", 
                                                            bu.""BudgetAllocationForm"",  
                                                            bu.""TotalBudget"", bu.""BudgetAvailable"", bu.""BudgetUsed"", 
                                                            bu.""LimitBudgetPerCustomer"",
                                                            bu.""FlagOverBudget"",
                                                            bu.""BudgetAllocationLevel"", alot.""SalesTerritoryValueCode"", 
                                                            alot.""BudgetQuantityDetail"" as ""STTotalBudget"", 
                                                            alot.""BudgetQuantityUsed"" as ""STBudgetUsed"", alot.""BudgetQuantityWait"" as ""STBudgetAvailable"", 
                                                            alot.""LimitBudgetPerCustomer"" as ""STLimitBudgetPerCustomer"" 
                                                            FROM ""TpBudgets"" bu
                                                            LEFT JOIN ""TpBudgetAllotments"" alot ON alot.""BudgetCode"" = bu.""Code""
                                                            WHERE bu.""Code"" = '" + input!.BudgetCode + @"' AND bu.""SaleOrg"" = '" + input!.SaleOrg + @"' AND alot.""SalesTerritoryValueCode"" = '" + input!.SalesTerritoryValueCode + "' limit 1").Result?.FirstOrDefault();

                #endregion


              
                #region CheckBudget

                if (input.BudgetBook < 0 && (query3 is null || (query3!.AmountUsed + query3!.QuantityUsed) < (input.BudgetBook * -1))) //
                {
                    output.Message = "BUGET_BOOK_INVALID";
                    result.Data = output;
                    result.Success = true;
                    return result;
                }else  if (input!.BudgetAllocationLevel.Equals("NW"))
                {
                    #region Nếu là NationWide
                    if ((query5 is null || query5.BudgetAllocationLevel is null || query5.BudgetAllocationForm is null) && query5.BudgetAvailable is null)
                    {
                        output.Message = "NO_FOUND_BUDGET";
                        return result;
                    }
                    else
                    {
                        output.BudgetBookOver = query5.FlagOverBudget;
                        if (query5.BudgetAllocationForm.Equals("02"))
                        {
                            #region Điểm Bán
                            if (query5 is null || query5!.BudgetAvailable is null)
                            {
                                output.Message = "NO_FOUND_BUDGET";
                                result.Data = output;
                                result.Success = true;
                                return result;
                            }
                            else if ( query3 is not null &&  ((query3!.AmountUsed + query3!.QuantityUsed + input.BudgetBook) > query5.LimitBudgetPerCustomer) )
                            {
                                output.Message = "OVER_LIMIT_PER_CUSTOMER";
                                result.Data = output; 
                                result.Success = true;
                                return result;
                            }
                            //else if (query3 is null && input.BudgetBook > query5.LimitBudgetPerCustomer)
                            //{
                            //    output.Message = "OVER_LIMIT_PER_CUSTOMER";
                            //    result.Data = output;
                            //    result.Success = true;
                            //    return result;
                            //}
                            else if (query5.BudgetAvailable == 0 && input!.BudgetBook > 0)
                            {
                                output.Message = "NO_BUDGET_AVAILABLE";
                                result.Data = output;
                                result.Success = true;
                                return result;
                            }
                            else
                            {
                                #region update điểm bán
                                output.BudgetBooked = input!.BudgetBook;
                                if (query5.BudgetAvailable < input!.BudgetBook)
                                {
                                    output.BudgetBooked = (double)query5.BudgetAvailable;
                                    query5.BudgetAvailable = 0;
                                    query5.BudgetUsed = query5.BudgetUsed + output.BudgetBooked;
                                }
                                else
                                {
                                    if(query5.LimitBudgetPerCustomer == null ||  query5.LimitBudgetPerCustomer >= input!.BudgetBook)
                                    {
                                        output.BudgetBooked = input!.BudgetBook;
                                        query5.BudgetAvailable = (double)query5!.BudgetAvailable + (input!.BudgetBook * -1);
                                        query5.BudgetUsed = query5!.BudgetUsed + input!.BudgetBook;
                                    }
                                    else
                                    {
                                        input!.BudgetBook = (double)query5!.LimitBudgetPerCustomer;
                                        output.BudgetBooked = input!.BudgetBook;
                                        query5.BudgetAvailable = (double)query5!.BudgetAvailable + (input!.BudgetBook * -1);
                                        query5.BudgetUsed = query5!.BudgetUsed + input!.BudgetBook;
                                    }
                                }

                                TpBudgetUsed use = new TpBudgetUsed()
                                {
                                    CountryCode = input!.NationwideCode,
                                    AreaCode = input!.AreaCode,
                                    BudgetCode = input!.BudgetCode,
                                    DSACodeCode = input!.DSACode,
                                    BranchCode = input!.BranchCode,
                                    CustomerCode = input!.CustomerCode,
                                    RegionCode = input!.RegionCode,
                                    ShiptoCode = input!.CustomerShipTo,
                                    Key = input!.ReferalCode,
                                    RouteZoneCode = input!.RouteZoneCode,
                                    SubAreaCode = input!.SubAreaCode,
                                    SaleOrgCode = input!.SalesOrgCode,
                                    BudgetType = input!.BudgetType,
                                    AmountUsed = input!.BudgetType == "02" ? (decimal)output.BudgetBooked : 0,
                                    QuantityUsed = input!.BudgetType == "01" ? (int)output.BudgetBooked : 0,
                                    SubRegionCode = input!.SubRegionCode
                                };
                                _serviceBudgetUsed.Insert(use);
                                var currentBudget = _serviceBudget.GetById(query5.Id);

                                currentBudget.BudgetAvailable = query5!.BudgetAvailable;
                                currentBudget.BudgetUsed = query5!.BudgetUsed;
                                currentBudget.UpdatedDate = DateTime.Now;
                                _serviceBudget.Update(currentBudget);
                                #endregion 
                            }
                            #endregion
                        }
                        else
                        {
                            #region Khu vực
                            if (query5.LimitBudgetPerCustomer < input!.BudgetBook)
                            {
                                output.Message = "OVER_LIMIT_PER_CUSTOMER";
                                result.Data = output;
                                result.Success = true;
                                return result;
                            }
                            else if (query5.BudgetAvailable == 0 && input!.BudgetBook > 0)
                            {
                                output.Message = "NO_BUDGET_AVAILABLE";
                                result.Data = output;
                                result.Success = true;
                                return result;
                            }
                            else
                            {
                                #region update khu vuc
                                output.BudgetBooked = input!.BudgetBook;
                                if (query5.BudgetAvailable < input!.BudgetBook)
                                {
                                    output.BudgetBooked = (double)query5!.BudgetAvailable;
                                    query5.BudgetAvailable = 0;
                                    query5.BudgetUsed = query5!.BudgetUsed + output!.BudgetBooked;
                                }
                                else
                                {
                                    if (query5.LimitBudgetPerCustomer == null || query5.LimitBudgetPerCustomer >= input!.BudgetBook)
                                    {
                                        output.BudgetBooked = input!.BudgetBook;
                                        query5.BudgetAvailable = (double)query5.BudgetAvailable + (input!.BudgetBook * -1);
                                        query5.BudgetUsed = query5.BudgetUsed + input!.BudgetBook;
                                    }
                                    else
                                    {
                                        input!.BudgetBook = (double)query5.LimitBudgetPerCustomer;
                                        output.BudgetBooked = input!.BudgetBook;
                                        query5.BudgetAvailable = (double)query5!.BudgetAvailable + (input!.BudgetBook * -1);
                                        query5.BudgetUsed = query5!.BudgetUsed + input!.BudgetBook;
                                    }
                                }

                                TpBudgetUsed use = new TpBudgetUsed()
                                {
                                    CountryCode = input.NationwideCode,
                                    AreaCode = input!.AreaCode,
                                    BudgetCode = input!.BudgetCode,
                                    DSACodeCode = input!.DSACode,
                                    BranchCode = input!.BranchCode,
                                    CustomerCode = input.CustomerCode,
                                    RegionCode = input!.RegionCode,
                                    ShiptoCode = input!.CustomerShipTo,
                                    Key = input!.ReferalCode,
                                    RouteZoneCode = input!.RouteZoneCode,
                                    SubAreaCode = input!.SubAreaCode,
                                    SaleOrgCode = input!.SalesOrgCode,
                                    BudgetType = input!.BudgetType,
                                    AmountUsed = input!.BudgetType == "02" ? (decimal)output.BudgetBooked : 0,
                                    QuantityUsed = input!.BudgetType == "01" ? (int)output.BudgetBooked : 0,
                                    SubRegionCode = input!.SubRegionCode
                                };
                                _serviceBudgetUsed.Insert(use);
                                var currentBudget = _serviceBudget.GetById(query5.Id);
                                currentBudget.BudgetAvailable = query5.BudgetAvailable;
                                currentBudget.BudgetUsed = query5.BudgetUsed;
                                currentBudget.UpdatedDate = DateTime.Now;


                                var allotment = _serviceBudgetAllotment.GetAllQueryable().FirstOrDefault(e => e.BudgetCode.Equals(input!.BudgetCode));
                                if (input.SalesTerritoryValueCode is not null)
                                {
                                    allotment = _serviceBudgetAllotment.GetAllQueryable().FirstOrDefault(e => e.BudgetCode.Equals(input!.BudgetCode) && e.SalesTerritoryValueCode!.Equals(input.SalesTerritoryValueCode));
                                    allotment.BudgetQuantityWait = (decimal)query4!.STBudgetAvailable;
                                    allotment.BudgetQuantityUsed = (decimal)query4!.STBudgetUsed;
                                    allotment.UpdatedDate = DateTime.Now;
                                    _serviceBudgetAllotment.Update(allotment);
                                }
                                    

                                _serviceBudget.Update(currentBudget);
                                #endregion 
                            }

                            #endregion
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Nếu Khác NationWide
                   
                    if (query4 is not null && query4.BudgetAllocationForm.Equals("02"))
                    {
                        #region Điểm Bán
                        if (query4 is null)
                        {
                            output.Message = "NO_FOUND_BUDGET";
                            result.Data = output;
                            result.Success = true;
                            return result;
                        }
                        else if (query3 is not null && (query3!.AmountUsed + query3!.QuantityUsed >= query4!.STLimitBudgetPerCustomer) && (query3!.AmountUsed + query3!.QuantityUsed + input!.BudgetBook) > query4!.STLimitBudgetPerCustomer)
                        {
                            output.Message = "OVER_LIMIT_PER_CUSTOMER";
                            result.Data = output;
                            result.Success = true;
                            return result;
                        }
                        //else if (query3 is null && input.BudgetBook > query4.STLimitBudgetPerCustomer)
                        //{
                        //    output.Message = "OVER_LIMIT_PER_CUSTOMER";
                        //    result.Data = output;
                        //    result.Success = true;
                        //    return result;
                        //}
                        else if (query4!.STBudgetAvailable == 0 && input!.BudgetBook > 0)
                        {
                            output.Message = "NO_BUDGET_AVAILABLE";
                            result.Data = output;
                            result.Success = true;
                            return result;
                        }
                        else
                        {
                            #region update điểm bán
                            output.BudgetBooked = input!.BudgetBook;
                            if (query4.STBudgetAvailable < input!.BudgetBook)
                            {
                                output.BudgetBooked = (double)query4!.STBudgetAvailable;
                                query4.STBudgetAvailable = 0;
                                query4.STBudgetUsed = query4!.STBudgetUsed + (double)output.BudgetBooked;

                                query4.BudgetAvailable = 0;
                                query4.BudgetUsed = query4!.BudgetUsed + (double)output.BudgetBooked;
                            }
                            
                            else
                            {
                                if(query4!.STLimitBudgetPerCustomer >= input!.BudgetBook)
                                {
                                    output.BudgetBooked = input!.BudgetBook;
                                    query4.STBudgetAvailable = query4!.STBudgetAvailable + (input!.BudgetBook * -1);
                                    query4.STBudgetUsed = query4!.STBudgetUsed + input!.BudgetBook;

                                    query4.BudgetAvailable = query4!.BudgetAvailable + (input!.BudgetBook * -1);
                                    query4.BudgetUsed = query4!.BudgetUsed + input!.BudgetBook;
                                }
                                else
                                {
                                    input!.BudgetBook = (double)query4!.STLimitBudgetPerCustomer;
                                    output.BudgetBooked = input!.BudgetBook;
                                    query4.STBudgetAvailable = query4!.STBudgetAvailable + (input!.BudgetBook * -1);
                                    query4.STBudgetUsed = query4!.STBudgetUsed + input!.BudgetBook;

                                    query4.BudgetAvailable = query4!.BudgetAvailable + (input!.BudgetBook * -1);
                                    query4.BudgetUsed = query4!.BudgetUsed + input!.BudgetBook;
                                }
                                
                            }

                            TpBudgetUsed use = new TpBudgetUsed()
                            {
                                CountryCode = input!.NationwideCode,
                                AreaCode = input!.AreaCode,
                                BudgetCode = input!.BudgetCode,
                                DSACodeCode = input!.DSACode,
                                BranchCode = input!.BranchCode,
                                CustomerCode = input.CustomerCode,
                                RegionCode = input!.RegionCode,
                                ShiptoCode = input!.CustomerShipTo,
                                Key = input!.ReferalCode,
                                RouteZoneCode = input!.RouteZoneCode,
                                SubAreaCode = input!.SubAreaCode,
                                SaleOrgCode = input!.SalesOrgCode,
                                BudgetType = input!.BudgetType,
                                AmountUsed = input!.BudgetType == "02" ? (decimal)output.BudgetBooked : 0,
                                QuantityUsed = input!.BudgetType == "01" ? (int)output.BudgetBooked : 0,
                                SubRegionCode = input!.SubRegionCode
                            };
                            _serviceBudgetUsed.Insert(use);
                            var currentBudget = _serviceBudget.GetById(query4.Id);

                            var allotment = _serviceBudgetAllotment.GetAllQueryable().FirstOrDefault(e => e.BudgetCode.Equals(input!.BudgetCode));
                            if (input.SalesTerritoryValueCode is not null)
                            {
                                allotment = _serviceBudgetAllotment.GetAllQueryable().FirstOrDefault(e => e.BudgetCode.Equals(input!.BudgetCode) && e.SalesTerritoryValueCode!.Equals(input.SalesTerritoryValueCode));
                                allotment.BudgetQuantityWait = (decimal)query4!.STBudgetAvailable;
                                allotment.BudgetQuantityUsed = (decimal)query4!.STBudgetUsed;
                                allotment.UpdatedDate = DateTime.Now;
                                _serviceBudgetAllotment.Update(allotment);
                            }

                            currentBudget.BudgetAvailable = (double)query4!.BudgetAvailable;
                            currentBudget.BudgetUsed = (double)query4!.BudgetUsed;
                            currentBudget.UpdatedDate = DateTime.Now;
                            _serviceBudget.Update(currentBudget);
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region Khu vực                       
                        if ((query4 is null || query4!.BudgetAvailable == 0 || query4!.STBudgetAvailable == 0) && input!.BudgetBook >= 0)
                        {
                            output.Message = "NO_BUDGET_AVAILABLE";
                            result.Data = output;
                            result.Success = true;
                            return result;
                        }
                        else
                        {
                            #region update Khu vực
                            output.BudgetBooked = input!.BudgetBook;
                            if (query4.STBudgetAvailable < input!.BudgetBook)
                            {
                                output.BudgetBooked = (double)query4.STBudgetAvailable;
                                query4.STBudgetAvailable = 0;
                                query4.STBudgetUsed = query4.STBudgetUsed + (double)output.BudgetBooked;

                                if((double)query4.BudgetAvailable <= output.BudgetBooked)
                                     query4.BudgetAvailable = 0;
                                else
                                    query4.BudgetAvailable = query4!.BudgetAvailable + ((double)output.BudgetBooked * -1);
                                query4.BudgetUsed = query4!.BudgetUsed + (double)output.BudgetBooked;
                            }
                            else
                            {
                                output.BudgetBooked = input!.BudgetBook;
                                query4.STBudgetAvailable = query4!.STBudgetAvailable + (input!.BudgetBook * -1);
                                query4.STBudgetUsed = query4!.STBudgetUsed + input!.BudgetBook;

                                query4.BudgetAvailable = query4!.BudgetAvailable + (input!.BudgetBook * -1);
                                query4.BudgetUsed = query4!.BudgetUsed + input!.BudgetBook;
                            }

                            TpBudgetUsed use = new TpBudgetUsed()
                            {
                                CountryCode = input.CustomerCode,
                                AreaCode = input!.AreaCode,
                                BudgetCode = input!.BudgetCode,
                                DSACodeCode = input!.DSACode,
                                BranchCode = input!.BranchCode,
                                CustomerCode = input.CustomerCode,
                                RegionCode = input!.RegionCode,
                                ShiptoCode = input!.CustomerShipTo,
                                Key = input!.ReferalCode,
                                RouteZoneCode = input!.RouteZoneCode,
                                SubAreaCode = input!.SubAreaCode,
                                SaleOrgCode = input!.SalesOrgCode,
                                BudgetType = input!.BudgetType,
                                AmountUsed = input!.BudgetType == "02" ? (decimal)output.BudgetBooked : 0,
                                QuantityUsed = input!.BudgetType == "01" ? (int)output.BudgetBooked : 0,
                                SubRegionCode = input!.SubRegionCode
                            };
                            _serviceBudgetUsed.Insert(use);
                            var currentBudget = _serviceBudget.GetById(query4.Id);                          
                            var allotment = _serviceBudgetAllotment.GetAllQueryable().FirstOrDefault(e => e.BudgetCode.Equals(input!.BudgetCode) && e.SalesTerritoryValueCode!.Equals(input.SalesTerritoryValueCode));
                            allotment.BudgetQuantityWait = (decimal)query4.STBudgetAvailable;
                            allotment.BudgetQuantityUsed = (decimal)query4.STBudgetUsed;
                            allotment.UpdatedDate = DateTime.Now;
                            _serviceBudgetAllotment.Update(allotment);

                            currentBudget.BudgetAvailable = (double)query4!.BudgetAvailable;
                            currentBudget.BudgetUsed = (double)query4!.BudgetUsed;
                            currentBudget.UpdatedDate = DateTime.Now;
                            _serviceBudget.Update(currentBudget);
                            #endregion
                        }
                        #endregion
                    }
                    output.BudgetBookOver = query4.FlagOverBudget;


                    #endregion
                }
                #endregion

                output.Message = string.IsNullOrEmpty(output.Message) ? null : output.Message;
                output.Status = true;
                result.Data = output;
                result.Success = true;

            }
            catch (Exception ex)
            {
                result.Messages.Add(ex.Message);
            }

            return result;
        }
    }
}
