using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Settlement
{
    public class TpSettlementObjectWithCalendarModel
    {
        public string SettlementCode { get; set; }
        public string ProgramType { get; set; }
        public string PromotionDiscountCode { get; set; }
        public string SaleCalendarCode { get; set; }
    }
}
