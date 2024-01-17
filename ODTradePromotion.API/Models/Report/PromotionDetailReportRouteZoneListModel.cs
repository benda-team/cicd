using ODTradePromotion.API.Models.Paging;
using Sys.Common.Models;
using System;
using System.Collections.Generic;

namespace ODTradePromotion.API.Models.Report
{
    public class PromotionDetailReportRouteZoneListModel
    {
        public string RouteZoneId { get; set; }
        public string RouteZoneDescription { get; set; }
        public string PromotionLevel { get; set; }
        public string PromotionLevelName { get; set; }
        public string SalesRepCode { get; set; }
        public string ReferenceLink { get; set; }
    }

    public class ListPromotionDetailReportRouteZoneListModel
    {
        public List<PromotionDetailReportRouteZoneListModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}