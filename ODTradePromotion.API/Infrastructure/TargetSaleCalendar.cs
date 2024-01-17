using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class TargetSaleCalendar
    {
        public Guid Id { get; set; }
        public string SyncCode { get; set; }
        public DateTime? Date { get; set; }
        public string WeekId { get; set; }
        public int Week { get; set; }
        public string SalesperiodId { get; set; }
        public int Salesperiod { get; set; }
        public string QuaterlyId { get; set; }
        public int Quaterly { get; set; }
        public string YearId { get; set; }
        public int Year { get; set; }
        public bool IsHoliday { get; set; }
        public string HolidayDesc { get; set; }
        public int NumOfWkDay { get; set; }
        public string StartWeek { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
