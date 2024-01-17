using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Temp
{
    public class TempTpOrderHeaderModel
    {
        public Guid Id { get; set; }
        public string OrdNbr { get; set; }
        public DateTime OrdDate { get; set; }
        public string PrincipalID { get; set; }
        public string Disty_BilltoCode { get; set; }
        public string Disty_BilltoName { get; set; }
        public string PeriodCode { get; set; }
        public string SalesRepCode { get; set; }
        public string RouteZoneID { get; set; }
        public string RouteZoneName { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string ShiptoID { get; set; }
        public string ShiptoName { get; set; }
        public string CustomerAttribute0 { get; set; }
        public string CustomerAttribute1 { get; set; }
        public string CustomerAttribute2 { get; set; }
        public string CustomerAttribute3 { get; set; }
        public string CustomerAttribute4 { get; set; }
        public string CustomerAttribute5 { get; set; }
        public string CustomerAttribute6 { get; set; }
        public string CustomerAttribute7 { get; set; }
        public string CustomerAttribute8 { get; set; }
        public string CustomerAttribute9 { get; set; }
        public string Status { get; set; }
        public string RecallOrderCode { get; set; }
        public string DiscountCode { get; set; }
        public string DiscountName { get; set; }
        public decimal SO_Shipped_Disc_Amt { get; set; }
        public string ReferenceLink { get; set; }
        public string SalesOrg_Code { get; set; }
        public string Branch_Code { get; set; }
        public string Region_Code { get; set; }
        public string SubRegion_Code { get; set; }
        public string Area_Code { get; set; }
        public string SubArea_Code { get; set; }
        public string DSA_Code { get; set; }
        public string NSD_Code { get; set; }
        public string Branch_Manager_Code { get; set; }
        public string Region_Manager_Code { get; set; }
        public string Sub_Region_Manager_Code { get; set; }
        public string Area_Manager_Code { get; set; }
        public string Sub_Area_Manager_Code { get; set; }
        public string DSA_Manager_Code { get; set; }
        public string RZ_Suppervisor_Code { get; set; }
        public string SIC_Code { get; set; }
    }
}
