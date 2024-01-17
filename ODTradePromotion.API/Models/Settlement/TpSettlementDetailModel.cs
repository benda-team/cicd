using ODTradePromotion.API.Models.Discount;
using ODTradePromotion.API.Models.Promotion;
using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Settlement
{
    public class TpSettlementDetailModel
    {
        public string SettlementCode { get; set; }
        public string ProgramType { get; set; }
        public string PromotionDiscountCode { get; set; }
        public string DistributorCode { get; set; }
        public string DistributorName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal? Quantity { get; set; }
        public string Package { get; set; }
        public string PackageName { get; set; }
        public decimal? Amount { get; set; }
        public string OrdNbr { get; set; }
        public string Status { get; set; }

        public DateTime OrdDate { get; set; }
        public string PromotionDiscountName { get; set; }
        public string PromotionLevel { get; set; }
        public string PromotionLevelName { get; set; }
        public string CustomerID { get; set; }
        public string ShiptoID { get; set; }
        public string ShiptoName { get; set; }
        public string ReferenceLink { get; set; }
        public string SalesRepCode { get; set; }
    }

    public class ListTpSettlementDetailModel
    {
        public List<TpSettlementDetailModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }

    public class DistForPromotionByListPromotionRequest
    {
        public string SettlementCode { get; set; }
        public string ProgramType { get; set; }
        public int FrequencySettlement { get; set; }
        public List<TpPromotionGeneralModel> ListPromotion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class DistForPromotionByPromotionRequest
    {
        public string SettlementCode { get; set; }
        public string ProgramType { get; set; }
        public int FrequencySettlement { get; set; }
        public TpPromotionGeneralModel Promotion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class DistForDiscountRequest
    {
        public string SettlementCode { get; set; }
        public string ProgramType { get; set; }
        public TpDiscountModel Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
