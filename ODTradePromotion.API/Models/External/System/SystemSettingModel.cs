using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.External.System
{
    public class SystemSettingModel
    {
        public Guid Id { get; set; }
        public string SettingType { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
        public string Description { get; set; }
    }
}
