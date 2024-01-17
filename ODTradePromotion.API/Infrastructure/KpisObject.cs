using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class KpisObject
    {
        public Guid Id { get; set; }
        public string KpisObjectCode { get; set; }
        public string KpisCode { get; set; }
        public string Frequency { get; set; }
        public bool RepeatTarget { get; set; }
    }
}
