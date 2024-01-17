using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpBudgetUsed
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string BudgetCode { get; set; }
        [MaxLength(10)]
        public string BudgetType { get; set; }
        public decimal AmountUsed { get; set; }
        public decimal QuantityUsed { get; set; }


        public string CustomerCode { get; set; }
        public string ShiptoCode { get; set; }
        public string RouteZoneCode { get; set; }

        public string SaleOrgCode { get; set; }
        public string CountryCode { get; set; }
        public string BranchCode { get; set; }
        public string RegionCode { get; set; }
        public string SubRegionCode { get; set; }
        public string AreaCode { get; set; }
        public string SubAreaCode { get; set; }
        public string DSACodeCode { get; set; }
        public string Key { get; set; }
    }
}
