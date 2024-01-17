using Elastic.Apm.Api;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ODTradePromotion.API.Infrastructure.Tp
{
    public class TpPromotion : TpAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string PromotionType { get; set; }
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }
        [Required]
        [MaxLength(100)]
        public string ShortName { get; set; }
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(10)]
        public string Status { get; set; }
        [Required]
        [MaxLength(255)]
        public string Scheme { get; set; }
        [Required]
        public DateTime EffectiveDateFrom { get; set; }
        [Required]
        public DateTime ValidUntil { get; set; }
        [Required]
        [MaxLength(10)]
        public string SaleOrg { get; set; }
        public string TerritoryStructureCode { get; set; }
        [Required]
        [MaxLength(10)]
        public string SicCode { get; set; }
        [DefaultValue(true)]
        public int SettlementFrequency { get; set; }
        [MaxLength(10)]
        public string FrequencyPromotion { get; set; }
        public string ImageName1 { get; set; }
        public string ImagePath1 { get; set; }
        public string ImageFileExt1 { get; set; }
        public string ImageFolderType1 { get; set; }
        public string ImageName2 { get; set; }
        public string ImagePath2 { get; set; }
        public string ImageFileExt2 { get; set; }
        public string ImageFolderType2 { get; set; }
        public string ImageName3 { get; set; }
        public string ImagePath3 { get; set; }
        public string ImageFileExt3 { get; set; }
        public string ImageFolderType3 { get; set; }
        public string ImageName4 { get; set; }
        public string ImagePath4 { get; set; }
        public string ImageFileExt4 { get; set; }
        public string ImageFolderType4 { get; set; }
        [Required]
        [MaxLength(10)]
        public string ScopeType { get; set; }
        public string ScopeSaleTerritoryLevel { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileExt { get; set; }
        public string FolderType { get; set; }
        [Required]
        [MaxLength(10)]
        public string ApplicableObjectType { get; set; }
        [DefaultValue(true)]
        public bool PromotionCheckBy { get; set; }
        [DefaultValue(false)]
        public bool RuleOfGiving { get; set; }
        public bool RuleOfGivingByValue { get; set; }
        public bool IsApplyBudget { get; set; }
        public string UserName { get; set; }
        public string ReasonStep1 { get; set; }
        public string ReasonStep2 { get; set; }
        public string ReasonStep3 { get; set; }
        public string ReasonStep4 { get; set; }
        public string ReasonStep5 { get; set; }
        public bool? IsDonateApplyBudget { get; set; }

        public string DisSicCode { get; set; }
        public string DisSicDesc { get; set; }
        public bool IsFlashSales { get; set; }
        public DateTime? FsValidHour { get; set; }
        public DateTime? FsUntilHour { get; set; }
        /// <summary>
        /// Sẽ được update=true khi data sync từ CP qua OD
        /// </summary>
        public bool IsSync { get; set; } = false;
        /// <summary>
        /// PRINCIPAL/DISTRIBUTOR/SYSTEM
        /// </summary>
        public string OwnerType { get; set; } = null;
        /// <summary>
        /// value = PrincipalCode Nếu Ownertype = PRINCIPAL
        /// value = DistributorCode nếu Ownertype = DISTRIBUTOR
        /// value = null nếu Ownertype = SYSTEM
        /// </summary>
        public string OwnerCode { get; set; } = null;
        /// <summary>
        /// PrincipalCode đã tạo CTKM
        /// Nếu tạo ở CP system: PrincipalCode
        /// Nếu tạo ở OD system: value đã chọn ở màn hình tạo
        /// </summary>
        public string PrincipalCode { get; set; } = null;
    }
}
