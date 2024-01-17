using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class PolicyAccessReport
    {
        public Guid Id { get; set; }
        public string PolicyCode { get; set; }
        public string AccessedUser { get; set; }
        public string AccessedUserName { get; set; }
        public string JobTitle { get; set; }
        public DateTime AccessedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
