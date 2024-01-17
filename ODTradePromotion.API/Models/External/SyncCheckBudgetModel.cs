using System;

namespace ODTradePromotion.API.Models.External
{
    public class SyncCheckBudgetModel
    {
        public string BudgetCode { get; set; } = string.Empty;
        public string? BudgetType { get; set; } = string.Empty;
        public string CustomerCode { get; set; } = string.Empty;
        public string CustomerShipTo { get; set; } = string.Empty;
        public string SaleOrg { get; set; } = string.Empty;
        public string BudgetAllocationLevel { get; set; } = string.Empty;
        public double BudgetBook { get; set; }
        public string SalesTerritoryValueCode { get; set; } = string.Empty;
        public string PromotionCode { get; set; } = string.Empty;
        public string PromotionLevel { get; set; } = string.Empty;
        public string RouteZoneCode { get; set; } = string.Empty;
        public string DSACode { get; set; } = string.Empty;
        public string SubAreaCode { get; set; } = string.Empty;
        public string AreaCode { get; set; } = string.Empty;
        public string SubRegionCode { get; set; } = string.Empty;
        public string RegionCode { get; set; } = string.Empty;
        public string BranchCode { get; set; } = string.Empty;
        public string NationwideCode { get; set; } = string.Empty;
        public string SalesOrgCode { get; set; } = string.Empty;
        public string ReferalCode { get; set; } = string.Empty;

    }
}
