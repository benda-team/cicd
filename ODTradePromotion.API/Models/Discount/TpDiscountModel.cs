using ODTradePromotion.API.Models.Customer;
using ODTradePromotion.API.Models.External;
using ODTradePromotion.API.Models.SalesOrg;
using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Models.Discount
{
    public class TpDiscountModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(100)]
        public string ShortName { get; set; }
        [MaxLength(255)]
        public string Scheme { get; set; }
        [Required]
        public DateTime EffectiveDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        [Required]
        [MaxLength(10)]
        public string SaleOrg { get; set; }
        [Required]
        [MaxLength(10)]
        public string SicCode { get; set; }
        [Required]
        [MaxLength(10)]
        public string DiscountFrequency { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public string FolderType { get; set; }
        public string UserName { get; set; }
        public string Reason { get; set; }
        public int DeleteFlag { get; set; }
        public TpDiscountListModel TpDiscountListModel { get; set; }
        [Required]
        [MaxLength(10)]
        public string Status { get; set; }
        public string StatusName { get; set; }
        [Required]
        [MaxLength(10)]
        public string ScopeType { get; set; }
        public string ScopeSaleTerritoryLevel { get; set; }
        [Required]
        [MaxLength(10)]
        public string ObjectType { get; set; }

        [Required]
        public int DiscountType { get; set; } = 1;

        public string ReasonStep1 { get; set; }
        public string ReasonStep2 { get; set; }
        public string ReasonStep3 { get; set; }
        public string ReasonStep4 { get; set; }

        // New
        [MaxLength(10)]
        public string DisSicCode { get; set; }
        [MaxLength(100)]
        public string OwnerType { get; set; }
        [MaxLength(255)]
        public string OwnerCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public TpScopeDiscountModel TpScopeDiscountModel { get; set; }
        public TpObjectDiscountModel TpObjectDiscountModel { get; set; }
        public TpDiscountStructureModel TpDiscountStructureModel { get; set; }

        public List<TpSalesTerritoryValueModel> ListScopeSalesTerritory { get; set; } = new List<TpSalesTerritoryValueModel>();
        public List<TpSalesOrgDsaModel> ListScopeDSA { get; set; } = new List<TpSalesOrgDsaModel>();
        public List<CustomerSettingModel> ListCustomerSetting { get; set; } = new List<CustomerSettingModel>();
        public List<CustomerAttributeModel> ListCustomerAttribute { get; set; } = new List<CustomerAttributeModel>();
        public List<CustomerShiptoModel> ListCustomerShipto { get; set; } = new List<CustomerShiptoModel>();
        public List<TpDiscountStructureDetailModel> ListDiscountStructureDetails { set; get; } = new();
    }
    public class TpDiscountListModel
    {
        public List<TpDiscountModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }

    public class DiscountScopeTerritoryMode
    {
        public string DiscountCode { get; set; }
        public string SaleOrg { get; set; }
        public string ScopeSaleTerritoryLevel { get; set; }
        public string SalesTerritoryValue { get; set; }
    }

    #region External API
    public class ExtGetListDiscountRequestModel
    {
        public string CustomerCode { get; set; }
        public string ShiptoCode { get; set; }
        public string RouteZoneCode { get; set; }
    }

    public class ExternalGetListDiscountRequestModel
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

    public class DiscountExternalModel
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
        public string SicCode { get; set; }
        public string ApplicableObjectType { get; set; }
        public int DiscountType { get; set; }

        public List<TpDiscountStructureDetailModel> ListDiscountStructureDetails { set; get; } = new();
    }

    public class CustomerAttributeMasterClass
    {
        public string Level { get; set; }
        public string Code { get; set; }
    }

    public class CustomerShiptoMasterClass
    {
        public string CustomerCode { get; set; }
        public string ShiptoCode { get; set; }
    }

    public class ScopeTerritoryMasterClass
    {
        public string TerritoryLevel { get; set; }
        public string TerritoryValue { get; set; }
    }
    #endregion
}
