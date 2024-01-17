using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Discount
{
    public class TpScopeDiscountDetailModel
    {
        public Guid Id { get; set; }
        public string DiscountCode { get; set; }
        public string ScopeType { get; set; }
        public string SalesTerritoryLevelCode { set; get; }
        public string SalesTerritoryValueCode { set; get; }
        public int DeleteFlag { get; set; }
    }
}
