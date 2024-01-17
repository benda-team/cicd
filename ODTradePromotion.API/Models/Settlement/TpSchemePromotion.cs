using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Settlement
{
    public class TpSchemePromotionModel
    {
        public string Scheme { get; set; }
    }

    public class TpSchemePromotionListModel
    {
        public List<TpSchemePromotionModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
