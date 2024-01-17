using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Promotion
{
    public class TpPromotionScopeTerritoryModel
    {
        public Guid Id { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionName { get; set; }
        public string SalesTerritoryValue { get; set; }
        public string SalesTerritoryValueName { get; set; }
    }
}
