using ODTradePromotion.API.Models.Item;
using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Promotion
{
    public class TpPromotionDefinitionProductForGiftModel
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string PromotionCode { get; set; }
        [Required]
        [MaxLength(10)]
        public string LevelCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string Packing { get; set; }
        public string PackingDescription { get; set; }
        public int NumberOfGift { get; set; }
        public string BudgetCode { get; set; }
        public string BudgetName { get; set; }
        public bool IsDefaultProduct { get; set; }
        public int Exchange { get; set; }
        public string ItemHierarchyLevel { get; set; }
        public string ProductType { get; set; }
        public List<UomsModel> ListUom { get; set; } = new List<UomsModel>();
    }
}
