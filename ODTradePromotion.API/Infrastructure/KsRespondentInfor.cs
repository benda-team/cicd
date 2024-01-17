using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class KsRespondentInfor
    {
        public Guid Id { get; set; }
        public string SicId { get; set; }
        public Guid RespondentInfoCode { get; set; }
        public string CampainCode { get; set; }
        public string RespondentInfoKey { get; set; }
        public string RespondentInfoValue { get; set; }
        public string SurveyResultCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
