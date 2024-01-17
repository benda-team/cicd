using Sys.Common.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpBudget : TpAuditableEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        public string IO { get; set; }
        [Required]
        [MaxLength(10)]
        public string SaleOrg { get; set; }
        [Required]
        [MaxLength(10)]
        public string BudgetType { get; set; }
        [Required]
        [MaxLength(100)]
        public string BudgetAllocationForm { get; set; }
        [MaxLength(10)]
        public string BudgetAllocationLevel { get; set; }
        public bool FlagOverBudget { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FolderType { get; set; }
        public string FileExt { get; set; }
        [Required]
        [MaxLength(10)]
        public string Status { get; set; }
        public double? TotalBudget { get; set; }
        public double? BudgetUsed { get; set; }
        public double? BudgetAvailable { get; set; }
        public double? LimitBudgetPerCustomer { get; set; }
    }
}
