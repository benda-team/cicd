using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.External.Sic
{
    public class SicPrimaryModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
