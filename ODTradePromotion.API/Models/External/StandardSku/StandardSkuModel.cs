using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.External.StandardSku
{
    public class StandardSkuModel
    {
        public string ItemGroupCode { get; set; }
        public int RuleType { get; set; }
        public string InventoryCode { get; set; }
        public int? Priority { get; set; }
        public int? Ratio { get; set; }
    }
}
