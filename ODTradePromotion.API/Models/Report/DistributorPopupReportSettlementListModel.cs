using Sys.Common.Models;
using System.Collections.Generic;

namespace ODTradePromotion.API.Models.Report
{
    public class DistributorPopupReportSettlementListModel
    {
        public string SettlementCode { get; set; }
        public string DistributorCode { get; set; }
        public string DistributorName { get; set; }
        public bool Confirm { get; set; }
        public bool UnConfirm { get; set; }
    }

    public class ListDistributorPopupReportSettlementListModel
    {
        public List<DistributorPopupReportSettlementListModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
