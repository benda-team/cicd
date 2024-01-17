using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class KsSurveyResult
    {
        public Guid Id { get; set; }
        public string SicId { get; set; }
        public string SurveyCode { get; set; }
        public string SurveyDescription { get; set; }
        public string CampainCode { get; set; }
        public string CampainName { get; set; }
        public string CampainPeopleSurvey { get; set; }
        public string CampainEvaluationType { get; set; }
        public string CampainEvaluationFrequency { get; set; }
        public DateTime? CampainStartEvaluationDate { get; set; }
        public DateTime? CampainEndEvaluationDate { get; set; }
        public string CampainSurveyMethod { get; set; }
        public string CampainRequiredType { get; set; }
        public string CampainRequiredValue { get; set; }
        public string CampainDescription { get; set; }
        public DateTime? CampainStartDate { get; set; }
        public DateTime? CampainEndDate { get; set; }
        public bool IsImplementSurvey { get; set; }
        public string QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public int QuestionOrdinal { get; set; }
        public string QuestionType { get; set; }
        public Guid AnswerId { get; set; }
        public string AnswerValue { get; set; }
        public string QuestionDependentType { get; set; }
        public string AnswerRelatedQuestionId { get; set; }
        public bool IsInputAnswer { get; set; }
        public string AnswerInputValue { get; set; }
        public bool IsRequirePhotoAnswer { get; set; }
        public string AnswerPhotoUrl { get; set; }
        public string SurveyorCode { get; set; }
        public string SurveyorJobTitleCode { get; set; }
        public Guid RespondentInfoCode { get; set; }
        public string SurveyResultCode { get; set; }
        public string SurveyResultIndex { get; set; }
        public string VisitId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
