using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class DataProtectionKey
    {
        public int Id { get; set; }
        public string FriendlyName { get; set; }
        public string Xml { get; set; }
    }
}
