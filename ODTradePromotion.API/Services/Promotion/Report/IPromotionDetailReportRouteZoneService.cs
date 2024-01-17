using ODTradePromotion.API.Models.Report;
using System.Linq;

namespace ODTradePromotion.API.Services.Promotion.Report
{
    public interface IPromotionDetailReportRouteZoneService
    {
        public IQueryable<PromotionDetailReportRouteZoneListModel> GetRouteZonesOrderForPopupPromotionReport(PromotionReportEcoParameters request);
        public IQueryable<PromotionDetailReportRouteZoneListModel> GetRouteZonesOrderForPromotionReport(PromotionReportEcoParameters request);
    }
}
