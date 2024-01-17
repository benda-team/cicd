using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpSettlement : TpAuditableEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        public string ProgramType { get; set; }
        public bool SchemeType { get; set; } = false;
        [Required]
        [MaxLength(10)]
        public string Status { get; set; }
        public DateTime SettlementDate { get; set; }
        public string PromotionDiscountCode { get; set; }
        public string PromotionDiscountScheme { get; set; }
        [Required]
        public int FrequencySettlement { get; set; }
        public string FrequencyCode { get; set; }
        public string SaleCalendarCode { get; set; }
        public decimal TotalDistributor { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
