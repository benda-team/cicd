using AutoMapper;
using Elastic.Apm.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Infrastructure.Tp;
using ODTradePromotion.API.Models.Report;
using ODTradePromotion.API.Services.Base;
using Sys.Common.Constants;
using System.Linq;
using static Sys.Common.Constants.CommonData;

namespace ODTradePromotion.API.Services.Promotion.Report
{
    public class PromotionSyntheticReportService : IPromotionSyntheticReportService
    {
        #region Property
        private readonly ILogger<PromotionSyntheticReportService> _logger;
        private readonly IBaseRepository<TpPromotion> _servicePromotion;
        private readonly IBaseRepository<TpPromotionDefinitionStructure> _servicePromotionDefinitionStructure;
        private readonly IBaseRepository<TpPromotionDefinitionProductForGift> _servicePromotionDefinitionProductForGift;
        private readonly IBaseRepository<Infrastructure.SystemSetting> _systemSettingService;
        private readonly IBaseRepository<TpBudget> _serviceBudget;
        private readonly IBaseRepository<TpBudgetDefine> _serviceBudgetDefine;
        private readonly IBaseRepository<Uom> _serviceUom;
        private readonly IBaseRepository<InventoryItem> _serviceInventoryItem;
        #endregion

        #region Constructor
        public PromotionSyntheticReportService(ILogger<PromotionSyntheticReportService> logger,
            IBaseRepository<TpPromotion> servicePromotion,
            IBaseRepository<TpPromotionDefinitionStructure> servicePromotionDefinitionStructure,
            IBaseRepository<TpPromotionDefinitionProductForGift> servicePromotionDefinitionProductForGift,
            IBaseRepository<Infrastructure.SystemSetting> systemSettingService,
            IBaseRepository<TpBudget> serviceBudget,
            IBaseRepository<TpBudgetDefine> serviceBudgetDefine,
            IBaseRepository<Uom> serviceUom,
            IBaseRepository<InventoryItem> serviceInventoryItem
            )
        {
            _logger = logger;
            _servicePromotion = servicePromotion;
            _servicePromotionDefinitionStructure = servicePromotionDefinitionStructure;
            _servicePromotionDefinitionProductForGift = servicePromotionDefinitionProductForGift;
            _systemSettingService = systemSettingService;
            _serviceBudget = serviceBudget;
            _serviceBudgetDefine = serviceBudgetDefine;
            _serviceUom = serviceUom;
            _serviceInventoryItem = serviceInventoryItem;
        }
        #endregion

        #region Method
        public IQueryable<PromotionSyntheticReportListModel> GetListPromotionSyntheticReport()
        {
            var systemSettings = _systemSettingService.GetAllQueryable(x => x.IsActive).AsQueryable();

            

            var results = (from p in _servicePromotion.GetAllQueryable(x => x.DeleteFlag == 0 && x.Status.Equals(PromotionSetting.Confirmed))

                           join scope in systemSettings.Where(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.MktScope.ToLower())).AsNoTracking()
                           on p.ScopeType equals scope.SettingKey

                           join applicable in systemSettings.Where(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.ApplicableObject.ToLower())).AsNoTracking()
                           on p.ApplicableObjectType equals applicable.SettingKey

                           join defineProductGift in _servicePromotionDefinitionProductForGift.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                           on p.Code equals defineProductGift.PromotionCode into defineProductGiftData
                           from defineProductGift in defineProductGiftData.DefaultIfEmpty()

                           join defineStructure in _servicePromotionDefinitionStructure.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking() on
                           new { Level_Code = defineProductGift.LevelCode, Promotion_Code = defineProductGift.PromotionCode } equals
                           new { Level_Code = defineStructure.LevelCode, Promotion_Code = defineStructure.PromotionCode }
                           into defineStructureData
                           from defineStructure in defineStructureData.DefaultIfEmpty()

                           join uom in _serviceUom.GetAllQueryable().AsNoTracking() on defineProductGift.Packing equals uom.UomId
                           into uomData
                           from uom in uomData.DefaultIfEmpty()

                           join product in _serviceInventoryItem.GetAllQueryable().AsNoTracking() on defineProductGift.ProductCode equals product.InventoryItemId
                           into productData
                           from product in productData.DefaultIfEmpty()

                           join budget in _serviceBudget.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking() on defineProductGift.BudgetCode equals budget.Code into budgetData
                           from budget in budgetData.DefaultIfEmpty()

                           join budgetDefine in _serviceBudgetDefine.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking() on budget.Code equals budgetDefine.BudgetCode
                           into budgetDefineData
                           from budgetDefine in budgetDefineData.DefaultIfEmpty()

                           select new PromotionSyntheticReportListModel()
                           {
                               Code = p.Code,
                               ShortName = p.ShortName,
                               EffectiveDateFrom = p.EffectiveDateFrom,
                               ValidUntil = p.ValidUntil,
                               ScopeType = p.ScopeType,
                               ScopeTypeDescription = scope != null ? scope.Description : string.Empty,
                               ApplicableObjectType = p.ApplicableObjectType,
                               ApplicableObjectTypeDescription = applicable != null ? applicable.Description : string.Empty,
                               Packing = uom != null ? uom.Description : string.Empty,
                               LevelName = defineStructure.LevelName,
                               ProductName = product != null ? product.Description : string.Empty,
                               DonateApplyBudgetCode = defineStructure.DonateApplyBudgetCode,
                               GiftApplyBudgetCode = defineStructure.GiftApplyBudgetCode
                           }).AsQueryable();
            
            return results;
        }
        public IQueryable<BudgetInfo> GetListBudget()
        {


            var budgetGift = (from p in _servicePromotion.GetAllQueryable(x => x.DeleteFlag == 0 && x.Status.Equals(PromotionSetting.Confirmed))
                              join tpds in _servicePromotionDefinitionStructure.GetAll() on p.Code equals tpds.PromotionCode

                              join tb in _serviceBudget.GetAll() on tpds.GiftApplyBudgetCode equals tb.Code
                              select new BudgetInfo
                              {
                                  Code = p.Code,
                                  GiftApplyBudgetCode = tpds.GiftApplyBudgetCode,
                                  DonateApplyBudgetCode = "",
                                  BudgetCode = tb.Code,
                                  BudgetName = tb.Name,
                                  BudgetQuantity = tb.TotalBudget,
                                  BudgetQuantityUsed = tb.BudgetUsed
                              });
            var budgetDonate = (from p in _servicePromotion.GetAllQueryable(x => x.DeleteFlag == 0 && x.Status.Equals(PromotionSetting.Confirmed))
                                join tpds in _servicePromotionDefinitionStructure.GetAll() on p.Code equals tpds.PromotionCode

                                join tb in _serviceBudget.GetAll() on tpds.DonateApplyBudgetCode equals tb.Code
                                select new BudgetInfo
                                {
                                    Code = p.Code,
                                    GiftApplyBudgetCode = "",
                                    DonateApplyBudgetCode = tpds.DonateApplyBudgetCode,
                                    BudgetCode = tb.Code,
                                    BudgetName = tb.Name,
                                    BudgetQuantity = tb.TotalBudget,
                                    BudgetQuantityUsed = tb.BudgetUsed
                                });
            var budgetAll = budgetGift.Union(budgetDonate);

            return budgetAll;
        }
        #endregion
    }
}
