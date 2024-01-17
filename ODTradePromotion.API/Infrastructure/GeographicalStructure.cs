using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class GeographicalStructure
    {
        public Guid Id { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? MasterId { get; set; }
        public int? Level { get; set; }
        public bool IsActive { get; set; }
        public int? DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
