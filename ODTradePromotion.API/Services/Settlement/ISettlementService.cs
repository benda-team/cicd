using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.Budget;
using ODTradePromotion.API.Models.Settlement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ODTradePromotion.API.Services.Settlement
{
    public interface ISettlementService
    {
        public List<TpSettlementModel> GetListSettlement();
        public IQueryable<TpSettlementModel> GetListSettlementForPopup();
        public List<string> SuggestionCode(string keyWord);
        public IQueryable<TpSettlementSearchModel> GetListSettlementCode(string status);
        public TpSettlementModel GetSettlementByCode(string code);
        public TpSettlementModel GetSettlementDetailByCode(string code);
        public bool Delete(string code, string userlogin);
        public BaseResultModel CreateSettlement(TpSettlementModel input, string userLogin);
        public BaseResultModel UpdateSettlement(TpSettlementModel input, string userLogin);
        public BaseResultModel ConfirmSettlementByDistributor(string distributorCode, List<string> lstInput);

        public List<TpDefinitionStructureForSettlementModel> GetsTpDefinitionStructuresForSettlement(string distributorCode);
        public List<TpSettlementConfirmModel> GetListSettlementConfirm();
        public IQueryable<TpSettlementConfirmModel> GetListSettlementConfirmByDistributor(string distributorCode);
        public IQueryable<TpSettlementDetailModel> GetListDetailSettlementConfirmByDistributor(string code, string distributorCode);
        public IQueryable<TpSettlementObjectWithCalendarModel> GetSettlementByPromotionCodeBySaleCalendar(string code, string calendar);
        public IQueryable<TpSettlementObjectWithCalendarModel> GetSettlementByDiscountCodeBySaleCalendar(string code, string calendar);
    }
}
