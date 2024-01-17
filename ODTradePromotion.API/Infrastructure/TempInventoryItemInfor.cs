using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class TempInventoryItemInfor
    {
        public Guid Id { get; set; }
        public string InventoryItemCode { get; set; }
        public int? OnHand { get; set; }
        public int? Avaiable { get; set; }
    }
}
