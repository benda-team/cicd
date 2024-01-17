using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Discount
{
    public class DiscountResultModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Scheme { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string SaleOrg { get; set; }
        public string DiscountFrequency { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public decimal AmountCalculate { get; set; }
        public List<DiscountResultDetailModel> DiscountResultDetails { get; set; } = new();
    }
    public class DiscountResultDetailModel
    {
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string LevelName { get; set; }
        public int CheckBy { get; set; }
        public decimal LevelCheckValue { get; set; }
        public decimal LevelAmount { get; set; }
        public decimal? LevelPercent { get; set; }
        public decimal DiscountAmount { get; set; }
    }

    public class DiscountResultParameters
    {
        public string DiscountCode { get; set; }
        public Guid DiscountLevelId { get; set; }
        public decimal PurchaseAmount { get; set; }
    }
}
