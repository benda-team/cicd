using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.Budget;
using System.Collections.Generic;
using System.Linq;

namespace ODTradePromotion.API.Services.Budget
{
    public interface IBudgetService
    {
        public IQueryable<TpBudgetListModel> GetListBudget();
        public IQueryable<TpBudgetListModel> GetListBudgetForPopup(string type);
        IQueryable<TpBudgetListModel> GetListBudgetForPopup();
        public List<TpBudgetListModel> GetListBudgetByTypeAndCodeOfProduct(FilterBudgetByProductModel input);
        public List<TpBudgetSearchModel> GetListBudgetCode();
        public TpBudgetModel GetBudgetByCode(string code);
        public TpBudgetModel GetBudgetByCodeGeneral(string code);
        public void CreateBudget(TpBudgetModel input, string userlogin);
        public bool UpdateBudget(TpBudgetModel input, string userlogin);
        public bool UpdateDataBudget(TpBudgetModel input, string userlogin);
        public bool DeleteBudgetByCode(string code, string userlogin);
        public BaseResultModel UpdateBudgetQuantityUsed(string code, decimal QuantityUsed, string userlogin);
    }
}
