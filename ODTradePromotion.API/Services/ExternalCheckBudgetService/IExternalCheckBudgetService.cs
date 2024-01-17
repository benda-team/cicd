using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.Budget;
using ODTradePromotion.API.Models.CheckBudget;
using ODTradePromotion.API.Models.External;
using Sys.Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace ODTradePromotion.API.Services.ExternalCheckBudgetService
{
    public interface IExternalCheckBudgetService
    {
        public List<BudgetRemainAndCustomerBudgetModel> SyncBudget(FilterBudgetRemainAndCustomerBudget request);
        public List<ConfigBudgetModel> GetConfigBudget(FilterConfigBudget request);
        Result<CheckBudgetOutput> CheckBudget(SyncCheckBudgetModel input);
    }
}
