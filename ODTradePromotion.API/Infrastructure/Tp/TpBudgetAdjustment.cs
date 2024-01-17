using Sys.Common.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpBudgetAdjustment : TpAuditableEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string BudgetCode { get; set; }
        public decimal BudgetQuantity { get; set; }
        public decimal BudgetQuantityNew { get; set; }
        public decimal TotalAmountAllotment { get; set; }
        public decimal TotalAmountAllotmentNew { get; set; }
        public decimal BudgetQuantityUsed { get; set; }
        public DateTime AdjustmentDate { get; set; }
        [MaxLength(100)]
        public string Account { get; set; }
        [MaxLength(255)]
        public string Reason { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FolderType { get; set; }
        public string FileExt { get; set; }
        public int CountAdjustment { get; set; }
        public int Type { get; set; } // Adjustment type: 1 - budget adjustment, 2 - budget allotment adjustment
    }
}
