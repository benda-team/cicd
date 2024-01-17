using Sys.Common.Models;
using System;
using System.Collections.Generic;

namespace ODTradePromotion.API.Models.Promotion
{
    public class PromotionDiscountModel
    {
        public Guid Id { get; set; }
        public string ProgramType { get; set; }
        public string ProgramTypeDescription { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string ScopeType { get; set; }
        public string ScopeTypeDescription { get; set; }
        public string ApplicableObjectType { get; set; }
        public string ApplicableObjectTypeDescription { get; set; }
        public bool IsApplyBudget { get; set; }
        public string UserName { get; set; }
        public bool? IsDonateApplyBudget { get; set; }
        public string OwnerType { get; set; }
        public string OwnerCode { get; set; }
        public string PrincipalCode { get; set; }        
    }

    public class PromotionDiscountModelListModel
    {
        public List<PromotionDiscountModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
