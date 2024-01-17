using ODTradePromotion.API.Models.Report;
using System.Linq;

namespace ODTradePromotion.API.Services.Promotion.Report
{
    public interface IPromotionDetailReportPointSaleService
    {
        public IQueryable<PromotionDetailReportPointSaleListModel> GetCustomersOrderForPopupPromotionReport(PromotionReportEcoParameters request);
        public IQueryable<PromotionDetailReportPointSaleListModel> GetCustomersOrderForPromotionReport(PromotionReportEcoParameters request);
    }
}
