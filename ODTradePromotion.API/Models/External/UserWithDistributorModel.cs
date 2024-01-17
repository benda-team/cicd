using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.External
{
    public class UserWithDistributorModel
    {
        public Guid Id { get; set; }
        public string UserCode { get; set; }
        public string DistributorCode { get; set; }
        public string DistributorName { get; set; }
    }
}
