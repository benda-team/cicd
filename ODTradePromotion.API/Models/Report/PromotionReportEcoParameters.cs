using ODTradePromotion.API.Models.Paging;
using System;
using System.Collections.Generic;

namespace ODTradePromotion.API.Models.Report
{
    public class PromotionReportEcoParameters : EcoParameters
    {
        public string PromotionCode { get; set; }
        public string PromotionLevelCode { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public string SaleOrg { get; set; }
        public string ScopeType { get; set; }
        public string ApplicableObjectType { get; set; }
        public List<string> ListApplicableObject { get; set; }
        public List<string> ListScope { get; set; }
        public List<string> ListCustomer { get; set; }
        public List<string> ListRouteZone { get; set; }
        public List<string> ListOrder { get; set; }
    }
}
