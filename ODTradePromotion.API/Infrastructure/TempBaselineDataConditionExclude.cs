using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class TempBaselineDataConditionExclude
    {
        public Guid Id { get; set; }
        public string OrderCode { get; set; }
        public string ConditionExcludeCode { get; set; }
        public string ResonCode { get; set; }
        public string ReasonDescription { get; set; }
    }
}
