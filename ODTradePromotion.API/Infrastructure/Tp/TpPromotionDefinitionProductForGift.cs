using Sys.Common.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpPromotionDefinitionProductForGift : TpAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string PromotionCode { get; set; }
        [Required]
        [MaxLength(10)]
        public string LevelCode { get; set; }
        public string ProductTypeForGift { get; set; }
        public string ItemHierarchyLevelForGift { get; set; }
        [Required]
        [MaxLength(10)]
        public string ProductCode { get; set; }
        [Required]
        [MaxLength(10)]
        public string Packing { get; set; }
        [Required]
        public int NumberOfGift { get; set; }
        public string BudgetCode { get; set; }
        public bool IsDefaultProduct { get; set; }
        public int Exchange { get; set; }
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
