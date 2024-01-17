using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Budget
{
    public class BudgetQuantityUsedModel
    {
        public string BudgetCode { get; set; }
        public decimal BudgetQuantityUsed { get; set; }
        public string UserLogin { get; set; }
    }
}
