using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpDiscountScopeDsa
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string DiscountCode { get; set; }
        [Required]
        [MaxLength(10)]
        public string SaleOrg { get; set; }
        [Required]
        [MaxLength(10)]
        public string ScopeDsaValue { get; set; }
        [MaxLength(100)]
        public string OwnerType { get; set; }
        [MaxLength(255)]
        public string OwnerCode { get; set; }
    }
}
