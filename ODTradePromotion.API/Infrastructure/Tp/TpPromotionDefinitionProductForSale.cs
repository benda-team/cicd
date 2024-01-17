using Sys.Common.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpPromotionDefinitionProductForSale : TpAuditableEntity
    {

        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string PromotionCode { get; set; }
        [Required]
        [MaxLength(10)]
        public string LevelCode { get; set; }
        public string ProductTypeForSale { get; set; }
        public string ItemHierarchyLevelForSale { get; set; }
        [Required]
        [MaxLength(10)]
        public string ProductCode { get; set; }
        //[Required]
        [MaxLength(10)]
        public string Packing { get; set; }
        [Required]
        public int SellNumber { get; set; }
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
