using Sys.Common.Models;
using System.Collections.Generic;

namespace ODTradePromotion.API.Models.Report
{
    public class PromotionSyntheticReportSettlementListModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string SalesPeriod { get; set; }
        public decimal? DistributorQuantity { get; set; }
        public decimal? DistributorQuantityConfirm { get; set; }
        public decimal? DistributorQuantityUnConfirm { get; set; }
    }

    public class ListPromotionSyntheticReportSettlementListModel
    {
        public List<PromotionSyntheticReportSettlementListModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
