using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class PolicyResource
    {
        public Guid Id { get; set; }
        public string PolicyCode { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileFormat { get; set; }
        public bool AllowDownload { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
