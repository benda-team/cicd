using System;
using System.Collections.Generic;

#nullable disable

namespace ODTradePromotion.API.Infrastructure
{
    public partial class UserType
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
    }
}
