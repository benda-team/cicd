using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class KsSurveyorResource
    {
        public Guid Id { get; set; }
        public string CampainCode { get; set; }
        public string JobTitleCode { get; set; }
        public string EmployeeCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
