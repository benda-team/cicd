using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class AutoKpisTarget
    {
        public Guid Id { get; set; }
        public string AutoKpisTargetCode { get; set; }
        public string SostructureCode { get; set; }
        public string SalesTerritoryStructure { get; set; }
        public int Year { get; set; }
        public string Status { get; set; }
    }
}
