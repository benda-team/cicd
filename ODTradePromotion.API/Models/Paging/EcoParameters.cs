using System;

namespace ODTradePromotion.API.Models.Paging
{
    public class EcoParameters
    {
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
        public string OrderBy { get; set; }
        public string Filter { get; set; }
        public bool IsDropdown { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SearchValue { get; set; }
        public string SearchText { get; set; }
        public string PrincipleCode { get; set; }
        public int? LogLevel { get; set; }
        public string UserName { get; set; }
        public string FeatureCode { get; set; }
    }
}
