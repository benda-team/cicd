namespace ODTradePromotion.API.Models.CheckBudget
{
    public class CheckBudgetOutput
    {
        public string BudgetCode { get; set; } = string.Empty;
        public string ReferalCode { get; set; } = string.Empty;
        public string BudgetType { get; set; } = string.Empty;
        public string PromotionCode { get; set; } = string.Empty;
        public string PromotionLevel { get; set; } = string.Empty;
        public string CustomerCode { get; set; } = string.Empty;
        public string CustomerShiptoCode { get; set; } = string.Empty;
        public double BudgetBook { get; set; }
        public double BudgetBooked { get; set; }
        public bool BudgetBookOver { get; set; } = false;
        public bool Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
