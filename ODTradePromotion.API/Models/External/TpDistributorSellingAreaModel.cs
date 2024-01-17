using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.External
{
    public class TpDistributorSellingAreaModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Guid? TerritoryMappingId { get; set; }
        public string Area { get; set; }
        public string TerritoryStructureCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime EffectDate { get; set; }
        public DateTime? UntilDate { get; set; }
        public bool IsChecked { get; set; }
    }
}
