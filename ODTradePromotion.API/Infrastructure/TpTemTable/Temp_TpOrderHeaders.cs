using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Infrastructure.TpTemTable
{
    public class Temp_TpOrderHeaders
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string OrdNbr { get; set; }
        public DateTime OrdDate { get; set; }
        [MaxLength(10)]
        public string PrincipalID { get; set; }
        [MaxLength(10)]
        public string Disty_BilltoCode { get; set; }  
        public string Disty_BilltoName { get; set; }  
        [MaxLength(10)]
        public string PeriodCode { get; set; }
        [MaxLength(10)]
        public string SalesRepCode { get; set; }   
        [MaxLength(10)]
        public string RouteZoneID { get; set; }
        [MaxLength(255)]
        public string RouteZoneName { get; set; }
        [MaxLength(10)]
        public string CustomerID { get; set; }
        [MaxLength(255)]
        public string CustomerName { get; set; } 
        [MaxLength(10)]
        public string ShiptoID { get; set; }
        [MaxLength(255)]
        public string ShiptoName { get; set; }
        [MaxLength(100)]
        public string CustomerAttribute0 { get; set; }
        [MaxLength(100)]
        public string CustomerAttribute1 { get; set; }
        [MaxLength(100)]
        public string CustomerAttribute2 { get; set; }
        [MaxLength(100)]
        public string CustomerAttribute3 { get; set; }
        [MaxLength(100)]
        public string CustomerAttribute4 { get; set; }
        [MaxLength(100)]
        public string CustomerAttribute5 { get; set; }
        [MaxLength(100)]
        public string CustomerAttribute6 { get; set; }
        [MaxLength(100)]
        public string CustomerAttribute7 { get; set; }
        [MaxLength(100)]
        public string CustomerAttribute8 { get; set; }
        [MaxLength(100)]
        public string CustomerAttribute9 { get; set; }
        [MaxLength(100)]
        public string Status { get; set; }
        [MaxLength(10)]
        public string RecallOrderCode { get; set; }
        [MaxLength(10)]
        public string DiscountCode { get; set; }
        public string DiscountName { get; set; }
        public decimal SO_Shipped_Disc_Amt { get; set; }
        [MaxLength(255)]
        public string ReferenceLink { get; set; }
        [MaxLength(10)]
        public string SalesOrg_Code { get; set; }
        [MaxLength(10)]
        public string Branch_Code { get; set; }        
        [MaxLength(10)]
        public string Region_Code { get; set; }
        [MaxLength(10)]
        public string SubRegion_Code { get; set; }
        [MaxLength(10)]
        public string Area_Code { get; set; }
        [MaxLength(10)]
        public string SubArea_Code { get; set; }
        [MaxLength(10)]
        public string DSA_Code { get; set; }
        [MaxLength(10)]
        public string NSD_Code { get; set; }
        [MaxLength(10)]
        public string Branch_Manager_Code { get; set; }
        [MaxLength(10)]
        public string Region_Manager_Code { get; set; }
        [MaxLength(10)]
        public string Sub_Region_Manager_Code { get; set; }
        [MaxLength(10)]
        public string Area_Manager_Code { get; set; }
        [MaxLength(10)]
        public string Sub_Area_Manager_Code { get; set; }
        [MaxLength(10)]
        public string DSA_Manager_Code { get; set; }
        [MaxLength(10)]
        public string RZ_Suppervisor_Code { get; set; }
        [MaxLength(10)]
        public string SIC_Code { get; set; }
    }
}
