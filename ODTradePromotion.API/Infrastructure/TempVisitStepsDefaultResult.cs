using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class TempVisitStepsDefaultResult
    {
        public Guid Id { get; set; }
        public string VisitStepsDefaultResultCode { get; set; }
        public string VisitStepsCode { get; set; }
        public string VisitStepsDefaultResultDescription { get; set; }
    }
}
