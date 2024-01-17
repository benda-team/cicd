using ODTradePromotion.API.Models.External;
using ODTradePromotion.API.Models.Paging;
using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Models.Customer
{
    public class CustomerAttributeModel
    {
        public Guid Id { get; set; }

        public string AttributeMaster { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public string Parent { get; set; }
        public Guid? ParentCustomerAttributeId { get; set; }
        public string ShortName { get; set; }

        public bool IsDistributorAttribute { get; set; } = false;
        public bool IsCustomerAttribute { get; set; } = false;
        public DateTime EffectiveDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public bool? IsChecked { get; set; } = false;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsCheck { get; set; }
        public CustomerSettingModel CustomerSetting { get; set; }
        [MaxLength(100)]
        public string OwnerType { get; set; }
        [MaxLength(255)]
        public string OwnerCode { get; set; }
    }

    public class CustomerAttributeListModel
    {
        public List<CustomerAttributeModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }

    public class CustomerAttributeCreateRequest
    {
        public string Code { get; set; }
        public string AttributeMaster { get; set; }
        public Guid? ParentCustomerAttributeId { get; set; }
        public string Description { get; set; }
        public string ShortName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public bool IsDistributorAttribute { get; set; }
        public bool IsCustomerAttribute { get; set; }
    }

    public class CustomerAttributeImportRequest
    {
        public string AttributeValueCode { get; set; }
        public string ParentAttributeValueCode { get; set; }
        public string ParentAttributeMaster { get; set; }
        public string AttributeMaster { get; set; }
        public string Description { get; set; }
        public string ShortName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public bool IsDistributorAttribute { get; set; }
        public bool IsCustomerAttribute { get; set; }
    }

    public class CustomerAttributeUpdateRequest : CustomerAttributeCreateRequest
    {
        public Guid Id { get; set; }
    }

    public class QueryCustomerAttributeRequest : EcoParameters
    {
        public string AttributeMaster { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDistributorAttribute { get; set; }
        public bool IsCustomerAttribute { get; set; }
        public string ChildAttributeMaster { get; set; }
    }

    public class CustomerAttributeExportParamModel
    {
        public List<Guid> Ids { get; set; }
        public bool IsSelectAll { get; set; } = false;
        public QueryCustomerAttributeRequest SearchModel { get; set; }
    }

    public class CustomerAttributeExistedModel
    {
        public string AttributeMaster { get; set; }
        public string AttributeValueCode { get; set; }
    }

    public class CustomerAttributeMasterDataModel
    {
        public string Description { get; set; }
    }
}
