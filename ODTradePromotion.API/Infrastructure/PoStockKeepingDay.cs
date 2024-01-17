using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class PoStockKeepingDay
    {
        public Guid Id { get; set; }
        public string StockKeepingDayNumber { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string SalesTerritoryLevel { get; set; }
        public string TerritoryLevelValue { get; set; }
        public string ItemHierachyLevel { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string SaleOrgId { get; set; }
    }
}
