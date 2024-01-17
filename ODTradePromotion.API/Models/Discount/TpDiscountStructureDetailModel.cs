using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Discount
{
    public class TpDiscountStructureDetailModel
    {
        [Required]
        public Guid Id { get; set; }
        public string DiscountCode { get; set; }
        public string SicCode { get; set; }
        public int DiscountType { get; set; }
        [Required]
        public string NameDiscountLevel { get; set; }
        public decimal DiscountCheckValue { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public string FileExt { get; set; }
        public string FolderType { get; set; }
        public int DeleteFlag { get; set; }
        [MaxLength(100)]
        public string OwnerType { get; set; }
        [MaxLength(255)]
        public string OwnerCode { get; set; }
    }
}
