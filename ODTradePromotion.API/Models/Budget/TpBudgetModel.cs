using ODTradePromotion.API.Infrastructure.Tp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Models.Budget
{
    public class TpBudgetModel
    {
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
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public double? TotalBudget { get; set; }
        public double? BudgetAvailable { get; set; }
        public double? BudgetUsed { get; set; }
        public double? LimitBudgetPerCustomer { get; set; }
        [Required]
        public TpBudgetDefineModel BudgetDefineModel  { get; set; }
        [Required]
        public List<TpBudgetAllotmentModel> BudgetAllotmentModels  { get; set; }
    }

    public class BudgetForCheckModel
    {
        public string Code { get; set; }
        public string SaleOrg { get; set; }
        public string BudgetType { get; set; }
        public string BudgetAllocationForm { get; set; }
        public string BudgetAllocationLevel { get; set; }
        public bool FlagOverBudget { get; set; }
        public string Status { get; set; }
        public string PromotionProductType { get; set; }
        public string PromotionProductCode { get; set; }
        public string PackSize { get; set; }
        public decimal BudgetQuantity { get; set; }
        public decimal BudgetQuantityUsed { get; set; }
        public string ItemHierarchyLevel { get; set; }
        public string ItemHierarchyValue { get; set; }
        public decimal TotalAmountAllotment { get; set; }
        public List<TpBudgetAllotment> BudgetAllotmentModels { get; set; }
    }
}
