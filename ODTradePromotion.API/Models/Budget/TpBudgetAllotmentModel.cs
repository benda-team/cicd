using System;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Models.Budget
{
    public class TpBudgetAllotmentModel
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string BudgetCode { get; set; }
        [MaxLength(10)]
        public string SalesTerritoryValueCode { get; set; }
        public string SalesTerritoryValueDescription { get; set; }
        public decimal BudgetQuantityDetail { get; set; }
        public decimal BudgetQuantityUsed { get; set; }
        public bool FlagBudgetQuantityLimitDetail { get; set; }
        public decimal BudgetQuantityLimitDetail { get; set; }
        public decimal BudgetQuantityWait { get; set; }
        public double? LimitBudgetPerCustomer { get; set; }
    }
}
