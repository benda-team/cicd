using Sys.Common.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpBudgetAllotment : TpAuditableEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string BudgetCode { get; set; }
        [MaxLength(10)]
        public string SalesTerritoryValueCode { get; set; }
        public decimal BudgetQuantityDetail { get; set; }
        public decimal BudgetQuantityWait { get; set; }
        public decimal BudgetQuantityUsed { get; set; }
        public bool FlagBudgetQuantityLimitDetail { get; set; }
        public decimal BudgetQuantityLimitDetail { get; set; }
        public double? LimitBudgetPerCustomer { get; set; }
    }
}
