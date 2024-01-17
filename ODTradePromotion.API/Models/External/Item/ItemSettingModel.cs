using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.External.Item
{
    public class ItemSettingModel
    {
        public Guid Id { get; set; }

        public string AttributeId { get; set; }

        public string AttributeName { get; set; }

        public string Description { get; set; }
        public bool IsHierarchy { get; set; }
        public bool Used { get; set; }
        public int? Level { get; set; }
    }
}
