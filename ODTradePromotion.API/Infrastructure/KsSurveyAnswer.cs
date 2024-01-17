using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class KsSurveyAnswer
    {
        public Guid Id { get; set; }
        public string QuestionId { get; set; }
        public string AnswerValue { get; set; }
        public string RelatedQuestionId { get; set; }
        public bool IsInputAnswer { get; set; }
        public bool IsRequirePhoto { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
