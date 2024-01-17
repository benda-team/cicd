using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class PoPurchaseSchedule
    {
        public Guid Id { get; set; }
        public string TerritoryValueKey { get; set; }
        public string SaleOrgCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
