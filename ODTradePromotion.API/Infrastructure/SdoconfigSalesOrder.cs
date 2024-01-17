using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class SdoconfigSalesOrder
    {
        public Guid Id { get; set; }
        public string Sdocode { get; set; }
        public string SalesUoMcode { get; set; }
        public int? Value { get; set; }
    }
}
