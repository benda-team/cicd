using System;
using System.Collections.Generic;

namespace ODTradePromotion.API.Models
{
    public class SystemUrlModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
