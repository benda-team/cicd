using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.External
{
    public class TpTerritoryValueModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Source { get; set; }//Branch,        Region,        Area
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? UntilDate { get; set; }
        public bool IsChecked { get; set; }
    }
}
