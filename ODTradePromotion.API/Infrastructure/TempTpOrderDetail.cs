using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class TempTpOrderDetail
    {
        public Guid Id { get; set; }
        public string OrdNbr { get; set; }
        public string InventoryId { get; set; }
        public string DiscountId { get; set; }
        public string DiscountType { get; set; }
        public string DiscountSchemeId { get; set; }
        public bool IsFree { get; set; }
        public string Uom { get; set; }
        public decimal ShippedQty { get; set; }
        public decimal ShippedLineDiscAmt { get; set; }
        public decimal UnitPrice { get; set; }
        public string PromotionLevel { get; set; }
    }
}
