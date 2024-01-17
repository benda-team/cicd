namespace ODTradePromotion.API.Models.Budget
{
    public class FilterConfigBudget
    {
        public string BudgetCode { get; set; }
        public string BudgetType { get; set; }
        public string BudgetAllocationLevel { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionLevel { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerShiptoCode { get; set; }
        public string SalesOrgCode { get; set; }
        public string RouteZoneCode { get; set; }
        public string DSACode { get; set; }
        public string SubAreaCode { get; set; }
        public string AreaCode { get; set; }
        public string SubRegionCode { get; set; }
        public string RegionCode { get; set; }
        public string BranchCode { get; set; }
        public string NationwideCode { get; set; }
        public string ReferalCode { get; set; }
        public float BudgetBook { get; set; }
    }

    public class ConfigBudgetModel
    {
        public string Code { get; set; }
        public string SaleOrg { get; set; }
        public string BudgetType { get; set; }
        public string BudgetAllocationForm { get; set; }    
        public float TotalBudget { get; set; }
        public float BudgetAvailable { get; set; }
        public float BudgetUsed { get; set; }   
        public float LimitBudgetPerCustomer { get; set; }
        public string BudgetAllocationLevel { get; set; }
        public string SalesTerritoryValueCode { get; set; } 
        public float STTotalBudget { get; set; } 
        public float STBudgetUsed { get; set; }
        public float STBudgetAvailable { get; set; }
        public float STLimitBudgetPerCustomer { get; set; }
    }
}
