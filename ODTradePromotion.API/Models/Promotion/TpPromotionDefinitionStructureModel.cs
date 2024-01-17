using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Models.Promotion
{
    public class TpPromotionDefinitionStructureModel
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string PromotionCode { get; set; }
        public bool PromotionCheckBy { get; set; } = true;
        [Required]
        [MaxLength(10)]
        public string LevelCode { get; set; }
        [Required]
        [MaxLength(100)]
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
        public string ProductTypeForSale { get; set; }
        public string ItemHierarchyLevelForSale { get; set; }
        public bool IsGiftProduct { get; set; }
        public bool IsDonate { get; set; }
        public bool IsDonateAllowance { get; set; } 
        public bool IsDonateApplyBudget { get; set; }
        public bool IsFixMoney { get; set; }
        public bool RuleOfGiving { get; set; } = true;
        public bool RuleOfGivingByProduct { get; set; } = true;
        public int RuleOfGivingByProductQuantity { get; set; }
        public string RuleOfGivingByProductPacking { get; set; }
        public bool IsGiveSameProductSale { get; set; } = false;
        public string ProductTypeForGift { get; set; }
        public string ItemHierarchyLevelForGift { get; set; }
        public bool IsApplyBudget { get; set; }
        //1S CR
        public string BudgetCodeForGift { get; set; }
        public string BudgetTypeOfGift { get; set; }
        public string BudgetAllocationLevelOfGift { get; set; }
        public string BudgetCodeForDonate { get; set; }
        public string BudgetTypeOfDonate { get; set; }
        public string BudgetAllocationLevelOfDonate { get; set; }
        public bool Allowance { get; set; }
        //1S CR
        public decimal AmountOfDonation { get; set; }
        public float PercentageOfAmount { get; set; }
        public string BudgetForDonation { get; set; }
        public bool IsGiftApplyBudget { get; set; }
        public string? GiftApplyBudgetCode { get; set; }
        public string? DonateApplyBudgetCode { get; set; }
        public List<TpPromotionDefinitionProductForSaleModel> ListProductForSales { get; set; } = new List<TpPromotionDefinitionProductForSaleModel>();
        public List<TpPromotionDefinitionProductForGiftModel> ListProductForGifts { get; set; } = new List<TpPromotionDefinitionProductForGiftModel>();
    }

    public class PromotionDefinitionForSettlementModel
    {
        public string PromotionCode { get; set; }
        public string LevelCode { get; set; }
        public string LevelName { get; set; }
        public bool IsGiftProduct { get; set; }
        public bool IsDonate { get; set; }
        public bool IsFixMoney { get; set; }
    }
}
