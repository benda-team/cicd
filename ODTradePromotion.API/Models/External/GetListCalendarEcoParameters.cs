using ODTradePromotion.API.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.External
{
    public class GetListCalendarEcoParameters : EcoParameters
    {
        public string Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
