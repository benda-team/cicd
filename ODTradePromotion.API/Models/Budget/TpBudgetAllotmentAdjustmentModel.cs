using System;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Models.Budget
{
    public class TpBudgetAllotmentAdjustmentModel
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string BudgetCode { get; set; }
        public string SalesTerritoryValueCode { get; set; }
        public decimal BudgetQuantityDetail { get; set; }
        public decimal BudgetQuantityDetailNew { get; set; }
        public decimal BudgetQuantityUsed { get; set; }
    }
}
