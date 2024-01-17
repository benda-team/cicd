namespace ODTradePromotion.API.Models.Budget
{
    public class BudgetRemainAndCustomerBudgetModel
    {
        public string BudgetCode { get; set; }
        public string BudgetType { get; set; }
        public string Type { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerShiptoCode { get; set; }
        public string BudgetAllocationLevel { get; set; }
        public string SalesTerritoryValueCode { get; set; }
        public string BudgetRemains { get; set; }
        public string CustomerBudget { get; set; }
        public float BudgetBooked { get; set; }
    }
}
