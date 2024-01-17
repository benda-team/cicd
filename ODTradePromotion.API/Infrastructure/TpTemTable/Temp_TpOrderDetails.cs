using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Infrastructure.TpTemTable
{
    public class Temp_TpOrderDetails
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string OrdNbr { get; set; }
        [MaxLength(10)]
        public string InventoryID { get; set; }
        public string InventoryName { get; set; }
        [MaxLength(10)]
        public string DiscountID { get; set; }
        [MaxLength(255)]
        public string DiscountName { get; set; }
        [MaxLength(10)]
        public string DiscountType { get; set; }
        [MaxLength(10)]
        public string DiscountSchemeID { get; set; }
        public bool IsFree { get; set; }   
        [MaxLength(100)]
        public string UOM { get; set; }
        [MaxLength(255)]
        public string UOMName { get; set; }
        public decimal? ShippedQty { get; set; }
        public decimal? ShippedLineDiscAmt { get; set; }
        public decimal UnitPrice { get; set; }
        [MaxLength(10)]
        public string PromotionLevel { get; set; }
        [MaxLength(255)]
        public string PromotionLevelName { get; set; }
    }
}
