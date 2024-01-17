using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Promotion
{
    public class TpPromotionGeneralModel
    {
        public Guid Id { get; set; }
        public string PromotionType { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public string Scheme { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public string SaleOrg { get; set; }
        public string SicCode { get; set; }
        public int SettlementFrequency { get; set; }
        public string FrequencyPromotion { get; set; }
        public string ScopeType { get; set; }
        public string ScopeTypeDescription { get; set; }
        public string ScopeSaleTerritoryLevel { get; set; }
        public string ApplicableObjectType { get; set; }
        public string ApplicableObjectTypeDescription { get; set; }
        public bool IsApplyBudget { get; set; }
        public string UserName { get; set; }
        public bool IsChecked { get; set; }
    }

    public class TpPromotionGeneralListModel
    {
        public List<TpPromotionGeneralModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
