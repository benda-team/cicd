using ODTradePromotion.API.Models.Report;
using System.Linq;

namespace ODTradePromotion.API.Services.Promotion.Report
{
    public interface IPromotionDetailReportOrderService
    {
        public IQueryable<PromotionDetailReportOrderListModel> GetOrdersForPopupPromotionReport(PromotionReportEcoParameters request);
        public IQueryable<PromotionDetailReportOrderListModel> GetOrdersForPromotionReport(PromotionReportEcoParameters request);
    }
}
