using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.External.StandardSku
{
    public class ItemInventoryStockModel
    {
        public string ItemGroupCode { get; set; }
        public string InventoryItemCode { get; set; }
        public string InventoryItemDescription { get; set; }
        public string BaseUom { get; set; }
        public int? OnHand { get; set; }
        public int? Avaiable { get; set; }
        public string BudgetCode { get; set; }
    }
}
