using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Models.Discount
{
    public class TpDiscountStructureModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string DiscountCode { get; set; }
        [Required]
        public string SicCode { get; set; }
        public string Reason { get; set; }
        [Required]
        public int DeleteFlag { get; set; }
        public List<TpDiscountStructureDetailModel> tpDiscountStructureDetailModels { set; get; }
    }
}
