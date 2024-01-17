using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Temp
{
    public class TempTpOrderDetailModel
    {
        public Guid Id { get; set; }
        public string OrdNbr { get; set; }
        public string InventoryID { get; set; }
        public string InventoryName { get; set; }
        public string DiscountID { get; set; }
        public string DiscountName { get; set; }
        public string DiscountType { get; set; }
        public string DiscountSchemeID { get; set; }
        public bool IsFree { get; set; }
        public string UOM { get; set; }
        public string UOMName { get; set; }
        public decimal ShippedQty { get; set; }
        public decimal ShippedLineDiscAmt { get; set; }
        public decimal UnitPrice { get; set; }
        public string PromotionLevel { get; set; }
        public string PromotionLevelName { get; set; }
    }
}
