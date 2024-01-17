using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class SaleProcessSetting
    {
        public Guid Id { get; set; }
        public string RefNo { get; set; }
        public string Description { get; set; }
        public string ShopFormat { get; set; }
        public string StepCode { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
