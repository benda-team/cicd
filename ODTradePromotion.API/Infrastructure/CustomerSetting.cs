﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class CustomerSetting
    {
        public CustomerSetting()
        {
            CustomerAttributes = new HashSet<CustomerAttribute>();
            CustomerSettingHierarchies = new HashSet<CustomerSettingHierarchy>();
        }

        public Guid Id { get; set; }
        public string AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string Description { get; set; }
        public bool IsDistributorAttribute { get; set; }
        public bool IsCustomerAttribute { get; set; }
        public bool Used { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<CustomerAttribute> CustomerAttributes { get; set; }
        public virtual ICollection<CustomerSettingHierarchy> CustomerSettingHierarchies { get; set; }
    }
}
