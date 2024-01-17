using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class TempProgramsDetail
    {
        public Guid Id { get; set; }
        public string ProgramDetailsKey { get; set; }
        public string ProgramsCode { get; set; }
        public string Level { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int RequiredQuantities { get; set; }
        public decimal RequiredAmount { get; set; }
        public bool IsDeleted { get; set; }
    }
}
