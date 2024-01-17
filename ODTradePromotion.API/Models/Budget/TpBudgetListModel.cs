using Sys.Common.Models;
using System;
using System.Collections.Generic;

namespace ODTradePromotion.API.Models.Budget
{
    public class TpBudgetListModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string IO { get; set; }
        public string SaleOrg { get; set; }
        public string BudgetType { get; set; }
        public string BudgetAllocationForm { get; set; }
        public string BudgetAllocationLevel { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
    }

    public class ListBudgetListModel
    {
        public List<TpBudgetListModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
