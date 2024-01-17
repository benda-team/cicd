using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class TempTpOrderHeader
    {
        public Guid Id { get; set; }
        public string OrdNbr { get; set; }
        public DateTime OrdDate { get; set; }
        public string PrincipalId { get; set; }
        public string DistyBilltoCode { get; set; }
        public string PeriodCode { get; set; }
        public string SalesRepCode { get; set; }
        public string Dsacode { get; set; }
        public string RouteZoneId { get; set; }
        public string CustomerId { get; set; }
        public string ShiptoId { get; set; }
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
        public decimal SoShippedDiscAmt { get; set; }
    }
}
