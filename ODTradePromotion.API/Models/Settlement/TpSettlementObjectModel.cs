using ODTradePromotion.API.Models.Discount;
using ODTradePromotion.API.Models.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Settlement
{
    public class TpSettlementObjectModel
    {
        public Guid Id { get; set; }
        public string SettlementCode { get; set; }
        public string SettlementName { get; set; }
        public string ProgramType { get; set; }
        public string PromotionDiscountCode { get; set; }
        public string PromotionDiscountName { get; set; }
        public TpPromotionGeneralModel PromotionGeneralModel { get; set; }
        public TpDiscountModel DiscountModel { get; set; }
    }
}
