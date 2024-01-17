using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Infrastructure.Tp;
using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.Budget;
using ODTradePromotion.API.Services.Base;
using Sys.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ODTradePromotion.API.Services.Budget
{
    public class BudgetService : IBudgetService
    {
        #region Property
        private readonly ILogger<BudgetService> _logger;
        private readonly IBaseRepository<TpBudget> _serviceBudget;
        private readonly IBaseRepository<TpBudgetDefine> _serviceBudgetDefine;
        private readonly IBaseRepository<TpBudgetAllotment> _serviceBudgetAllotment;
        private readonly IBaseRepository<SystemSetting> _systemSettingService;
        private readonly IBaseRepository<ScTerritoryValue> _serviceTerritoryValue;
        private readonly IBaseRepository<ScTerritoryStructureDetail> _serviceTerritoryStructureDetail;
        private readonly IBaseRepository<ScSalesOrganizationStructure> _serviceSaleSalesOrganization;
        private readonly IMapper _mapper;
        private readonly IDapperRepositories _dapper;

        #endregion

        #region Constructor
        public BudgetService(ILogger<BudgetService> logger, IBaseRepository<TpBudget> serviceBudget,
            IBaseRepository<TpBudgetDefine> serviceBudgetDefine,
            IBaseRepository<TpBudgetAllotment> serviceBudgetAllotment,
            IBaseRepository<SystemSetting> systemSettingService,
            IBaseRepository<ScTerritoryValue> serviceTerritoryValue,
            IBaseRepository<ScTerritoryStructureDetail> serviceTerritoryStructureDetail,
            IBaseRepository<ScSalesOrganizationStructure> serviceSaleSalesOrganization,
            IMapper mapper,
            IDapperRepositories dapper
            )
        {
            _logger = logger;
            _serviceBudget = serviceBudget;
            _serviceBudgetDefine = serviceBudgetDefine;
            _serviceBudgetAllotment = serviceBudgetAllotment;
            _systemSettingService = systemSettingService;
            _serviceTerritoryValue = serviceTerritoryValue;
            _serviceTerritoryStructureDetail = serviceTerritoryStructureDetail;
            _serviceSaleSalesOrganization = serviceSaleSalesOrganization;
            _mapper = mapper;
            _dapper = dapper;
        }
        #endregion

        #region Method
        public void CreateBudget(TpBudgetModel input, string userlogin)
        {
            // Create budget
            var budget = _mapper.Map<TpBudget>(input);
            budget.Id = Guid.NewGuid();
            budget.CreatedBy = userlogin;
            budget.CreatedDate = DateTime.Now;
            budget.DeleteFlag = 0;
            budget.BudgetAvailable = budget.TotalBudget;
            budget.BudgetUsed = 0;
            _serviceBudget.Insert(budget);


            //Create BudgetAllotment
            var lstBudgetAllotmentCreateNew = CreateDataBudgetAllotmentsCreateNew(input.BudgetAllotmentModels, userlogin);
            _serviceBudgetAllotment.InsertRange(lstBudgetAllotmentCreateNew);

            // save into db
            _serviceBudget.Save();
        }

        public bool DeleteBudgetByCode(string code, string userlogin)
        {
            var budget = _serviceBudget.FirstOrDefault(f => f.Code == code && f.DeleteFlag == 0);
            if (budget != null)
            {
                // delete in Budeget
                budget.DeleteFlag = 1;
                budget.UpdatedBy = userlogin;
                budget.UpdatedDate = DateTime.Now;
                _serviceBudget.Update(budget);

                //delete in BudgetDefine
                var budgetDefine = _serviceBudgetDefine.FirstOrDefault(f => f.BudgetCode == budget.Code && f.DeleteFlag == 0);
                if (budgetDefine != null)
                {
                    budgetDefine.DeleteFlag = 1;
                    budgetDefine.UpdatedBy = userlogin;
                    budgetDefine.UpdatedDate = DateTime.Now;
                    _serviceBudgetDefine.Update(budgetDefine);
                }
                else
                {
                    return false;
                }

                //delete in BudgetAllotment
                var lstBudgetAllotmentDelete = CreateDataBudgetAllotmentsDelete(budget.Code, userlogin);
                if (lstBudgetAllotmentDelete == null)
                {
                    return false;
                }
                _serviceBudgetAllotment.UpdateRange(lstBudgetAllotmentDelete);

                // save into db
                _serviceBudget.Save();
                return true;
            }
            return false;
        }

        public TpBudgetModel GetBudgetByCode(string code)
        {
            var returnValue = _mapper.Map<TpBudgetModel>(_serviceBudget.GetAllQueryable().AsNoTracking()
                .Where(m => m.Code == code && m.DeleteFlag == 0).FirstOrDefault());
            if (returnValue != null)
            {
                returnValue.BudgetDefineModel = _mapper.Map<TpBudgetDefineModel>(_serviceBudgetDefine.GetAllQueryable()
                    .AsNoTracking().Where(m => m.BudgetCode == returnValue.Code && m.DeleteFlag == 0).FirstOrDefault());
                var dataBudgetAllotmentModels = (from b in _serviceBudgetAllotment.GetAllQueryable(x => x.DeleteFlag == 0 && x.BudgetCode.ToLower().Equals(code.ToLower())).AsNoTracking()
                                                 join t in _serviceTerritoryValue.GetAllQueryable().AsNoTracking() on
                                                 new { code = b.SalesTerritoryValueCode, territoryLevelCode = returnValue.BudgetAllocationLevel } equals
                                                 new { code = t.Code, territoryLevelCode = t.TerritoryLevelCode } into tData
                                                 from t in tData.DefaultIfEmpty()
                                                 select new TpBudgetAllotmentModel()
                                                 {
                                                     Id = b.Id,
                                                     BudgetCode = b.BudgetCode,
                                                     BudgetQuantityDetail = b.BudgetQuantityDetail,
                                                     BudgetQuantityLimitDetail = b.BudgetQuantityLimitDetail,
                                                     SalesTerritoryValueCode = b.SalesTerritoryValueCode,
                                                     FlagBudgetQuantityLimitDetail = b.FlagBudgetQuantityLimitDetail,
                                                     SalesTerritoryValueDescription = (t != null) ? t.Description : string.Empty,
                                                     BudgetQuantityUsed = b.BudgetQuantityUsed,
                                                     LimitBudgetPerCustomer = b.LimitBudgetPerCustomer
                                                 }).ToList();
                returnValue.BudgetAllotmentModels = new List<TpBudgetAllotmentModel>();
                returnValue.BudgetAllotmentModels = dataBudgetAllotmentModels;
            }
            return returnValue;
        }
        public TpBudgetModel GetBudgetByCodeGeneral(string code)
        {
            var returnValue = _mapper.Map<TpBudgetModel>(_serviceBudget.GetAllQueryable().AsNoTracking().FirstOrDefault(m => m.Code == code && m.DeleteFlag == 0));
            return returnValue;
        }

        public IQueryable<TpBudgetListModel> GetListBudget()
        {
            var systemSettings = _systemSettingService.GetAllQueryable(x => x.IsActive).AsQueryable();

            var listResult = (from b in _serviceBudget.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()

                              join saleOrgObj in _serviceSaleSalesOrganization.GetAllQueryable().AsNoTracking()
                              on b.SaleOrg equals saleOrgObj.Code into saleOrgData
                              from saleOrgObj in saleOrgData.DefaultIfEmpty()

                              join budgetAllocationLevelObj in _serviceTerritoryStructureDetail.GetAllQueryable().AsNoTracking() on
                              new { code = saleOrgObj.TerritoryStructureCode, territoryLevelCode = b.BudgetAllocationLevel } equals
                              new { code = budgetAllocationLevelObj.TerritoryStructureCode, territoryLevelCode = budgetAllocationLevelObj.TerritoryLevelCode } into dataBudgetAllocationLevel
                              from budgetAllocationLevelObj in dataBudgetAllocationLevel.DefaultIfEmpty()

                              join budgetAllocationFormObj in systemSettings.Where(x => x.SettingType == CommonData.SystemSetting.BudgetAllocationForm).AsNoTracking()
                              on b.BudgetAllocationForm equals budgetAllocationFormObj.SettingKey

                              join budgetTypeObj in systemSettings.Where(x => x.SettingType == CommonData.SystemSetting.BudgetType).AsNoTracking()
                              on b.BudgetType equals budgetTypeObj.SettingKey

                              join statusObj in systemSettings.Where(x => x.SettingType == CommonData.SystemSetting.TpStatus).AsNoTracking()
                              on b.Status equals statusObj.SettingKey

                              select new TpBudgetListModel
                              {
                                  Id = b.Id,
                                  Name = b.Name,
                                  IO = b.IO,
                                  Code = b.Code,
                                  SaleOrg = saleOrgObj.Description,
                                  BudgetAllocationForm = budgetAllocationFormObj.Description,
                                  BudgetAllocationLevel = budgetAllocationLevelObj == null ? b.BudgetAllocationLevel : budgetAllocationLevelObj.Description,
                                  BudgetType = budgetTypeObj.Description,
                                  Status = b.Status,
                                  StatusName = statusObj.Description
                              }).AsQueryable();
            return listResult;
        }
        public IQueryable<TpBudgetListModel> GetListBudgetForPopup(string type)
        {
            var listResult = (from b in _serviceBudget.GetAllQueryable(x => x.DeleteFlag == 0 && x.BudgetType.Equals(type)).AsNoTracking()
                              join statusObj in _systemSettingService.GetAllQueryable(x => x.IsActive && x.SettingType == CommonData.SystemSetting.TpStatus).AsQueryable().AsNoTracking()
                              on b.Status equals statusObj.SettingKey
                              select new TpBudgetListModel
                              {
                                  Id = b.Id,
                                  Name = b.Name,
                                  Code = b.Code,
                                  Status = b.Status,
                                  StatusName = statusObj.Description,
                                  BudgetAllocationLevel= b.BudgetAllocationLevel
                              }).AsQueryable();
            return listResult;
        }
        public IQueryable<TpBudgetListModel> GetListBudgetForPopup()
        {
            var listResult = (from b in _serviceBudget.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                              join statusObj in _systemSettingService.GetAllQueryable(x => x.IsActive && x.SettingType == CommonData.SystemSetting.TpStatus).AsQueryable().AsNoTracking()
                              on b.Status equals statusObj.SettingKey
                              select new TpBudgetListModel
                              {
                                  Id = b.Id,
                                  Name = b.Name,
                                  Code = b.Code,
                                  Status = b.Status,
                                  StatusName = statusObj.Description,
                                  BudgetAllocationLevel = b.BudgetAllocationLevel
                              }).AsQueryable();
            return listResult;
        }
        public List<TpBudgetSearchModel> GetListBudgetCode()
        {
            return _serviceBudget.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                .Where(x => x.Status.Equals(CommonData.PromotionSetting.StatusCanLinkPromotion) || x.Status.Equals(CommonData.PromotionSetting.StatusLinkedPromotion))
                .Select(x => new TpBudgetSearchModel { Code = x.Code, Name = x.Name })
                .OrderBy(x => x.Name)
                .ToList();
        }
        public List<TpBudgetListModel> GetListBudgetByTypeAndCodeOfProduct(FilterBudgetByProductModel input)
        {
            List<TpBudgetListModel> listResult;

            if (input.ProductType.Equals(CommonData.PromotionSetting.ItemHierarchyValue))
            {
                listResult = (from b in _serviceBudget.GetAllQueryable(x => x.DeleteFlag == 0 && !x.Status.ToLower().Equals(CommonData.PromotionSetting.StatusDefining)).AsNoTracking()

                              join budgetDefineObj in _serviceBudgetDefine.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                              on b.Code equals budgetDefineObj.BudgetCode into budgetDefineData
                              from budgetDefineObj in budgetDefineData.DefaultIfEmpty()

                              where budgetDefineObj.PromotionProductType.ToLower().Equals(input.ProductType.ToLower()) &&
                              budgetDefineObj.ItemHierarchyLevel.ToLower().Equals(input.ItemHierarchyLevel.ToLower())
                              && budgetDefineObj.ItemHierarchyValue.ToLower().Equals(input.ProductCode.ToLower()) &&
                              budgetDefineObj.PackSize.ToLower().Equals(input.Packing.ToLower())
                              select new TpBudgetListModel
                              {
                                  Id = b.Id,
                                  Name = b.Name,
                                  Code = b.Code
                              }).ToList();
            }
            else
            {
                listResult = (from b in _serviceBudget.GetAllQueryable(x => x.DeleteFlag == 0 && !x.Status.ToLower().Equals(CommonData.PromotionSetting.StatusDefining)).AsNoTracking()

                              join budgetDefineObj in _serviceBudgetDefine.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                              on b.Code equals budgetDefineObj.BudgetCode into budgetDefineData
                              from budgetDefineObj in budgetDefineData.DefaultIfEmpty()

                              where budgetDefineObj.PromotionProductType.ToLower().Equals(input.ProductType.ToLower()) &&
                              budgetDefineObj.PromotionProductCode.ToLower().Equals(input.ProductCode.ToLower()) &&
                              budgetDefineObj.PackSize.ToLower().Equals(input.Packing.ToLower())
                              select new TpBudgetListModel
                              {
                                  Id = b.Id,
                                  Name = b.Name,
                                  Code = b.Code
                              }).ToList();
            }

            return listResult;
        }

        public bool UpdateBudget(TpBudgetModel input, string userlogin)
        {
            var result = this.UpdateDataBudget(input, userlogin);
            if (result)
            {
                // save into db
                _serviceBudget.Save();
            }
            return result;
        }

        public bool UpdateDataBudget(TpBudgetModel input, string userlogin)
        {
            // update Budget
            var budget = _serviceBudget.FirstOrDefault(f => f.Code == input.Code && f.DeleteFlag == 0);
            if (budget != null)
            {
                var idBudgetTemp = budget.Id;
                _mapper.Map(input, budget);
                budget.Id = idBudgetTemp;
                budget.UpdatedBy = userlogin;
                budget.UpdatedDate = DateTime.Now;
                _serviceBudget.Update(budget);

                // update BudgetDefine
                var budgetDefine = _serviceBudgetDefine.FirstOrDefault(f => f.BudgetCode == budget.Code && f.DeleteFlag == 0);
                if (budgetDefine != null)
                {
                    var idBudgetDinfineTemp = budgetDefine.Id;
                    _mapper.Map(input.BudgetDefineModel, budgetDefine);
                    budgetDefine.Id = idBudgetDinfineTemp;
                    budgetDefine.UpdatedBy = userlogin;
                    budgetDefine.UpdatedDate = DateTime.Now;
                    _serviceBudgetDefine.Update(budgetDefine);
                }

                // Delete BudgetAllotment Old
                var lstBudgetAllotment = _serviceBudgetAllotment.GetAllQueryable(x => x.BudgetCode == budget.Code && x.DeleteFlag == 0);
                if (lstBudgetAllotment != null)
                {
                    _serviceBudgetAllotment.DeleteRange(lstBudgetAllotment);
                }

                // insert new BudgetAllotment
                _serviceBudgetAllotment.InsertRange(CreateDataBudgetAllotmentsCreateNew(input.BudgetAllotmentModels, userlogin));

                return true;
            }
            return false;
        }

        private List<TpBudgetAllotment> CreateDataBudgetAllotmentsCreateNew(List<TpBudgetAllotmentModel> lstInput, string userlogin)
        {
            var lstBudgetAllotmentCreateNew = new List<TpBudgetAllotment>();
            foreach (var item in lstInput)
            {
                var entity = _mapper.Map<TpBudgetAllotment>(item);
                entity.Id = Guid.NewGuid();
                entity.CreatedBy = userlogin;
                entity.CreatedDate = DateTime.Now;
                entity.DeleteFlag = 0;
                entity.BudgetQuantityWait = entity.BudgetQuantityDetail;
                lstBudgetAllotmentCreateNew.Add(entity);
            }
            return lstBudgetAllotmentCreateNew;
        }

        private List<TpBudgetAllotment> CreateDataBudgetAllotmentsDelete(string budgetCode, string userlogin)
        {
            var lstBudgetAllotmentDelete = new List<TpBudgetAllotment>();
            var lstBudgetAllotment = _serviceBudgetAllotment.GetAllQueryable(x => x.BudgetCode == budgetCode && x.DeleteFlag == 0).AsNoTracking().ToList();
            if (lstBudgetAllotment.Count == 0)
                return lstBudgetAllotmentDelete;
            foreach (var item in lstBudgetAllotment)
            {
                var entity = _mapper.Map<TpBudgetAllotment>(item);
                entity.DeleteFlag = 1;
                entity.UpdatedBy = userlogin;
                entity.UpdatedDate = DateTime.Now;
                lstBudgetAllotmentDelete.Add(entity);
            }
            return lstBudgetAllotmentDelete;
        }

        public BaseResultModel UpdateBudgetQuantityUsed(string code, decimal QuantityUsed, string userlogin)
        {
            var budgetDefine = _serviceBudgetDefine.FirstOrDefault(x => x.DeleteFlag == 0 && x.BudgetCode.ToLower().Equals(code.ToLower()));
            if (budgetDefine != null)
            {
                if (budgetDefine.BudgetQuantityUsed > QuantityUsed)
                {
                    budgetDefine.BudgetQuantityUsed -= QuantityUsed;
                    budgetDefine.UpdatedBy = userlogin;
                    budgetDefine.UpdatedDate = DateTime.Now;
                    _serviceBudgetDefine.Update(budgetDefine);
                }
                else
                {
                    return new BaseResultModel() { IsSuccess = false, Message = "QuantityIsNotEnough" };
                }

                return new BaseResultModel() { IsSuccess = true, Message = "UpdateBudgetQuantityUsedSuccess" };
            }
            return new BaseResultModel() { IsSuccess = false, Message = "BudgetCodeDoesNotExist" };
        }
        #endregion

    }
}

