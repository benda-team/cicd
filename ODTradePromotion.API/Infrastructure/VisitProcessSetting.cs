using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class VisitProcessSetting
    {
        public Guid Id { get; set; }
        public string RefNo { get; set; }
        public string Description { get; set; }
        public string Objectives { get; set; }
        public string Purpose { get; set; }
        public string VisitType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string Status { get; set; }
    }
}
