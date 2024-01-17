using System;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Models.Budget
{
    public class TpBudgetDefineModel
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string BudgetCode { get; set; }
        [MaxLength(10)]
        public string PromotionProductType { get; set; }
        [MaxLength(10)]
        public string PromotionProductCode { get; set; }
        [MaxLength(100)]
        public string PackSize { get; set; }
        public decimal BudgetQuantity { get; set; }
        public decimal BudgetQuantityUsed { get; set; }
        [MaxLength(100)]
        public string ItemHierarchyLevel { get; set; }
        [MaxLength(100)]
        public string ItemHierarchyValue { get; set; }
        public decimal TotalAmountAllotment { get; set; }
    }
}
