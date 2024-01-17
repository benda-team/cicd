using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.External.StandardSku
{
    public class StandardSkuWithQuantity
    {
        public string ItemCode { get; set; }
        //public string BaseUom { get; set; }
        public string UomCode { get; set; }
        public bool IsEnoughStock { get; set; }
        public bool IsEnoughBudget { get; set; } = true;
        public int Available { get; set; }
    }
}
