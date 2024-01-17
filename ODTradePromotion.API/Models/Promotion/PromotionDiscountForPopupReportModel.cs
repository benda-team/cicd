using Sys.Common.Models;
using System;
using System.Collections.Generic;

namespace ODTradePromotion.API.Models.Promotion
{
    public class PromotionDiscountForPopupReportModel
    {
        public Guid Id { get; set; }
        public string ProgramType { get; set; }
        public string ProgramTypeDescription { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
    }

    public class PromotionDiscountForPopupReportListModel
    {
        public List<PromotionDiscountForPopupReportModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
