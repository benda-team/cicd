using System;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpDiscountStructureDetail : TpAuditableEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [MaxLength(10)]
        public string DiscountCode { get; set; }
        public int DiscountType { get; set; }
        [Required]
        [MaxLength(100)]
        public string NameDiscountLevel { get; set; }
        public decimal DiscountCheckValue { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public string FileExt { get; set; }
        public string FolderType { get; set; }
        [MaxLength(100)]
        public string OwnerType { get; set; }
        [MaxLength(255)]
        public string OwnerCode { get; set; }
    }
}
