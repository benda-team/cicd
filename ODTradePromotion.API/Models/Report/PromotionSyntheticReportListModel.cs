using Sys.Common.Models;
using System;
using System.Collections.Generic;

namespace ODTradePromotion.API.Models.Report
{
    public class PromotionSyntheticReportListModel
    {
        public string Code { get; set; }
        public string ShortName { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public string ScopeType { get; set; }
        public string ScopeTypeDescription { get; set; }
        public string ApplicableObjectType { get; set; }
        public string ApplicableObjectTypeDescription { get; set; }
        public string LevelName { get; set; }
        public string ProductName { get; set; }
        public string Packing { get; set; }
        //public string BudgetCode { get; set; }
        //public string BudgetName { get; set; }
        //public string BudgetType { get; set; }
        //public decimal? BudgetQuantity { get; set; }
        //public decimal? BudgetQuantityUsed { get; set; }
        public string DonateApplyBudgetCode { get; set; }
        public string GiftApplyBudgetCode { get; set; }
        public List<BudgetInfo> BudgetInfos { get; set; }
    }
    public class ListPromotionSyntheticReportListModel
    {
        public List<PromotionSyntheticReportListModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }

    public class BudgetInfo
    {
        public string Code { get; set; }
        public string DonateApplyBudgetCode { get; set; }
        public string GiftApplyBudgetCode { get; set; }
        public string BudgetCode { get; set; }
        public string BudgetName { get; set; }
        public double? BudgetQuantity { get; set; }
        public double? BudgetQuantityUsed { get; set; }
    }
}
