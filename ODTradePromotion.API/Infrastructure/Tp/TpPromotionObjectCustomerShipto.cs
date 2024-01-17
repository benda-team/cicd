using Sys.Common.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpPromotionObjectCustomerShipto : TpAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string PromotionCode { get; set; }
        [Required]
        [MaxLength(10)]
        public string CustomerCode { get; set; }
        [Required]
        [MaxLength(10)]
        public string CustomerShiptoCode { get; set; }
        /// <summary>
        /// PRINCIPAL/DISTRIBUTOR/SYSTEM
        /// </summary>
        public string OwnerType { get; set; } = null;
        /// <summary>
        /// value = PrincipalCode Nếu Ownertype = PRINCIPAL
        /// value = DistributorCode nếu Ownertype = DISTRIBUTOR
        /// value = null nếu Ownertype = SYSTEM
        /// </summary>
        public string OwnerCode { get; set; } = null;
    }
}
