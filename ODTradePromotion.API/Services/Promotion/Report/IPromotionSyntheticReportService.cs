using ODTradePromotion.API.Models.Report;
using System.Linq;

namespace ODTradePromotion.API.Services.Promotion.Report
{
    public interface IPromotionSyntheticReportService
    {
        public IQueryable<PromotionSyntheticReportListModel> GetListPromotionSyntheticReport();
        public IQueryable<BudgetInfo> GetListBudget();
    }
}
