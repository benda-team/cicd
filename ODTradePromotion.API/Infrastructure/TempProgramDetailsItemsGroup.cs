using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class TempProgramDetailsItemsGroup
    {
        public Guid Id { get; set; }
        public string ProgramDetailsItemsGroupKey { get; set; }
        public string Description { get; set; }
        public string ProgramDetailsKey { get; set; }
        public string ItemGroupCode { get; set; }
        public Guid ItemGroupId { get; set; }
        public string Uomcode { get; set; }
        public bool IsRequired { get; set; }
        public int FixedQuantities { get; set; }
        public bool IsDeleted { get; set; }
    }
}
