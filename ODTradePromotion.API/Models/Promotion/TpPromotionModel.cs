using ODTradePromotion.API.Models.Budget;
using ODTradePromotion.API.Models.Customer;
using ODTradePromotion.API.Models.Discount;
using ODTradePromotion.API.Models.External;
using ODTradePromotion.API.Models.External.StandardSku;
using ODTradePromotion.API.Models.Item;
using ODTradePromotion.API.Models.SalesOrg;
using Sys.Common.Constants;
using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Models.Promotion
{
    public class TpPromotionModel
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string PromotionType { get; set; }
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }
        [Required]
        [MaxLength(100)]
        public string ShortName { get; set; }
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(10)]
        public string Status { get; set; }
        [Required]
        [MaxLength(255)]
        public string Scheme { get; set; }
        [Required]
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public string SaleOrg { get; set; }
        public string TerritoryStructureCode { get; set; }
        public string SicCode { get; set; }
        public int SettlementFrequency { get; set; }
        public string FrequencyPromotion { get; set; }
        public string ImageName1 { get; set; }
        public string ImagePath1 { get; set; }
        public string ImageFileExt1 { get; set; }
        public string ImageFolderType1 { get; set; }
        public string ImageName2 { get; set; }
        public string ImagePath2 { get; set; }
        public string ImageFileExt2 { get; set; }
        public string ImageFolderType2 { get; set; }
        public string ImageName3 { get; set; }
        public string ImagePath3 { get; set; }
        public string ImageFileExt3 { get; set; }
        public string ImageFolderType3 { get; set; }
        public string ImageName4 { get; set; }
        public string ImagePath4 { get; set; }
        public string ImageFileExt4 { get; set; }
        public string ImageFolderType4 { get; set; }
        public string ScopeType { get; set; }
        public string ScopeSaleTerritoryLevel { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileExt { get; set; }
        public string FolderType { get; set; }
        public string ApplicableObjectType { get; set; }
        public bool PromotionCheckBy { get; set; } = true;
        public bool RuleOfGiving { get; set; } = true;
        public bool RuleOfGivingByValue { get; set; } = true;
        public bool IsApplyBudget { get; set; }
        public string UserName { get; set; }
        public string ReasonStep1 { get; set; }
        public string ReasonStep2 { get; set; }
        public string ReasonStep3 { get; set; }
        public string ReasonStep4 { get; set; }
        public string ReasonStep5 { get; set; }
        public bool? IsDonateApplyBudget { get; set; }
        public string DisSicCode { get; set; }
        public string DisSicDesc { get; set; }
        public bool IsFlashSales { get; set; }
        public DateTime? FsValidHour { get; set; }
        public DateTime? FsUntilHour { get; set; }
        public string OwnerType { get; set; } = null;
        public string OwnerCode { get; set; } = null;
        public bool IsCheck { get; set; }
        /// <summary>
        /// Nếu tạo ở CP system: PrincipalCode
        /// Nếu tạo ở OD system: value đã chọn ở màn hình tạo
        /// </summary>
        public string PrincipalCode { get; set; }
        public List<TpSalesTerritoryValueModel> ListScopeSalesTerritory { get; set; } = new List<TpSalesTerritoryValueModel>();
        public List<TpSalesOrgDsaModel> ListScopeDSA { get; set; } = new List<TpSalesOrgDsaModel>();
        public List<CustomerSettingModel> ListCustomerSetting { get; set; } = new List<CustomerSettingModel>();
        public List<CustomerAttributeModel> ListCustomerAttribute { get; set; } = new List<CustomerAttributeModel>();
        public List<CustomerShiptoModel> ListCustomerShipto { get; set; } = new List<CustomerShiptoModel>();
        public List<TpPromotionDefinitionStructureModel> ListDefinitionStructure { get; set; } = new List<TpPromotionDefinitionStructureModel>();
    }

    public class TpPromotionListModel
    {
        public List<TpPromotionModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }

    public class PromotionPopupModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string ScopeType { get; set; }
        public string ScopeTypeDescription { get; set; }
        public string ApplicableObjectType { get; set; }
        public string ApplicableObjectTypeDescription { get; set; }
        public bool IsApplyBudget { get; set; }
        public string UserName { get; set; }
        public string Scheme { get; set; }
    }

    public class ListPromotionPopupModel
    {
        public List<PromotionPopupModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }

    public class ConfirmPromotionReq
    {
        public string Code { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
    }

    #region For External API
    public class ExtGetListPromotionRequestModel
    {
        public string CustomerCode { get; set; }
        public string ShiptoCode { get; set; }
        public string RouteZoneCode { get; set; }
        public string SicCode { get; set; }
    }

    public class ExternalGetListPromotionRequestModel
    {
        public string SaleOrgCode { get; set; }
        public string SicCode { get; set; }
        public string CustomerCode { get; set; }
        public string ShiptoCode { get; set; }
        public string RouteZoneCode { get; set; }
        public string DsaCode { get; set; }
        public string Branch { get; set; }
        public string Region { get; set; }
        public string SubRegion { get; set; }
        public string Area { get; set; }
        public string SubArea { get; set; }
    }

    public class PromotionExternalModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Scheme { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string SaleOrg { get; set; }
        public string ScopeType { get; set; }
        public string ApplicableObjectType { get; set; }
        public string SicCode { get; set; }
    }

    public class CustomerScopeModel
    {
        public string SaleOrgCode { get; set; }
        public string TerritoryStructureCode { get; set; }
        public string CustomerCode { get; set; }
        public string ShiptoCode { get; set; }
        public string Country { get; set; }
        public string Branch { get; set; }
        public string Region { get; set; }
        public string SubRegion { get; set; }
        public string Area { get; set; }
        public string SubArea { get; set; }
        public string DSACode { get; set; }
    }

    public class CustomerObjectApplicable
    {
        public string CustomerCode { get; set; }
        public string ShiptoCode { get; set; }
        public string CustomerAttributeLevel { get; set; }
        public string CustomerAttributeValue { get; set; }
    }

    public class PromotionResultModel
    {
        public Guid Id { get; set; }
        public string PromotionCode { get; set; }
        public bool PromotionCheckBy { get; set; } = true;
        public string LevelCode { get; set; }
        public string LevelName { get; set; }
        public decimal QuantityPurchased { get; set; }
        public decimal ValuePurchased { get; set; }
        public decimal OnEach { get; set; }
        public bool IsGiftProduct { get; set; }
        public bool IsDonate { get; set; }
        public bool IsFixMoney { get; set; }
        public bool RuleOfGiving { get; set; }
        public bool RuleOfGivingByValue { get; set; }
        public bool IsApplyBudget { get; set; }
        public decimal AmountOfDonation { get; set; }
        public float PercentageOfAmount { get; set; }
        public string BudgetForDonation { get; set; }

        public List<TpPromotionDefinitionProductForSaleModel> ListProductForSales { get; set; } = new List<TpPromotionDefinitionProductForSaleModel>();
        public List<TpPromotionDefinitionProductForGiftModel> ListProductForGifts { get; set; } = new List<TpPromotionDefinitionProductForGiftModel>();
    }

    public class PromotionResultRequestModel
    {
        public string PromotionCode { get; set; }
        public string LevelCode { get; set; }
        public int NumberOfPurchases { get; set; }

        public string CustomerCode { get; set; }
        public string ShiptoCode { get; set; }
        public string RouteZoneCode { get; set; }
        public string DistributorCode { get; set; }
    }

    public class PromotionResultResponseModel
    {
        public PromotionProductForSaleModel ProductForSale { get; set; }
        public PromotionProductForGift ProductForGift { get; set; }
    }

    public class PromotionProductForSaleModel
    {
        public List<StandardSkuWithQuantity> productDetails { get; set; } = new List<StandardSkuWithQuantity>();
    }

    public class PromotionProductForGift
    {
        public int TypeOfReward { get; set; }
        public decimal AmountOfDonation { get; set; }
        public bool IsEnoughBudgetAmount { get; set; } = true;
        public List<ProductForGift> productForGiftDetails { get; set; }
    }

    public class ProductForGift
    {
        public bool IsDefault { get; set; }
        public int Exchange { get; set; }
        public List<StandardSkuWithQuantity> productByCodeDetails { get; set; } = new List<StandardSkuWithQuantity>();
    }

    public class PromotionProductForGiftRequest
    {
        public string PromotionCode { get; set; }
        public string LevelCode { get; set; }
        public int NumberOfPurchases { get; set; }
        public decimal TotalMoneyPurchases { get; set; }
    }

    public class InventoryUomModel
    {
        public Guid InventoryId { get; set; }
        public string InvenrotyCode { get; set; }
        public string ItemGroupCode { get; set; }
        public Guid UomFromId { get; set; }
        public string UomFromCode { get; set; }
        public string UomFromDes { get; set; }
        public Guid UomToId { get; set; }
        public string UomToCode { get; set; }
        public string UomToDes { get; set; }
        public int ConversionFactor { get; set; }
    }

    public class ItemGroupModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class TempProductModel
    {
        public string Code { get; set; }
        public string Uom { get; set; }
    }

    public class PromotionBudgetRequest
    { 
        public string PromotionCode { get; set; }
        public string PromotionLevel { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerShiptoCode { get; set; }
        public string RouteZoneCode { get; set; }
        public string ProductSkuCode { get; set; }
        public int ProductNumber { get; set; }
        public decimal AmountOfDonation { get; set; }
        public string SaleOrg { get; set; }
        public string SicCode { get; set; }
        public string DsaCode { get; set; }
        public string CountryCode { get; set; }
        public string BranchCode { get; set; }
        public string RegionCode { get; set; }
        public string SubRegionCode { get; set; }
        public string AreaCode { get; set; }
        public string SubAreaCode { get; set; }
        public string Key {
            get { return $"{PromotionCode}-{PromotionLevel}-{CustomerCode}-{CustomerShiptoCode}-{RouteZoneCode}"; }
        }
    }

    public class PromotionBudgetResponse
    {
        public bool IsHaveBudget { get; set; }
        public bool? IsEnoughBudgetForProduct { get; set; }
        public bool? FlagOverBudgetForProduct { get; set; }
        public decimal SumProductQuantityUsed { get; set; }
        public decimal SumProductQuantityUsedByCustomer { get; set; }
        public bool OverProductQuanty { get; set; }
        public bool OverProductQuantyByCustomer { get; set; }
        public bool? IsEnoughBudgetForAmount { get; set; }
        public bool? FlagOverBudgetForAmount { get; set; }
        public decimal SumAmountUsed { get; set; }
        public decimal SumAmountUsedByCustomer { get; set; }
        public bool OverAmount { get; set; }
        public bool OverAmountByCustomer { get; set; }
        public BudgetForCheckModel BudgetForProductInfo { get; set; }
        public BudgetForCheckModel BudgetForAmountInfo { get; set; }
    }

    public class ProductItemHierarchyValue
    {
        public string ItemHierarchyLevel { get; set; }
        public string ItemHierarchyValue { get; set; }
    }

    public class ListPromotionAndDiscountRequestModel
    {
        public string SaleOrgCode { get; set; }
        public string SicCode { get; set; }
        public string CustomerCode { get; set; }
        public string ShiptoCode { get; set; }
        public string RouteZoneCode { get; set; }
        public string DsaCode { get; set; }
        public string Branch { get; set; }
        public string Region { get; set; }
        public string SubRegion { get; set; }
        public string Area { get; set; }
        public string SubArea { get; set; }
    }

    public class ListPromotionAndDiscountResponseModel
    {
        public List<PromotionExternalModel> ListPromotionGeneralInfos { get; set; }
        public DiscountExternalModel DiscountGeneralInfo { get; set; }
    }

    #endregion
}
