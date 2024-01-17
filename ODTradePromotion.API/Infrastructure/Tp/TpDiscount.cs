using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpDiscount : TpAuditableEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(100)]
        public string ShortName { get; set; }
        [MaxLength(255)]
        public string Scheme { get; set; }
        [Required]
        public DateTime EffectiveDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        [Required]
        [MaxLength(10)]
        public string SaleOrg { get; set; }
        [Required]
        [MaxLength(10)]
        public string SicCode { get; set; }
        [DefaultValue(true)]
        public int SettlementFrequency { get; set; }
        [Required]
        [MaxLength(10)]
        public string DiscountFrequency { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public string FolderType { get; set; }
        public string Reason { get; set; }
        [Required]
        [MaxLength(10)]
        public string Status { get; set; }
        [Required]
        [MaxLength(10)]
        public string ScopeType { get; set; }
        public string ScopeSaleTerritoryLevel { get; set; }
        [Required]
        [MaxLength(10)]
        public string ObjectType { get; set; }
        [Required]
        public int DiscountType { get; set; } = 1;

        public string ReasonStep1 { get; set; }
        public string ReasonStep2 { get; set; }
        public string ReasonStep3 { get; set; }
        public string ReasonStep4 { get; set; }

        [MaxLength(10)]
        public string DisSicCode { get; set; }
        [MaxLength(100)]
        public string OwnerType { get; set; }
        [MaxLength(255)]
        public string OwnerCode { get; set; }
    }
}
