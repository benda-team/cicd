using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpObjectSalesAttributeDiscount : TpAuditableEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }
        [MaxLength(10)]
        public string SalesAttributeCode { get; set; }
        [MaxLength(10)]
        public string SalesAttributeValueCode { set; get; }
    }
}
