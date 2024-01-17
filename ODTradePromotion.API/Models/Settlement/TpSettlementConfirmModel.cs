using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Settlement
{
    public class TpSettlementConfirmModel
    {
        public string SettlementCode { get; set; }
        public string SettlementName { get; set; }
        public string ProgramType { get; set; }
        public string ProgramName { get; set; }
        public string DistributorCode { get; set; }
        public string DistributorName { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
    }

    public class TpSettlementConfirmListModel
    {
        public List<TpSettlementConfirmModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }

    public class TpDefinitionStructureForSettlementModel
    {
        public string SettlementCode { get; set; }
        public bool IsGiftProduct { get; set; }
        public bool IsDonate { get; set; }
        public bool IsFixMoney { get; set; }
    }
}
