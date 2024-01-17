using System.Collections.Generic;

namespace ODTradePromotion.API.Models.Budget
{
    public class FilterBudgetRemainAndCustomerBudget
    {
        public List<string> BudgetCode { get; set; }
        public string SalesOrgCode { get; set; }
        public string RouteZoneCode { get; set; }

        public string DSACode { get; set; }
        public string SubAreaCode { get; set; }
        public string AreaCode { get; set; }
        public string SubRegionCode { get; set; }
        public string RegionCode { get; set; }
        public string BranchCode { get; set; }
        public string NationwideCode { get; set; }
    }
}
