using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpScopeDiscountDetail : TpAuditableEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string DiscountCode { get; set; }
        public string ScopeType { get; set; }
        [MaxLength(10)]
        public string SalesTerritoryLevelCode { set; get; }
        [MaxLength(10)]
        public string SalesTerritoryValueCode { set; get; }
    }
}
