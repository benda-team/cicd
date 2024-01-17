using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.Paging;
using ODTradePromotion.API.Models.SalesOrg;
using ODTradePromotion.API.Services.SalesOrg;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using static Sys.Common.Constants.ErrorCodes;

namespace ODTradePromotion.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    [ApiVersion("1.0")]
    public class SalesOrgController : Controller
    {
        private readonly ISalesOrgService _salesOrgService;
        public SalesOrgController(ISalesOrgService salesOrgService)
        {
            _salesOrgService = salesOrgService;
        }

        /// <summary>
        /// Get List Sales Org
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListSalesOrg")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListSalesOrg(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _salesOrgService.GetListSalesOrg();
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpSalesOrganizationModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpSalesOrganizationModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpSalesOrganizationModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.Code);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<TpSalesOrganizationModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new TpSalesOrganizationListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpSalesOrganizationModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new TpSalesOrganizationListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(SalesOrgError.GetListSalesOrgFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Sales Territory Level by TerritoryStructureCode
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListSalesTerritoryLevel")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListSalesTerritoryLevel(TerritoryLevelEcoParameters parameters)
        {
            try
            {
                var featureListTemp = _salesOrgService.GetListSalesTerritoryLevel(parameters.TerritoryStructureCode);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpTerritoryStructureLevelModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpTerritoryStructureLevelModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpTerritoryStructureLevelModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.TerritoryLevelCode);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<TpTerritoryStructureLevelModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new TpTerritoryStructureDetailListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpTerritoryStructureLevelModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new TpTerritoryStructureDetailListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(SalesOrgError.GetListSalesTerritoryLevelFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Sales Territory Value by territoryStructureCode and territoryLevelCode
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListSalesTerritoryValue")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListSalesTerritoryValue(TerritoryValueEcoParameters parameters)
        {
            try
            {
                var featureListTemp = _salesOrgService.GetListSalesTerritoryValue(parameters.TerritoryStructureCode, parameters.TerritoryLevelCode);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpSalesTerritoryValueModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpSalesTerritoryValueModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpSalesTerritoryValueModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.Code);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<TpSalesTerritoryValueModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new TpSalesTerritoryValueListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpSalesTerritoryValueModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new TpSalesTerritoryValueListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(SalesOrgError.GetListSalesTerritoryValueFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Sales Dsa Value
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListSalesDsaValue")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListSalesDsaValue(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _salesOrgService.GetListSalesDsaValue();
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpSalesOrgDsaModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpSalesOrgDsaModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpSalesOrgDsaModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.Code);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<TpSalesOrgDsaModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new TpSalesOrgDsaListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpSalesOrgDsaModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new TpSalesOrgDsaListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(SalesOrgError.GetListSalesDsaValueFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}
