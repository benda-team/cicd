using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.SalesOrg
{
    public class TpSalesTerritoryValueModel
    {
        public string Key { get; set; }
        public string Code { get; set; }
        public string TerritoryLevelCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public object UntilDate { get; set; }
        public bool IsChecked { get; set; }
        [MaxLength(100)]
        public string OwnerType { get; set; }
        [MaxLength(255)]
        public string OwnerCode { get; set; }
    }

    public class TpSalesTerritoryValueListModel
    {
        public List<TpSalesTerritoryValueModel> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
