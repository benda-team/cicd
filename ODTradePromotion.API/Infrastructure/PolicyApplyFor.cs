using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class PolicyApplyFor
    {
        public Guid Id { get; set; }
        public string PolicyCode { get; set; }
        public string Type { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string JobTitleCode { get; set; }
        public string JobTitleName { get; set; }
        public string JobTitleDefaultName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
