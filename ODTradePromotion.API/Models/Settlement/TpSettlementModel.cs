using ODTradePromotion.API.Models.Promotion;
using Sys.Common.Constants;
using Sys.Common.Models;
using System;
using System.Collections.Generic;

namespace ODTradePromotion.API.Models.Settlement
{
    public class TpSettlementModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ProgramType { get; set; } = CommonData.PromotionSetting.PromotionProgram;
        public string SettlementTypeName { get; set; }
        public bool SchemeType { get; set; } = false;
        public string Status { get; set; }
        public string StatusName { get; set; }
        public DateTime SettlementDate { get; set; }
        public bool IsChecked { get; set; }
        public bool? IsDeleted { get; set; }
        public string PromotionDiscountCode { get; set; }
        public string PromotionDiscountName { get; set; }
        public string PromotionDiscountScheme { get; set; }
        public int FrequencySettlement { get; set; }
        public string FrequencyCode { get; set; }
        public string SaleCalendarCode { get; set; }
        public decimal TotalDistributor { get; set; }
        public decimal? TotalAmount { get; set; }
        public List<TpPromotionGeneralModel> ListPromotionModels { get; set; } = new List<TpPromotionGeneralModel>();
        public List<TpSettlementObjectModel> ListTpSettlementObjects { get; set; } = new List<TpSettlementObjectModel>();
        public List<TpSettlementDetailModel> TpSettlementDetailModels { get; set; } = new List<TpSettlementDetailModel>();

    }

    public class ListSettlemenListModel
    {
        public List<TpSettlementModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
