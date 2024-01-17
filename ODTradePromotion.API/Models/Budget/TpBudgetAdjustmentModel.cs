using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Models.Budget
{
    public class TpBudgetAdjustmentModel
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string BudgetCode { get; set; }
        public decimal BudgetQuantity { get; set; }
        public decimal BudgetQuantityNew { get; set; }
        public decimal TotalAmountAllotment { get; set; }
        public decimal TotalAmountAllotmentNew { get; set; }
        public decimal BudgetQuantityUsed { get; set; }
        public DateTime? AdjustmentDate { get; set; }
        public string Account { get; set; }
        [MaxLength(255)]
        public string Reason { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FolderType { get; set; }
        public string FileExt { get; set; }
        public int CountAdjustment { get; set; }
        public int Type { get; set; }

        [Required]
        public List<TpBudgetAllotmentAdjustmentModel> BudgetAllotmentAdjustments { get; set; }
        [Required]
        public TpBudgetModel Budget { get; set; }
    }
}
