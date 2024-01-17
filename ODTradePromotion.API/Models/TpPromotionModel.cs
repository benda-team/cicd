using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models
{
    public class TpPromotionModel
    {
        public Guid Id { get; set; }
        public string PromotionType { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string ScopeTypeText { get; set; }
        public string ApplicableObjectTypeText { get; set; }
        public string Status { get; set; }
        public string Scheme { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public string SaleOrg { get; set; }
        public string SicCode { get; set; }
        public int SettlementFrequency { get; set; }
        public string FrequencyPromotion { get; set; }
        public string ImageName1 { get; set; }
        public string ImagePath1 { get; set; }
        public string ImageName2 { get; set; }
        public string ImagePath2 { get; set; }
        public string ImageName3 { get; set; }
        public string ImagePath3 { get; set; }
        public string ImageName4 { get; set; }
        public string ImagePath4 { get; set; }
        public string ScopeType { get; set; }
        public string ScopeSaleTerritoryLevel { get; set; }
        public bool IsProgram { get; set; }

        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ApplicableObjectType { get; set; }
        public bool PromotionCheckBy { get; set; }
        public bool RuleOfGiving { get; set; }
        public string UserName { get; set; }
    }

    public class ListPromotionModel
    {
        public List<TpPromotionModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}