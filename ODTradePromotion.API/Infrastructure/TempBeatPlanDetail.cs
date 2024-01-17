using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class TempBeatPlanDetail
    {
        public Guid Id { get; set; }
        public Guid BeatPlanId { get; set; }
        public Guid CustomerShiptoId { get; set; }
    }
}
