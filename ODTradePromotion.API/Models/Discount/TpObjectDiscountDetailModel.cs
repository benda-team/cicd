using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Discount
{
    public class TpObjectDiscountDetailModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string DiscountCode { get; set; }
        [Required]
        public string ObjectType { get; set; }
        public string ObjectSalesAttributeDiscountCode { set; get; }
        public string ObjectSalesAttributeValue { set; get; }
        public string CustomerShipToCode { get; set; }
        public int DeleteFlag { get; set; }
    }
}
