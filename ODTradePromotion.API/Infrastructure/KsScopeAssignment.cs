using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class KsScopeAssignment
    {
        public Guid Id { get; set; }
        public string CampainCode { get; set; }
        public string GeoCode { get; set; }
        public string GeoValue { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryValue { get; set; }
        public string SyncCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
