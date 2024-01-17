using Dapper;
using ODTradePromotion.API.Models.Base;
using ODTradePromotion.API.Models.Budget;
using ODTradePromotion.API.Models.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.Promotion
{
    public interface IPromotionService
    {
        public IQueryable<PromotionDiscountModel> GetListTpPromotion();
        public Task<List<PromotionDiscountModel>> GetListTpPromotionDapper(PromotionDiscountSearchModel parameters);
        public IQueryable<PromotionPopupModel> GetListPromotionPopup(string userName, string promotionType);
        public IQueryable<TpPromotionGeneralModel> GetListPromotionGeneral();
        public IQueryable<TpPromotionGeneralModel> GetListPromotionGeneralForSelettlement(string calendar);
        public IQueryable<TpPromotionSearchModel> GetListPromotionCode(string status);
        public Task<List<TpPromotionSearchModel>> GetListPromotionCodeDapper(string status);
        public TpPromotionModel GetGeneralPromotionByCode(string code);
        public TpPromotionModel GetDetailPromotionByCode(string code);
        public Task<TpPromotionModel> GetDetailPromotionByCodeDapper(string code);
        public Task<BaseResultModel> CreatePromotion(TpPromotionModel input, string userLogin, bool isSync = false);
        Task UpdateSyncedFlagAsync(string promotionCode, string userLogin);
        public bool UpdatePromotion(TpPromotionModel input, string userLogin);
        public bool ConfirmPromotion(ConfirmPromotionReq request);
        public bool DeletePromotionByCode(string code, string userLogin);
        public List<TpPromotionDefinitionProductForGiftModel> GetListProductForGiftByPromotionCode(string promotionCode);
        public List<TpBudgetModel> GetListBudgetByPromotionCode(string promotionCode);
        public PromotionDefinitionForSettlementModel GetPromotionDefinitionForSettlement(string promotionCode);
        public Task<PromotionInitialModel> GetDataInitialPromotionMain();
        public Task<PromotionInitialModel> GetDataInitialPromotionMainByDapper();
        #region External API
        public Task<List<PromotionExternalModel>> GetListPromotionByCustomer(ListPromotionAndDiscountRequestModel request);
        public Task<TpPromotionModel> GetDetailPromotionExternalByCode(string code);
        public List<ItemGroupModel> ExternalGetListItemGroupByItemHierarchy(string ItemHierarchyLevel, string ItemHierarchyCode);
        public Task<PromotionResultResponseModel> GetPromotionResult(PromotionResultRequestModel request);
        public Task<PromotionBudgetResponse> ExtenalApiCheckBudgetInfoPromotion(PromotionBudgetRequest request);
        public Task<ListPromotionAndDiscountResponseModel> ExternalApiGetListPromotionAndDiscount(ListPromotionAndDiscountRequestModel request);
        #endregion
    }
}
