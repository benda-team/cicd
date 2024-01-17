using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class RegistrationQueue
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int QueueNumber { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionLevel { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerShiptoCode { get; set; }
        public string RouteZoneCode { get; set; }
        public string ProductSkuCode { get; set; }
        public int ProductNumber { get; set; }
        public decimal AmountOfDonation { get; set; }
        public string SaleOrg { get; set; }
        public string SicCode { get; set; }
        public string DsaCode { get; set; }
        public string CountryCode { get; set; }
        public string BranchCode { get; set; }
        public string RegionCode { get; set; }
        public string SubRegionCode { get; set; }
        public string AreaCode { get; set; }
        public string SubAreaCode { get; set; }
        public string Key
        {
            get { return $"{PromotionCode}-{PromotionLevel}-{CustomerCode}-{CustomerShiptoCode}-{RouteZoneCode}"; }
            set { value = $"{PromotionCode}-{PromotionLevel}-{CustomerCode}-{CustomerShiptoCode}-{RouteZoneCode}"; }
        }
    }
}
