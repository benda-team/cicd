using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpAuditableEntity : TpValueObjects
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [MaxLength(256)]
        public string CreatedBy { get; set; }
        [MaxLength(256)]
        public string UpdatedBy { get; set; }
        public int DeleteFlag { get; set; }
    }
}
