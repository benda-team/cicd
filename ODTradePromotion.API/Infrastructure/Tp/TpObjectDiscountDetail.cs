using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpObjectDiscountDetail : TpAuditableEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string DiscountCode { get; set; }
        [Required]
        [MaxLength(10)]
        public string ObjectType { get; set; }
        [MaxLength(10)]
        public string ObjectSalesAttributeDiscountCode { set; get; }
        public string ObjectSalesAttributeValue { set; get; }
        [MaxLength(10)]
        public string CustomerShipToCode { get; set; }
    }
}
