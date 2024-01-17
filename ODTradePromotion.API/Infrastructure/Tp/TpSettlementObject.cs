using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpSettlementObject : TpAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string SettlementCode { get; set; }
        [Required]
        public string ProgramType { get; set; }
        [Required]
        [MaxLength(10)]
        public string PromotionDiscountCode { get; set; }
    }
}
