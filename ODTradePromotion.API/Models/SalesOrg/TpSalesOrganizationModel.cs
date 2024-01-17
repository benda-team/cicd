using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.SalesOrg
{
    public class TpSalesOrganizationModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string TerritoryStructureCode { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? UntilDate { get; set; }
        public bool IsActive { get; set; }
        public bool? IsSpecificChanel { get; set; }
        public string Channel { get; set; }
        public bool IsSpecificSIC { get; set; }
        public string SIC { get; set; }
    }
    public class TpSalesOrganizationListModel
    {
        public List<TpSalesOrganizationModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }

}
