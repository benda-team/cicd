using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Customer
{
    public class CustomerSettingModel
    {
        public Guid Id { get; set; }
        public string AttributeID { get; set; }
        public string AttributeName { get; set; }
        public string Description { get; set; }
        public bool IsDistributorAttribute { get; set; } = false;
        public bool IsCustomerAttribute { get; set; } = false;
        public bool Used { get; set; } = false;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsEditable { get; set; } = false;
        public bool IsDistributorAttributeEditable { get; set; }
        public bool IsCustomerAttributeEditable { get; set; }
        public bool IsUsedEditable { get; set; }
        public bool IsChecked { get; set; }
        public Guid CustomerSettingId { get; set; }
        [MaxLength(100)]
        public string OwnerType { get; set; }
        [MaxLength(255)]
        public string OwnerCode { get; set; }
    }

    public class CustomerSettingListModel
    {
        public List<CustomerSettingModel> Items { get; set; } = new List<CustomerSettingModel>();
    }

    public class CustomerSettingMasterDataModel
    {
        public string AttributeID { get; set; }
        public string Description { get; set; }
    }

    public class CustomerSettingUpdateRequest
    {
        public string AttributeID { get; set; }
        public string Description { get; set; }
        public bool IsDistributorAttribute { get; set; }
        public bool IsCustomerAttribute { get; set; }
        public bool Used { get; set; } = false;
    }
}
