using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.External.Item
{
    public class InventoryItemModel
    {
        public Guid Id { get; set; }
        public string InventoryItemId { get; set; }
        public string Status { get; set; }
        public string ShortName { get; set; }
        public string ReportName { get; set; }
        public string Description { get; set; }
        public string ERPCode { get; set; }
        public string DistribiutorCode { get; set; }
    }
}
