using ODTradePromotion.API.Models.Budget;
using System;
using System.Linq;

namespace ODTradePromotion.API.Services.Budget
{
    public interface IBudgetAdjustmentService
    {
        public IQueryable<TpBudgetAdjustmentListModel> GetListBudgetAdjustment(int type, string budgetCode);
        public TpBudgetAdjustmentModel GetHistoryBudgetAdjustment(string budgetCode, int type, int countAdjustment);
        public bool CreateBudgetAdjustment(TpBudgetAdjustmentModel input, string userlogin);
    }
}
