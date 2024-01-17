using Sys.Common.Models;
using System;
using System.Collections.Generic;

namespace ODTradePromotion.API.Models.Budget
{
    public class TpBudgetAdjustmentListModel
    {
        public Guid Id { get; set; }
        public string BudgetCode { get; set; }
        public string Account { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public string Reason { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FolderType { get; set; }
        public string FileExt { get; set; }
        public int CountAdjustment { get; set; }
        public int Type { get; set; }
    }
    public class ListTpBudgetAdjustmentListModel
    {
        public List<TpBudgetAdjustmentListModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
