using ODTradePromotion.API.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.SalesOrg
{
    public class TerritoryValueEcoParameters : EcoParameters
    {
        public string TerritoryStructureCode { get; set; }
        public string TerritoryLevelCode { get; set; }
    }
}
