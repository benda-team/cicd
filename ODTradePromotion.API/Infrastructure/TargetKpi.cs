using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class TargetKpi
    {
        public Guid Id { get; set; }
        public string SyncCode { get; set; }
        public string EmployeeCode { get; set; }
        public string SalesindicatorCode { get; set; }
        public string SalesindicatorName { get; set; }
        public string KpiFrequencyType { get; set; }
        public string KpiFrequencyId { get; set; }
        public string KpiType { get; set; }
        public double KpiTargetValue { get; set; }
        public double KpiActuallyAchievedValue { get; set; }
        public string KpiUom { get; set; }
        public DateTime? KpiUpdatedDataTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
