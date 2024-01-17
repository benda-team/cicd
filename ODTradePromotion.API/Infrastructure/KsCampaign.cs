using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class KsCampaign
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string SurveyCode { get; set; }
        public string Name { get; set; }
        public string SurveyMethod { get; set; }
        public string ApplicableObjects { get; set; }
        public string PeopleSurvey { get; set; }
        public string ScopeType { get; set; }
        public string EvaluationType { get; set; }
        public string EvaluationFrequency { get; set; }
        public string EveryDays { get; set; }
        public string Month { get; set; }
        public int Day { get; set; }
        public string Days { get; set; }
        public string RequiredType { get; set; }
        public string RequiredValue { get; set; }
        public string Description { get; set; }
        public DateTime StartEvaluationDate { get; set; }
        public DateTime EndEvaluationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsImplementSurvey { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string BusinessPartnerType { get; set; }
        public string SurveyedSubjects { get; set; }
    }
}
