using ODTradePromotion.API.Models.Paging;
using ODTradePromotion.API.Models.Promotion;
using Sys.Common.Models;
using System;
using System.Collections.Generic;

namespace ODTradePromotion.API.Models.Report
{
    public class PromotionDetailReportOrderListModel
    {
        public string OrdNbr { get; set; }
        public DateTime OrdDate { get; set; }
        public string PromotionLevel { get; set; }
        public string InventoryID { get; set; }
        public string InventoryName { get; set; }
        public decimal? Shipped_Qty { get; set; }
        public string PackSize { get; set; }
        public decimal? ShippedLineDiscAmt { get; set; }
        public string CustomerID { get; set; }
        public string ShiptoID { get; set; }
        public string ShiptoName { get; set; }
        public string ReferenceLink { get; set; }
        public string SalesRepCode { get; set; }
    }

    public class ListPromotionDetailReportOrderListModel
    {
        public List<PromotionDetailReportOrderListModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
