using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Discount
{
    public class TpScopeDiscountModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string DiscountCode { get; set; }
        [Required]
        public string ScopeType { get; set; }
        public string SalesTerritoryLevelCode { set; get; }
        public string Reason { get; set; }
        public int DeleteFlag { get; set; }
        public List<TpScopeDiscountDetailModel> tpScopeDiscountDetailModels { set; get; }
    }
}
