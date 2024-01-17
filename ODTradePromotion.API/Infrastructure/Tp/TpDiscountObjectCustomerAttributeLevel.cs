using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpDiscountObjectCustomerAttributeLevel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string DiscountCode { get; set; }
        [Required]
        [MaxLength(10)]
        public string CustomerAttributerLevel { get; set; }
        [DefaultValue(false)]
        public bool IsApply { get; set; }
        [MaxLength(100)]
        public string OwnerType { get; set; }
        [MaxLength(255)]
        public string OwnerCode { get; set; }
    }
}
