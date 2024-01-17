using ODTradePromotion.API.Models.Paging;
using ODTradePromotion.API.Models.Promotion;
using Sys.Common.Models;
using System;
using System.Collections.Generic;

namespace ODTradePromotion.API.Models.Report
{
    public class PromotionDetailReportPointSaleListModel
    {
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string ShiptoID { get; set; }
        public string ShiptoName { get; set; }
        public string PromotionLevel { get; set; }
        public string PromotionLevelName { get; set; }
        public string ReferenceLink { get; set; }
        public string SalesRepCode { get; set; }
    }

    public class ListPromotionDetailReportPointSaleListModel
    {
        public List<PromotionDetailReportPointSaleListModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
