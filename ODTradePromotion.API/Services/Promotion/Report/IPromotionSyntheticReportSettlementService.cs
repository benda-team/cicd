using ODTradePromotion.API.Models.Promotion;
using ODTradePromotion.API.Models.Report;
using System.Linq;

namespace ODTradePromotion.API.Services.Promotion.Report
{
    public interface IPromotionSyntheticReportSettlementService
    {
        public IQueryable<PromotionDiscountForPopupReportModel> GetListPromotionSettlementForPopup();
        public IQueryable<PromotionSyntheticReportSettlementListModel> GetListPromotionSyntheticReportSettlement(string promotionDiscountCode);
        public IQueryable<DistributorPopupReportSettlementListModel> GetListDistributorPopupReportSettlement(string settlementCode);
        public int CountSettlementQuantity(string promotionDiscountCode);
    }
}
