using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class PrincipalPullDataQueue
    {
        public Guid Id { get; set; }
        public string DataType { get; set; }
        public string Owner { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime? ExcuteDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string OwnerId { get; set; }
        public string OwnerShiptoId { get; set; }
    }
}
