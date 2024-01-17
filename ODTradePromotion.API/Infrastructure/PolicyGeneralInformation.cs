using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class PolicyGeneralInformation
    {
        public Guid Id { get; set; }
        public string PolicyCode { get; set; }
        public string PolicyName { get; set; }
        public string PolicyDescription { get; set; }
        public string AvatarFilePath { get; set; }
        public string AvatarFileName { get; set; }
        public string AvatarFileFormat { get; set; }
        public string Type { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string ApplyTo { get; set; }
        public string ApplicableTo { get; set; }
        public string Status { get; set; }
        public bool IsImediateLy { get; set; }
        public bool IsNotify { get; set; }
        public bool CompulsoryContent { get; set; }
        public bool IsDeleted { get; set; }
        public int NeedToApproached { get; set; }
        public int Approached { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
