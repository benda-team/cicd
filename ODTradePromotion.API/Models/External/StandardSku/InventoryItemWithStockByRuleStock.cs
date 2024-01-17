using Sys.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.External.StandardSku
{
    public class InventoryItemWithStockByRuleStock
    {
        public Guid ItemGroupId { get; set; }
        public string ItemGroupCode { get; set; }
        public Guid InventoryId { get; set; }
        public string InventoryCode { get; set; }
        public string InventoryDescription { get; set; }
        public int? StdRuleCode { get; set; }
        public string StdRuleName
        {
            get
            {
                var result = "";
                if (StdRuleCode == CommonData.PriorityStandard.Priority)
                {
                    result = "Priority";
                }
                else if (StdRuleCode == CommonData.PriorityStandard.Ratio)
                {
                    result = "Ratio";
                }
                else if (StdRuleCode == CommonData.PriorityStandard.PriorityByTime)
                {
                    result = "PriorityByTime";
                }
                else
                {
                    result = "";
                }
                return result;
            }
        }
        public int? Priority { get; set; }
        public DateTime? EffectiveDateFrom { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool AllocateToItemGroup { get; set; }
        public int? Ratio { get; set; }
        public int? OnHand { get; set; }
        public int? Avaiable { get; set; }
        public string BudgetCode { get; set; }
    }
}
