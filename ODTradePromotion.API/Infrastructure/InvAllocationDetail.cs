using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class InvAllocationDetail
    {
        public Guid Id { get; set; }
        public string ItemKey { get; set; }
        public Guid ItemId { get; set; }
        public string ItemCode { get; set; }
        public string BaseUom { get; set; }
        public string ItemDescription { get; set; }
        public string WareHouseCode { get; set; }
        public string LocationCode { get; set; }
        public string DistributorCode { get; set; }
        public int OnHand { get; set; }
        public int OnSoShipping { get; set; }
        public int OnSoBooked { get; set; }
        public int Available { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public string Attribute4 { get; set; }
        public string Attribute5 { get; set; }
        public string Attribute6 { get; set; }
        public string Attribute7 { get; set; }
        public string Attribute8 { get; set; }
        public string Attribute9 { get; set; }
        public string Attribute10 { get; set; }
        public string ItemGroupCode { get; set; }
        public string DSACode { get; set; }
        public string ShortName { get; set; }
        public Guid? Hierarchy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool LSNumber { get; set; }
        public string ReportName { get; set; }
    }
}
