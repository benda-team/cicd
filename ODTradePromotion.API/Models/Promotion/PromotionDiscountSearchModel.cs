using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Promotion
{
    public class PromotionDiscountSearchModel
    {
        public string KeySearch { get; set; }
        public string ProgramType { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public string ShortName { get; set; }
        public string Scope { get; set; }
        public string Applicable { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public bool? ApplyBudget { get; set; }
        public string Orderby { get; set; }
        public string OrderbyType { get; set; } = "asc";
        public bool IsDropdown { get; set; }
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public int? Skip { get; set; }
        public int? Top { get; set; }
        public string PrincipalCode { get; set; }
    }
}
