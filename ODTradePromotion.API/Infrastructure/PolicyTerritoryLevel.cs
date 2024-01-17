using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class PolicyTerritoryLevel
    {
        public Guid Id { get; set; }
        public string PolicyCode { get; set; }
        public string TerritoryStructureCode { get; set; }
        public string TerritoryStructureDescription { get; set; }
        public string TerritoryLevel { get; set; }
        public string TerritoryLevelSource { get; set; }
        public string TerritoryValue { get; set; }
        public string TerritoryValueName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
