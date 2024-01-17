using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class KpisSiref
    {
        public Guid Id { get; set; }
        public string KpisCode { get; set; }
        public string SirefCode { get; set; }
    }
}
