using Sys.Common.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpPromotionDefinitionStructure : TpAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string PromotionCode { get; set; }
        [Required]
        [MaxLength(10)]
        public string LevelCode { get; set; }
        [Required]
        [MaxLength(255)]
        public string LevelName { get; set; }
        public int QuantityPurchased { get; set; }
        public int OnEachQuantity { get; set; }
        public decimal ValuePurchased { get; set; }
        public decimal OnEachValue { get; set; }
        public string ImageName1 { get; set; }
        public string ImagePath1 { get; set; }
        public string ImageFileExt1 { get; set; }
        public string ImageFolderType1 { get; set; }
        public string ImageName2 { get; set; }
        public string ImagePath2 { get; set; }
        public string ImageFileExt2 { get; set; }
        public string ImageFolderType2 { get; set; }
        [Required]
        [MaxLength(10)]
        public string ProductTypeForSale { get; set; }
        public string ItemHierarchyLevelForSale { get; set; }
        public bool IsGiftProduct { get; set; }
        public bool IsDonate { get; set; }
        public bool IsFixMoney { get; set; }
        public bool RuleOfGiving { get; set; }
        public bool RuleOfGivingByProduct { get; set; } = true;
        public int RuleOfGivingByProductQuantity { get; set; }
        public string RuleOfGivingByProductPacking { get; set; }
        public bool IsGiveSameProductSale { get; set; }
        [MaxLength(10)]
        public string ProductTypeForGift { get; set; }
        public string ItemHierarchyLevelForGift { get; set; }
        public bool IsApplyBudget { get; set; }
        public decimal AmountOfDonation { get; set; }
        public float PercentageOfAmount { get; set; }
        [MaxLength(10)]
        public string BudgetForDonation { get; set; }
        public string? GiftApplyBudgetCode { get; set; }
        public bool? IsDonateApplyBudget { get; set; } = false;
        public bool? IsGiftApplyBudget { get; set; } = false;
        public string? DonateApplyBudgetCode { get; set; }
        public bool? IsDonateAllowance { get; set; } = false;
        /// <summary>
        /// PRINCIPAL/DISTRIBUTOR/SYSTEM
        /// </summary>
        public string OwnerType { get; set; } = null;
        /// <summary>
        /// value = PrincipalCode Nếu Ownertype = PRINCIPAL
        /// value = DistributorCode nếu Ownertype = DISTRIBUTOR
        /// value = null nếu Ownertype = SYSTEM
        /// </summary>
        public string OwnerCode { get; set; } = null;
    }
}
