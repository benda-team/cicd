using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class KsResource
    {
        public Guid Id { get; set; }
        public string ResourceId { get; set; }
        public string ResourceType { get; set; }
        public string FilePath { get; set; }
        public string ExternalLink { get; set; }
        public string ResDesc { get; set; }
        public string FileName { get; set; }
        public string SyncCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
