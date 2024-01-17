using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class KsApplicapleAssignment
    {
        public Guid Id { get; set; }
        public string CampainCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerShiptoCode { get; set; }
        public string DistributorShiptoCode { get; set; }
        public string DistributorCode { get; set; }
        public string AttId { get; set; }
        public string AttName { get; set; }
        public string AttDesc { get; set; }
        public string AttCode { get; set; }
        public string AttCodeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
