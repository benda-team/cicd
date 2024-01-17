using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class KsSurveyQuestion
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string SurveyId { get; set; }
        public string DependentType { get; set; }
        public string QuestionType { get; set; }
        public string Title { get; set; }
        public int Ordinal { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
