using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class Province
    {
        public Guid Id { get; set; }
        public string ProvinceCode { get; set; }
        public string Description { get; set; }
        public string ShortName { get; set; }
        public int ProvincePhoneCode { get; set; }
        public int? ZipCode { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
