using System;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpSettlementDetail : TpAuditableEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string SettlementCode { get; set; }
        [Required]
        public string ProgramType { get; set; }
        [MaxLength(10)]
        public string OrdNbr { get; set; }
        [MaxLength(10)]
        public string Status { get; set; }
        [Required]
        [MaxLength(10)]
        public string PromotionDiscountCode { get; set; }       
        [Required]
        [MaxLength(10)]
        public string DistributorCode { get; set; }
        public string ProductCode { get; set; }
        public decimal? Quantity { get; set; }
        public string Package { get; set; }
        public decimal? Amount { get; set; }

        public DateTime OrdDate { get; set; }
        public string PromotionDiscountName { get; set; }
        public string PromotionLevel { get; set; }
        public string PromotionLevelName { get; set; }
        public string CustomerID { get; set; }
        public string ShiptoID { get; set; }
        public string ShiptoName { get; set; }
        public string ReferenceLink { get; set; }
        public string SalesRepCode { get; set; }
    }
}
