using System;
using System.Numerics;

namespace ODTradePromotion.API.Models.CheckBudget
{
    public class QueryReturn
    {
        public string BudgetCode { get; set; } = string.Empty;
        public string CustomerCode { get; set; } = string.Empty;
        public string CustomerShiptoCode { get; set; } = string.Empty;
        public double? QuantityUsed { get; set; }
        public double? AmountUsed { get; set; }
    }
    public class QueryFourReturn
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string SaleOrg { get; set; } = string.Empty;
        public string BudgetType { get; set; } = string.Empty;
        public string BudgetAllocationForm { get; set; } = string.Empty;
        public double TotalBudget { get; set; } 
        public double BudgetAvailable { get; set; }
        public double BudgetUsed { get; set; }
        public int LimitBudgetPerCustomer { get; set; }
        public string BudgetAllocationLevel { get; set; } = string.Empty;
        public string SalesTerritoryValueCode { get; set; } = string.Empty;
        public double? STTotalBudget { get; set; }
        public double? STBudgetUsed { get; set; }
        public double? STBudgetAvailable { get; set; }
        public double? STLimitBudgetPerCustomer { get; set; }
        public bool FlagOverBudget { get; set; } = false;
    }
}
