using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Item
{
    public class UomsModel
    {
        public Guid Id { get; set; }
        public string UomId { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime? EffectiveDateBefore { get; set; }
        public DateTime? ValidUntil { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsChecked { get; set; }
        public bool IsUse { get; set; }
    }
    public class UomsListModel
    {
        public List<UomsModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
