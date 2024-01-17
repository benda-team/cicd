using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ODTradePromotion.API.Constants;
using ODTradePromotion.API.Enums;
using ODTradePromotion.API.HttpClients;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Infrastructure.Tp;
using ODTradePromotion.API.Models.Base;
using ODTradePromotion.API.Models.Budget;
using ODTradePromotion.API.Models.Customer;
using ODTradePromotion.API.Models.Discount;
using ODTradePromotion.API.Models.External;
using ODTradePromotion.API.Models.External.Item;
using ODTradePromotion.API.Models.External.Sic;
using ODTradePromotion.API.Models.External.StandardSku;
using ODTradePromotion.API.Models.External.System;
using ODTradePromotion.API.Models.Item;
using ODTradePromotion.API.Models.Promotion;
using ODTradePromotion.API.Models.SalesOrg;
using ODTradePromotion.API.Models.Settlement;
using ODTradePromotion.API.Services.Base;
using ODTradePromotion.API.Services.Principals;
using ODTradePromotion.API.Services.User;
using Sys.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.Promotion
{
    public class PromotionService : IPromotionService
    {
        #region Property
        private readonly ILogger<PromotionService> _logger;
        private readonly IBaseRepository<TpPromotion> _dbPromotion;
        private readonly IBaseRepository<TpPromotionScopeTerritory> _dbPromotionScopeTerritory;
        private readonly IBaseRepository<TpPromotionScopeDsa> _dbPromotionScopeDsa;
        private readonly IBaseRepository<TpPromotionObjectCustomerAttributeLevel> _dbPromotionObjectCustomerAttributeLevel;
        private readonly IBaseRepository<TpPromotionObjectCustomerAttributeValue> _dbTpPromotionObjectCustomerAttributeValues;
        private readonly IBaseRepository<TpPromotionObjectCustomerShipto> _dbTpPromotionObjectCustomerShipto;
        private readonly IBaseRepository<TpPromotionDefinitionStructure> _dbPromotionDefinitionStructure;
        private readonly IBaseRepository<TpPromotionDefinitionProductForSale> _dbPromotionDefinitionProductForSale;
        private readonly IBaseRepository<TpPromotionDefinitionProductForGift> _dbPromotionDefinitionProductForGift;

        private readonly IBaseRepository<Infrastructure.Tp.TpDiscount> _dbDiscount;
        private readonly IBaseRepository<TpDiscountScopeTerritory> _dbDiscountScopeTerritory;
        private readonly IBaseRepository<TpDiscountScopeDsa> _dbDiscountScopeDsa;
        private readonly IBaseRepository<TpDiscountObjectCustomerAttributeValue> _dbDiscountAttributeValue;
        private readonly IBaseRepository<TpDiscountObjectCustomerShipto> _dbDiscountShipto;

        private readonly IBaseRepository<SystemSetting> _dbSystemSetting;
        private readonly IBaseRepository<ScTerritoryValue> _dbTerritoryValue;
        private readonly IBaseRepository<DsaDistributorSellingArea> _dbDsa;
        private readonly IBaseRepository<CustomerSetting> _dbCustomerSetting;
        private readonly IBaseRepository<CustomerAttribute> _dbCustomerAttribute;
        private readonly IBaseRepository<CustomerShipto> _dbCustomerShipto;
        private readonly IBaseRepository<CustomerInformation> _dbCustomerInformation;
        private readonly IBaseRepository<InventoryItem> _dbInventoryItem;
        private readonly IBaseRepository<ItemGroup> _dbItemGroup;
        private readonly IBaseRepository<ItemAttribute> _dbItemAttribute;
        private readonly IBaseRepository<Uom> _dbUom;
        private readonly IBaseRepository<TpBudget> _dbPromotionBudget;
        private readonly IBaseRepository<TpBudgetDefine> _dpTpBudgetDefine;
        private readonly IBaseRepository<TpBudgetAllotment> _dbTpBudgetAllotment;
        private readonly IBaseRepository<TpSettlement> _dpTpSettlement;
        private readonly IBaseRepository<TpSettlementObject> _dpTpSettlementObject;

        private readonly IBaseRepository<ScSalesOrganizationStructure> _dbSaleOrg;
        private readonly IBaseRepository<RzRouteZoneInfomation> _dbRzInfo;
        private readonly IBaseRepository<RzRouteZoneShipto> _dbRzShipto;
        private readonly IBaseRepository<ScTerritoryMapping> _dbScTerritoryMapping;
        private readonly IBaseRepository<Infrastructure.CustomerDmsAttribute> _dbCustomerDsm;
        private readonly IBaseRepository<Standard> _dbStandardSku;
        private readonly IBaseRepository<StandardItem> _dbStandardSkuItem;
        private readonly IBaseRepository<InvAllocationDetail> _dbInvAllocationDetail;
        private readonly IBaseRepository<ItemsUomconversion> _dbItemUom;
        private readonly IBaseRepository<PoRpoparameter> _dbPoRpoparameter;
        private readonly IBaseRepository<TpBudgetUsed> _dbBudgetUsed;
        private readonly IBaseRepository<PrimarySic> _dbPrimarySic;
        private readonly IBaseRepository<ScTerritoryStructureDetail> _dbSalesTerritoryLevel;
        private readonly IBaseRepository<ItemSetting> _dbItemSetting;
        private readonly IMapper _mapper;
        private readonly IUserService userService;
        private readonly IDapperRepositories _dapper;

        #endregion

        #region Constructor
        public PromotionService(ILogger<PromotionService> logger,
            IBaseRepository<TpPromotion> servicePromotion,
            IBaseRepository<TpPromotionScopeTerritory> servicePromotionScopeTerritory,
            IBaseRepository<TpPromotionScopeDsa> servicePromotionScopeDsa,
            IBaseRepository<TpPromotionObjectCustomerAttributeLevel> servicePromotionObjectCustomerAttributeLevel,
            IBaseRepository<TpPromotionObjectCustomerAttributeValue> serviceTpPromotionObjectCustomerAttributeValues,
            IBaseRepository<TpPromotionObjectCustomerShipto> serviceTpPromotionObjectCustomerShipto,
            IBaseRepository<TpPromotionDefinitionStructure> servicePromotionDefinitionStructure,
            IBaseRepository<TpPromotionDefinitionProductForSale> servicePromotionDefinitionProductForSale,
            IBaseRepository<TpPromotionDefinitionProductForGift> servicePromotionDefinitionProductForGift,

            IBaseRepository<Infrastructure.Tp.TpDiscount> serviceDiscount,
            IBaseRepository<TpDiscountScopeTerritory> dbScopeTerritory,
            IBaseRepository<TpDiscountScopeDsa> dbScopeDsa,
            IBaseRepository<TpDiscountObjectCustomerAttributeValue> dbDiscountAttributeValue,
            IBaseRepository<TpDiscountObjectCustomerShipto> dbDiscountShipto,

            IBaseRepository<SystemSetting> systemSettingService,
            IBaseRepository<ScTerritoryValue> serviceTerritoryValue,
            IBaseRepository<DsaDistributorSellingArea> serviceDsa,
            IBaseRepository<CustomerSetting> serviceCustomerSetting,
            IBaseRepository<CustomerAttribute> serviceCustomerAttribute,
            IBaseRepository<CustomerShipto> serviceCustomerShipto,
            IBaseRepository<CustomerInformation> serviceCustomerInformation,
            IBaseRepository<InventoryItem> serviceInventoryItem,
            IBaseRepository<ItemGroup> serviceItemGroup,
            IBaseRepository<ItemAttribute> serviceItemAttribute,
            IBaseRepository<Uom> serviceUom,
            IBaseRepository<TpBudget> serviceBudget,
            IBaseRepository<TpBudgetDefine> serviceBudgetDefine,
            IBaseRepository<TpBudgetAllotment> serviceBudgetAllotment,
            IBaseRepository<TpSettlement> serviceSettlement,
            IBaseRepository<TpSettlementObject> serviceSettlementObject,

            IBaseRepository<ScSalesOrganizationStructure> dbSaleOrg,
            IBaseRepository<RzRouteZoneInfomation> dbRzInfo,
            IBaseRepository<RzRouteZoneShipto> dbRzShipto,
            IBaseRepository<ScTerritoryMapping> dbScTerritoryMapping,
            IBaseRepository<Infrastructure.CustomerDmsAttribute> dbCustomerDsm,
            IBaseRepository<Standard> dbStandardSku,
            IBaseRepository<StandardItem> dbStandardSkuItem,
            IBaseRepository<InvAllocationDetail> dbInvAllocationDetail,
            IBaseRepository<ItemsUomconversion> dbItemUom,
            IBaseRepository<PoRpoparameter> dbPoRpoparameter,
            IBaseRepository<TpBudgetUsed> dbBudgetUsed,
            IBaseRepository<PrimarySic> dbPrimarySic,
            IBaseRepository<ScTerritoryStructureDetail> dbSalesTerritoryLevel,
            IBaseRepository<ItemSetting> dbItemSetting,
            IMapper mapper,
            IUserService userService,
            IDapperRepositories dapper
            )
        {
            _logger = logger;
            _dbPromotion = servicePromotion;

            _dbPromotionScopeTerritory = servicePromotionScopeTerritory;
            _dbPromotionScopeDsa = servicePromotionScopeDsa;
            _dbPromotionObjectCustomerAttributeLevel = servicePromotionObjectCustomerAttributeLevel;
            _dbTpPromotionObjectCustomerAttributeValues = serviceTpPromotionObjectCustomerAttributeValues;
            _dbTpPromotionObjectCustomerShipto = serviceTpPromotionObjectCustomerShipto;
            _dbPromotionDefinitionStructure = servicePromotionDefinitionStructure;
            _dbPromotionDefinitionProductForSale = servicePromotionDefinitionProductForSale;
            _dbPromotionDefinitionProductForGift = servicePromotionDefinitionProductForGift;

            _dbDiscount = serviceDiscount;
            _dbDiscountScopeTerritory = dbScopeTerritory;
            _dbDiscountScopeDsa = dbScopeDsa;
            _dbDiscountAttributeValue = dbDiscountAttributeValue;
            _dbDiscountShipto = dbDiscountShipto;

            _dbSystemSetting = systemSettingService;
            _dbTerritoryValue = serviceTerritoryValue;
            _dbDsa = serviceDsa;
            _dbCustomerSetting = serviceCustomerSetting;
            _dbCustomerAttribute = serviceCustomerAttribute;
            _dbCustomerShipto = serviceCustomerShipto;
            _dbCustomerInformation = serviceCustomerInformation;
            _dbInventoryItem = serviceInventoryItem;
            _dbItemGroup = serviceItemGroup;
            _dbItemAttribute = serviceItemAttribute;
            _dbUom = serviceUom;
            _dbPromotionBudget = serviceBudget;
            _dpTpBudgetDefine = serviceBudgetDefine;
            _dbTpBudgetAllotment = serviceBudgetAllotment;
            _dpTpSettlement = serviceSettlement;
            _dpTpSettlementObject = serviceSettlementObject;

            _dbSaleOrg = dbSaleOrg;
            _dbRzInfo = dbRzInfo;
            _dbRzShipto = dbRzShipto;
            _dbScTerritoryMapping = dbScTerritoryMapping;
            _dbCustomerDsm = dbCustomerDsm;
            _dbStandardSku = dbStandardSku;
            _dbStandardSkuItem = dbStandardSkuItem;
            _dbInvAllocationDetail = dbInvAllocationDetail;
            _dbItemUom = dbItemUom;
            _dbPoRpoparameter = dbPoRpoparameter;
            _dbBudgetUsed = dbBudgetUsed;
            _dbPrimarySic = dbPrimarySic;
            _dbSalesTerritoryLevel = dbSalesTerritoryLevel;
            _dbItemSetting = dbItemSetting;

            _mapper = mapper;
            this.userService = userService;
            _dapper = dapper;
        }
        #endregion

        public IQueryable<PromotionDiscountModel> GetListTpPromotion()
        {
            var systemSettings = _dbSystemSetting.GetAllQueryable(x => x.IsActive).AsNoTracking().AsQueryable();
            string ctkmDescription = systemSettings.FirstOrDefault(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.TpType.ToLower())
            && x.SettingKey.ToLower().Equals(CommonData.PromotionSetting.PromotionProgram.ToLower())).Description;
            string ctckDescription = systemSettings.FirstOrDefault(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.TpType.ToLower())
            && x.SettingKey.ToLower().Equals(CommonData.PromotionSetting.DiscountProgram.ToLower())).Description;

            var resultPromotions = (from p in _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                                    join status in systemSettings.Where(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.PromotionStatus.ToLower()))
                                    .AsNoTracking() on p.Status equals status.SettingKey into emptyStatus
                                    from status in emptyStatus.DefaultIfEmpty()
                                    join scope in systemSettings.Where(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.MktScope.ToLower()))
                                    on p.ScopeType equals scope.SettingKey into emptyScope
                                    from scope in emptyScope.DefaultIfEmpty()

                                    join applicable in systemSettings.Where(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.ApplicableObject.ToLower()))
                                    on p.ApplicableObjectType equals applicable.SettingKey into emptyApplicable
                                    from applicable in emptyApplicable.DefaultIfEmpty()
                                    select new PromotionDiscountModel()
                                    {
                                        ProgramType = CommonData.PromotionSetting.PromotionProgram,
                                        ProgramTypeDescription = ctkmDescription,
                                        Code = p.Code,
                                        ShortName = p.ShortName,
                                        Status = p.Status,
                                        StatusDescription = (status != null) ? status.Description : string.Empty,
                                        EffectiveDateFrom = p.EffectiveDateFrom,
                                        ValidUntil = p.ValidUntil,
                                        ScopeType = p.ScopeType,
                                        ScopeTypeDescription = (scope != null) ? scope.Description : string.Empty,
                                        ApplicableObjectType = p.ApplicableObjectType,
                                        ApplicableObjectTypeDescription = (applicable != null) ? applicable.Description : string.Empty,
                                        IsApplyBudget = p.IsApplyBudget,
                                        IsDonateApplyBudget = p.IsDonateApplyBudget,
                                        UserName = p.UserName,
                                        OwnerType = p.OwnerType,
                                        OwnerCode = p.OwnerCode,
                                        PrincipalCode = p.PrincipalCode
                                    }).AsQueryable();

            var resultDiscounts = (from p in _dbDiscount.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                                   join status in systemSettings.Where(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.PromotionStatus.ToLower()))
                                   on p.Status equals status.SettingKey into emptyStatus
                                   from status in emptyStatus.DefaultIfEmpty()
                                   join scope in systemSettings.Where(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.MktScope.ToLower()))
                                   on p.ScopeType equals scope.SettingKey into emptyScope
                                   from scope in emptyScope.DefaultIfEmpty()
                                   join applicable in systemSettings.Where(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.ApplicableObject.ToLower()))
                                   on p.ObjectType equals applicable.SettingKey into emptyApplicable
                                   from applicable in emptyApplicable.DefaultIfEmpty()
                                   select new PromotionDiscountModel()
                                   {
                                       ProgramType = CommonData.PromotionSetting.DiscountProgram,
                                       ProgramTypeDescription = ctckDescription,
                                       Code = p.Code,
                                       ShortName = p.ShortName,
                                       Status = p.Status,
                                       StatusDescription = (status != null) ? status.Description : string.Empty,
                                       EffectiveDateFrom = p.EffectiveDate,
                                       ValidUntil = p.ValidUntil,
                                       ScopeType = p.ScopeType,
                                       ScopeTypeDescription = (scope != null) ? scope.Description : string.Empty,
                                       ApplicableObjectType = p.ObjectType,
                                       ApplicableObjectTypeDescription = (applicable != null) ? applicable.Description : string.Empty,
                                       IsApplyBudget = false,
                                       IsDonateApplyBudget = false,
                                       UserName = string.Empty,
                                       OwnerType = "",
                                       OwnerCode = "",
                                       PrincipalCode = ""
                                   }).AsQueryable();
            return resultPromotions.Union(resultDiscounts);
        }

        public async Task<List<PromotionDiscountModel>> GetListTpPromotionDapper(PromotionDiscountSearchModel parameters)
        {
            string sqlQuery = @$"select * from 
                                (select tp.""Id"", '01' as ""ProgramType"", ss4.""Description"" as ""ProgramTypeDescription"", tp.""Code"", tp.""ShortName"", 
                                tp.""Status"", ss.""Description"" as ""StatusDescription"", tp.""EffectiveDateFrom"" as ""EffectiveDateFrom"", tp.""ValidUntil"" as ""ValidUntil"", 
                                tp.""ScopeType"" as ""ScopeType"", ss2.""Description"" as ""ScopeTypeDescription"", 
                                tp.""ApplicableObjectType"" as ""ApplicableObjectType"", ss3.""Description"" as ""ApplicableObjectTypeDescription"", 
                                tp.""IsApplyBudget"" as ""IsApplyBudget"", tp.""UserName"" as ""UserName"", tp.""IsDonateApplyBudget"" as ""IsDonateApplyBudget"",
                                tp.""OwnerType"" as ""OwnerType"", tp.""OwnerCode"" as ""OwnerCode"", tp.""PrincipalCode"" as ""PrincipalCode"" 
                                from ""TpPromotions"" tp 
                                join ""SystemSettings"" ss on tp.""Status""  = ss.""SettingKey""
                                join ""SystemSettings"" ss2 on tp.""ScopeType""  = ss2.""SettingKey"" 
                                join ""SystemSettings"" ss3 on tp.""ApplicableObjectType""  = ss3.""SettingKey"" 
                                join ""SystemSettings"" ss4 on '01'  = ss4.""SettingKey"" 
                                where ss.""SettingType""  = @status and ss.""IsActive"" 
                                and ss2.""SettingType""  = @scope and ss2.""IsActive""
                                and ss3.""SettingType""  = @object and ss3.""IsActive""
                                and ss4.""SettingType""  = @programType and ss4.""IsActive""

                                union all 

                                select td.""Id"", '02' as ""ProgramType"", ss4.""Description"" as ""ProgramTypeDescription"", td.""Code"", td.""ShortName"", 
                                td.""Status"",ss.""Description"" as ""StatusDescription"", td.""EffectiveDate"" as ""EffectiveDateFrom"", td.""ValidUntil"" as ""ValidUntil"", 
                                td.""ScopeType"" as ""ScopeType"", ss2.""Description"" as ""ScopeTypeDescription"", 
                                td.""ObjectType"" as ""ApplicableObjectType"", ss3.""Description"" as ""ApplicableObjectTypeDescription"", 
                                false as ""IsApplyBudget"", '' as ""UserName"", false as ""IsDonateApplyBudget"",
                                '' as ""OwnerType"", '' as ""OwnerCode"",'' as ""PrincipalCode""
                                from ""TpDiscounts""  td  
                                join ""SystemSettings"" ss on td.""Status""  = ss.""SettingKey""
                                join ""SystemSettings"" ss2 on td.""ScopeType""  = ss2.""SettingKey"" 
                                join ""SystemSettings"" ss3 on td.""ObjectType""  = ss3.""SettingKey"" 
                                join ""SystemSettings"" ss4 on '02'  = ss4.""SettingKey"" 
                                where ss.""SettingType""  = @status and ss.""IsActive"" 
                                and ss2.""SettingType""  = @scope and ss2.""IsActive""
                                and ss3.""SettingType""  = @object and ss3.""IsActive""
                                and ss4.""SettingType""  = @programType and ss4.""IsActive""
                                ) as dt
                                where dt.""Status"" = @StatusValue";

            DynamicParameters dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(parameters.KeySearch))
            {
                sqlQuery = sqlQuery + @$" and (lower(dt.""ProgramTypeDescription"") like @KeySearch 
                                                or lower(dt.""Code"") like @KeySearch 
                                                or lower(dt.""ShortName"") like @KeySearch 
                                                or lower(dt.""StatusDescription"") like @KeySearch 
                                                or lower(dt.""ScopeTypeDescription"") like @KeySearch 
                                                or lower(dt.""ApplicableObjectTypeDescription"") like @KeySearch)";

                dynamicParameters.Add("KeySearch", $"%{parameters.KeySearch.ToLower()}%");
            }

            if (!string.IsNullOrEmpty(parameters.ProgramType))
            {
                sqlQuery = sqlQuery + @$" and dt.""ProgramType"" = @ProgramValue";
            }

            if (!string.IsNullOrEmpty(parameters.Code))
            {
                sqlQuery = sqlQuery + @$" and lower(dt.""Code"") like @CodeValue";
                dynamicParameters.Add("CodeValue", $"%{parameters.Code.ToLower()}%");
            }

            if (!string.IsNullOrEmpty(parameters.ShortName))
            {
                sqlQuery = sqlQuery + @$" and lower(dt.""ShortName"") like @ShortNameValue";
                dynamicParameters.Add("ShortNameValue", $"%{parameters.ShortName.ToLower()}%");
            }

            if (parameters.EffectiveDate != null)
            {
                sqlQuery = sqlQuery + @$" and dt.""EffectiveDateFrom""  >= @EffectiveValue";
            }

            if (parameters.ValidUntil != null)
            {
                sqlQuery = sqlQuery + @$" and dt.""ValidUntil""  <= @ValidUntilValue";
            }

            if (!string.IsNullOrEmpty(parameters.Scope))
            {
                sqlQuery = sqlQuery + @$" and dt.""ScopeType"" = @ScopeValue";
            }

            if (!string.IsNullOrEmpty(parameters.Applicable))
            {
                sqlQuery = sqlQuery + @$" and dt.""ApplicableObjectType"" = @ApplicableValue";
            }

            if (parameters.ApplyBudget != null)
            {
                sqlQuery = sqlQuery + @$" and dt.""IsApplyBudget"" = @IsBudgetValue";
            }
            if (!string.IsNullOrEmpty(parameters.PrincipalCode))
            {
                sqlQuery = sqlQuery + @$" and dt.""PrincipalCode"" = @PrincipalCode";
            }

            if (string.IsNullOrEmpty(parameters.Orderby))
            {
                sqlQuery = sqlQuery + @$" order by dt.""Code"" asc";
            }
            else
            {
                sqlQuery = sqlQuery + @$" order by dt.""{parameters.Orderby}"" {parameters.OrderbyType}";
            }

            dynamicParameters.Add("status", CommonData.SystemSetting.PromotionStatus);
            dynamicParameters.Add("scope", CommonData.SystemSetting.MktScope);
            dynamicParameters.Add("object", CommonData.SystemSetting.ApplicableObject);
            dynamicParameters.Add("programType", CommonData.SystemSetting.TpType);
            dynamicParameters.Add("StatusValue", parameters.Status);
            dynamicParameters.Add("ProgramValue", parameters.ProgramType);
            dynamicParameters.Add("EffectiveValue", parameters.EffectiveDate);
            dynamicParameters.Add("ValidUntilValue", parameters.ValidUntil);
            dynamicParameters.Add("ScopeValue", parameters.Scope);
            dynamicParameters.Add("ApplicableValue", parameters.Applicable);
            dynamicParameters.Add("IsBudgetValue", parameters.ApplyBudget);
            dynamicParameters.Add("PrincipalCode", parameters.PrincipalCode);

            var data = await _dapper.QueryWithParams<PromotionDiscountModel>(sqlQuery, dynamicParameters);

            return data.ToList();
        }

        public IQueryable<TpPromotionSearchModel> GetListPromotionCode(string status)
        {
            if (status.Equals(CommonData.PromotionSetting.Confirmed))
            {
                var ressultPromotionListView = _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0
                && x.Status.Equals(CommonData.PromotionSetting.Confirmed)).AsNoTracking()
                .Select(x => new TpPromotionSearchModel { Code = x.Code, Name = x.ShortName })
                .OrderBy(x => x.Name)
                .AsQueryable();
                var ressultDiscountListView = _dbDiscount.GetAllQueryable(x => x.DeleteFlag == 0
                && x.Status.Equals(CommonData.PromotionSetting.Confirmed)).AsNoTracking()
                    .Select(x => new TpPromotionSearchModel { Code = x.Code, Name = x.ShortName })
                    .OrderBy(x => x.Name)
                    .AsQueryable();

                return ressultPromotionListView.Union(ressultDiscountListView);
            }
            else if (status.Equals(CommonData.PromotionSetting.WaitConfirm))
            {
                var ressultPromotionListConfirm = _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0
                && x.Status.Equals(CommonData.PromotionSetting.WaitConfirm)).AsNoTracking()
                .Select(x => new TpPromotionSearchModel { Code = x.Code, Name = x.ShortName })
                .OrderBy(x => x.Name)
                .AsQueryable();
                var ressultDiscountListConfirm = _dbDiscount.GetAllQueryable(x => x.DeleteFlag == 0
                && x.Status.Equals(CommonData.PromotionSetting.WaitConfirm)).AsNoTracking()
                    .Select(x => new TpPromotionSearchModel { Code = x.Code, Name = x.ShortName })
                    .OrderBy(x => x.Name)
                    .AsQueryable();

                return ressultPromotionListConfirm.Union(ressultDiscountListConfirm);
            }

            IQueryable<TpPromotionSearchModel> result = null;
            return result;
        }

        public async Task<List<TpPromotionSearchModel>> GetListPromotionCodeDapper(string status)
        {
            string sqlQuery = $@"select * from 
                        (select tp.""Code"", tp.""ShortName"" as ""Name"", tp.""Status"" from ""TpPromotions"" tp 
                        union all 
                        select td.""Code"", td.""ShortName"" as ""Name"", td.""Status""
                        from ""TpDiscounts""  td  
                        ) as dt
                        where dt.""Status"" = '{status}'";

            var data = await _dapper.Query<TpPromotionSearchModel>(sqlQuery);

            return data.ToList();
        }
        public IQueryable<PromotionPopupModel> GetListPromotionPopup(string userName, string promotionType)
        {
            var results = (from p in _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                           join status in _dbSystemSetting.GetAllQueryable(x => x.IsActive && x.SettingType.ToLower().Equals(CommonData.SystemSetting.PromotionStatus.ToLower())).AsNoTracking()
                           on p.Status equals status.SettingKey into emptyStatus
                           from status in emptyStatus.DefaultIfEmpty()
                           select new { p, status });
            if (!string.IsNullOrEmpty(userName))
            {
                results = results.Where(x => x.p.UserName.ToLower().Equals(userName.ToLower()) && x.p.PromotionType == promotionType);
            }
            return results.Select(x => new PromotionPopupModel()
            {
                Code = x.p.Code,
                ShortName = x.p.ShortName,
                Status = x.p.Status,
                StatusDescription = (x.status != null) ? x.status.Description : string.Empty
            });
        }

        public async Task<BaseResultModel> CreatePromotion(TpPromotionModel input, string userLogin, bool isSync = false)
        {
            var userInfo = userService.GetUserInfoByUserName(userLogin);
            try
            {
                // Create promotion
                var promotion = _mapper.Map<TpPromotion>(input);
                promotion.Id = Guid.NewGuid();
                promotion.CreatedBy = userLogin;
                promotion.CreatedDate = DateTime.Now;
                promotion.DeleteFlag = 0;
                // Check budget
                foreach (var item in input.ListDefinitionStructure)
                {
                    if (item.IsApplyBudget)
                    {
                        promotion.IsApplyBudget = true;
                    }
                    if (item.IsDonateApplyBudget)
                    {
                        promotion.IsDonateApplyBudget = true;
                    }
                }             

                promotion.OwnerType = OwnerTypeConst.SYSTEM;
                if (isSync)
                {
                    promotion.OwnerType = OwnerTypeConst.PRINCIPAL;
                    // promotion.OwnerCode = input.OwnerCode;
                    // promotion.PrincipalCode = input.Code;
                }
                else
                {
                    // User is Distributor
                    if (userInfo.IsDistributorUser)
                    {
                        promotion.OwnerType = OwnerTypeConst.DISTRIBUTOR;
                        promotion.OwnerCode = await userService.GetDistributorCodeOfLoginUserAsync(userLogin);
                        //promotion.PrincipalCode = input.PrincipalCode;
                    }
                    else
                    {
                        //promotion.OwnerType = OwnerTypeConst.SYSTEM;
                        promotion.OwnerCode = null;
                        //promotion.PrincipalCode = input.PrincipalCode;
                    }
                }

                _dbPromotion.Insert(promotion);

                // Scope
                if (promotion.ScopeType.ToLower().Equals(CommonData.PromotionSetting.ScopeSalesTerritoryLevel.ToLower()))
                {
                    List<TpPromotionScopeTerritory> listScope = new List<TpPromotionScopeTerritory>();
                    if (input.ListScopeSalesTerritory != null && input.ListScopeSalesTerritory.Count > 0)
                    {
                        foreach (var item in input.ListScopeSalesTerritory)
                        {
                            listScope.Add(new TpPromotionScopeTerritory()
                            {
                                Id = Guid.NewGuid(),
                                PromotionCode = promotion.Code,
                                SaleOrg = promotion.SaleOrg,
                                ScopeSaleTerritoryLevel = promotion.ScopeSaleTerritoryLevel,
                                SalesTerritoryValue = item.Code,
                                CreatedBy = userLogin,
                                CreatedDate = DateTime.Now,
                                DeleteFlag = 0,
                                OwnerType = promotion.OwnerType,
                                OwnerCode = promotion.OwnerCode
                            });
                        }
                        _dbPromotionScopeTerritory.InsertRange(listScope);
                    }
                }
                else if (promotion.ScopeType.ToLower().Equals(CommonData.PromotionSetting.ScopeDSA.ToLower()))
                {
                    List<TpPromotionScopeDsa> listScope = new List<TpPromotionScopeDsa>();
                    if (input.ListScopeDSA != null && input.ListScopeDSA.Count > 0)
                    {
                        foreach (var item in input.ListScopeDSA)
                        {
                            listScope.Add(new TpPromotionScopeDsa()
                            {
                                Id = Guid.NewGuid(),
                                PromotionCode = promotion.Code,
                                SaleOrg = promotion.SaleOrg,
                                ScopeDsaValue = item.Code,
                                CreatedBy = userLogin,
                                CreatedDate = DateTime.Now,
                                DeleteFlag = 0,
                                OwnerType = promotion.OwnerType,
                                OwnerCode = promotion.OwnerCode
                            });
                        }
                        _dbPromotionScopeDsa.InsertRange(listScope);
                    }
                }

                // Applicable Object
                if (promotion.ApplicableObjectType.ToLower().Equals(CommonData.PromotionSetting.ObjectCustomerAttributes.ToLower()))
                {
                    // Insert Customer Setting
                    List<TpPromotionObjectCustomerAttributeLevel> listCustomerSetting = new List<TpPromotionObjectCustomerAttributeLevel>();
                    if (input.ListCustomerSetting != null && input.ListCustomerSetting.Count > 0)
                    {
                        foreach (var item in input.ListCustomerSetting)
                        {
                            listCustomerSetting.Add(new TpPromotionObjectCustomerAttributeLevel()
                            {
                                Id = Guid.NewGuid(),
                                PromotionCode = promotion.Code,
                                CustomerAttributerLevel = item.AttributeID,
                                IsApply = item.IsChecked,
                                CreatedBy = userLogin,
                                CreatedDate = DateTime.Now,
                                DeleteFlag = 0,
                                OwnerType = promotion.OwnerType,
                                OwnerCode = promotion.OwnerCode
                            });
                        }
                        _dbPromotionObjectCustomerAttributeLevel.InsertRange(listCustomerSetting);
                    }

                    // Insert Customer Attribute
                    List<TpPromotionObjectCustomerAttributeValue> listCustomerAttribute = new List<TpPromotionObjectCustomerAttributeValue>();
                    if (input.ListCustomerAttribute != null && input.ListCustomerAttribute.Count > 0)
                    {
                        foreach (var item in input.ListCustomerAttribute)
                        {
                            listCustomerAttribute.Add(new TpPromotionObjectCustomerAttributeValue()
                            {
                                Id = Guid.NewGuid(),
                                PromotionCode = promotion.Code,
                                CustomerAttributerLevel = item.AttributeMaster,
                                CustomerAttributerValue = item.Code,
                                CreatedBy = userLogin,
                                CreatedDate = DateTime.Now,
                                DeleteFlag = 0,
                                OwnerType = promotion.OwnerType,
                                OwnerCode = promotion.OwnerCode
                            });
                        }
                        _dbTpPromotionObjectCustomerAttributeValues.InsertRange(listCustomerAttribute);
                    }
                }
                else if (promotion.ApplicableObjectType.ToLower().Equals(CommonData.PromotionSetting.ObjectCustomerShipto.ToLower()))
                {
                    // Insert Customer Shipto
                    List<TpPromotionObjectCustomerShipto> listCustomerShipto = new List<TpPromotionObjectCustomerShipto>();
                    if (input.ListCustomerShipto != null && input.ListCustomerShipto.Count > 0)
                    {
                        foreach (var item in input.ListCustomerShipto)
                        {
                            listCustomerShipto.Add(new TpPromotionObjectCustomerShipto()
                            {
                                Id = Guid.NewGuid(),
                                PromotionCode = promotion.Code,
                                CustomerCode = item.CustomerCode,
                                CustomerShiptoCode = item.ShiptoCode,
                                CreatedBy = userLogin,
                                CreatedDate = DateTime.Now,
                                DeleteFlag = 0,
                                OwnerType = promotion.OwnerType,
                                OwnerCode = promotion.OwnerCode
                            });
                        }
                        _dbTpPromotionObjectCustomerShipto.InsertRange(listCustomerShipto);
                    }
                }

                // Definition Structure
                // Insert Customer Shipto
                List<TpPromotionDefinitionStructure> listPromotionStructure = new List<TpPromotionDefinitionStructure>();
                List<TpPromotionDefinitionProductForSale> listProductForSale = new List<TpPromotionDefinitionProductForSale>();
                List<TpPromotionDefinitionProductForGift> listProductForGift = new List<TpPromotionDefinitionProductForGift>();
                if (input.ListDefinitionStructure != null && input.ListDefinitionStructure.Count > 0)
                {
                    foreach (var item in input.ListDefinitionStructure)
                    {
                        // Create Definition Structure
                        var structure = _mapper.Map<TpPromotionDefinitionStructure>(item);
                        structure.Id = Guid.NewGuid();
                        structure.CreatedBy = userLogin;
                        structure.CreatedDate = DateTime.Now;
                        structure.DeleteFlag = 0;
                        structure.OwnerType = promotion.OwnerType;
                        structure.OwnerCode = promotion.OwnerCode;
                        if (structure.IsApplyBudget == true)
                        {
                            structure.IsGiftApplyBudget = structure.IsApplyBudget;
                        }
                        listPromotionStructure.Add(structure);

                        // Create Product For Sale
                        if (item.ListProductForSales != null && item.ListProductForSales.Count > 0)
                        {
                            foreach (var productSale in item.ListProductForSales)
                            {
                                listProductForSale.Add(new TpPromotionDefinitionProductForSale()
                                {
                                    Id = Guid.NewGuid(),
                                    PromotionCode = promotion.Code,
                                    LevelCode = productSale.LevelCode,
                                    ProductTypeForSale = item.ProductTypeForSale,
                                    ItemHierarchyLevelForSale = item.ItemHierarchyLevelForSale,
                                    ProductCode = productSale.ProductCode,
                                    Packing = productSale.Packing,
                                    SellNumber = productSale.SellNumber,
                                    CreatedBy = userLogin,
                                    CreatedDate = DateTime.Now,
                                    DeleteFlag = 0,
                                    OwnerType = promotion.OwnerType,
                                    OwnerCode = promotion.OwnerCode
                                });
                            }
                        }

                        // Create Product For Gift
                        if (item.ListProductForGifts != null && item.ListProductForGifts.Count > 0)
                        {
                            foreach (var productGift in item.ListProductForGifts)
                            {
                                listProductForGift.Add(new TpPromotionDefinitionProductForGift()
                                {
                                    Id = Guid.NewGuid(),
                                    PromotionCode = promotion.Code,
                                    LevelCode = productGift.LevelCode,
                                    ProductTypeForGift = item.ProductTypeForGift,
                                    ItemHierarchyLevelForGift = item.ItemHierarchyLevelForGift,
                                    ProductCode = productGift.ProductCode,
                                    Packing = productGift.Packing,
                                    NumberOfGift = productGift.NumberOfGift,
                                    BudgetCode = productGift.BudgetCode,
                                    IsDefaultProduct = productGift.IsDefaultProduct,
                                    Exchange = productGift.Exchange,
                                    CreatedBy = userLogin,
                                    CreatedDate = DateTime.Now,
                                    DeleteFlag = 0,
                                    OwnerType = promotion.OwnerType,
                                    OwnerCode = promotion.OwnerCode
                                });
                            }
                        }
                    }
                    _dbPromotionDefinitionStructure.InsertRange(listPromotionStructure);
                    _dbPromotionDefinitionProductForSale.InsertRange(listProductForSale);
                    _dbPromotionDefinitionProductForGift.InsertRange(listProductForGift);
                }

                return new BaseResultModel
                {
                    IsSuccess = true,
                    Code = 201,
                    Message = "CreateSuccess"
                };
            }
            catch (Exception ex)
            {
                return new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 500,
                    Message = ex.InnerException?.Message ?? ex.Message
                };
            }
        }

        private List<TpPromotionScopeTerritory> GetScopeTerritorys(List<TpSalesTerritoryValueModel> ListScopeSalesTerritory, string promotionCode, string OrgCode, string ScopeSaleTerritoryLevel, string userLogin)
        {
            List<TpPromotionScopeTerritory> listScope = new List<TpPromotionScopeTerritory>();
            if (ListScopeSalesTerritory != null && ListScopeSalesTerritory.Count > 0)
            {
                foreach (var item in ListScopeSalesTerritory)
                {
                    listScope.Add(new TpPromotionScopeTerritory()
                    {
                        Id = Guid.NewGuid(),
                        PromotionCode = promotionCode,
                        SaleOrg = OrgCode,
                        ScopeSaleTerritoryLevel = ScopeSaleTerritoryLevel,
                        SalesTerritoryValue = item.Code,
                        CreatedBy = userLogin,
                        CreatedDate = DateTime.Now,
                        DeleteFlag = 0
                    });
                }
            }
            return listScope;
        }

        private List<TpPromotionScopeDsa> GetScopeDsas(List<TpSalesOrgDsaModel> ListScopeDSA, string promotionCode, string SaleOrg, string userLogin)
        {
            List<TpPromotionScopeDsa> listScope = new List<TpPromotionScopeDsa>();
            if (ListScopeDSA != null && ListScopeDSA.Count > 0)
            {
                foreach (var item in ListScopeDSA)
                {
                    listScope.Add(new TpPromotionScopeDsa()
                    {
                        Id = Guid.NewGuid(),
                        PromotionCode = promotionCode,
                        SaleOrg = SaleOrg,
                        ScopeDsaValue = item.Code,
                        CreatedBy = userLogin,
                        CreatedDate = DateTime.Now,
                        DeleteFlag = 0
                    });
                }
            }
            return listScope;
        }

        private List<TpPromotionObjectCustomerAttributeLevel> GetObjectCustomerAttributeLevels(List<CustomerSettingModel> ListCustomerSetting, string promotionCode, string userLogin)
        {
            List<TpPromotionObjectCustomerAttributeLevel> listCustomerSetting = new List<TpPromotionObjectCustomerAttributeLevel>();
            if (ListCustomerSetting != null && ListCustomerSetting.Count > 0)
            {
                foreach (var item in ListCustomerSetting)
                {
                    listCustomerSetting.Add(new TpPromotionObjectCustomerAttributeLevel()
                    {
                        Id = Guid.NewGuid(),
                        PromotionCode = promotionCode,
                        CustomerAttributerLevel = item.AttributeID,
                        IsApply = item.IsChecked,
                        CreatedBy = userLogin,
                        CreatedDate = DateTime.Now,
                        DeleteFlag = 0
                    });
                }
            }
            return listCustomerSetting;
        }

        private List<TpPromotionObjectCustomerAttributeValue> GetObjectCustomerAttributeValues(List<CustomerAttributeModel> ListCustomerAttribute, string promotionCode, string userLogin)
        {
            List<TpPromotionObjectCustomerAttributeValue> listCustomerAttribute = new List<TpPromotionObjectCustomerAttributeValue>();
            if (ListCustomerAttribute != null && ListCustomerAttribute.Count > 0)
            {
                foreach (var item in ListCustomerAttribute)
                {
                    listCustomerAttribute.Add(new TpPromotionObjectCustomerAttributeValue()
                    {
                        Id = Guid.NewGuid(),
                        PromotionCode = promotionCode,
                        CustomerAttributerLevel = item.AttributeMaster,
                        CustomerAttributerValue = item.Code,
                        CreatedBy = userLogin,
                        CreatedDate = DateTime.Now,
                        DeleteFlag = 0
                    });
                }
            }
            return listCustomerAttribute;
        }

        private List<TpPromotionObjectCustomerShipto> GetObjectCustomerShipto(List<CustomerShiptoModel> ListCustomerShipto, string promotionCode, string userLogin)
        {
            List<TpPromotionObjectCustomerShipto> listCustomerShipto = new List<TpPromotionObjectCustomerShipto>();
            if (ListCustomerShipto != null && ListCustomerShipto.Count > 0)
            {
                foreach (var item in ListCustomerShipto)
                {
                    listCustomerShipto.Add(new TpPromotionObjectCustomerShipto()
                    {
                        Id = Guid.NewGuid(),
                        PromotionCode = promotionCode,
                        CustomerCode = item.CustomerCode,
                        CustomerShiptoCode = item.ShiptoCode,
                        CreatedBy = userLogin,
                        CreatedDate = DateTime.Now,
                        DeleteFlag = 0
                    });
                }
            }
            return listCustomerShipto;
        }

        public bool DeletePromotionByCode(string code, string userLogin)
        {
            var promotion = _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0 && x.Code.ToLower().Equals(code.ToLower())).FirstOrDefault();
            // delete in Promotion
            promotion.DeleteFlag = 1;
            promotion.UpdatedBy = userLogin;
            promotion.UpdatedDate = DateTime.Now;
            _dbPromotion.Update(promotion);

            // Delete Scope
            var scopeTerritories = _dbPromotionScopeTerritory.GetAllQueryable(x => x.DeleteFlag == 0 && x.PromotionCode.ToLower().Equals(code.ToLower())).ToList();
            if (scopeTerritories != null && scopeTerritories.Count > 0)
            {
                foreach (var item in scopeTerritories)
                {
                    item.DeleteFlag = 1;
                    item.UpdatedBy = userLogin;
                    item.UpdatedDate = DateTime.Now;
                }
                _dbPromotionScopeTerritory.UpdateRange(scopeTerritories);
            }

            var scopeDsas = _dbPromotionScopeDsa.GetAllQueryable(x => x.DeleteFlag == 0 && x.PromotionCode.ToLower().Equals(code.ToLower())).ToList();
            if (scopeDsas != null && scopeDsas.Count > 0)
            {
                foreach (var item in scopeDsas)
                {
                    item.DeleteFlag = 1;
                    item.UpdatedBy = userLogin;
                    item.UpdatedDate = DateTime.Now;
                }
                _dbPromotionScopeDsa.UpdateRange(scopeDsas);
            }

            // Applicable Object
            var customersettings = _dbPromotionObjectCustomerAttributeLevel.GetAllQueryable(x => x.DeleteFlag == 0 && x.PromotionCode.ToLower().Equals(code.ToLower())).ToList();
            if (customersettings != null && customersettings.Count > 0)
            {
                foreach (var item in customersettings)
                {
                    item.DeleteFlag = 1;
                    item.UpdatedBy = userLogin;
                    item.UpdatedDate = DateTime.Now;
                }
                _dbPromotionObjectCustomerAttributeLevel.UpdateRange(customersettings);
            }

            var customerattribute = _dbTpPromotionObjectCustomerAttributeValues.GetAllQueryable(x => x.DeleteFlag == 0 && x.PromotionCode.ToLower().Equals(code.ToLower())).ToList();
            if (customerattribute != null && customerattribute.Count > 0)
            {
                foreach (var item in customerattribute)
                {
                    item.DeleteFlag = 1;
                    item.UpdatedBy = userLogin;
                    item.UpdatedDate = DateTime.Now;
                }
                _dbTpPromotionObjectCustomerAttributeValues.UpdateRange(customerattribute);
            }

            var customershipto = _dbTpPromotionObjectCustomerShipto.GetAllQueryable(x => x.DeleteFlag == 0 && x.PromotionCode.ToLower().Equals(code.ToLower())).ToList();
            if (customershipto != null && customershipto.Count > 0)
            {
                foreach (var item in customershipto)
                {
                    item.DeleteFlag = 1;
                    item.UpdatedBy = userLogin;
                    item.UpdatedDate = DateTime.Now;
                }
                _dbTpPromotionObjectCustomerShipto.UpdateRange(customershipto);
            }

            // Definition Level
            var defineLevels = _dbPromotionDefinitionStructure.GetAllQueryable(x => x.DeleteFlag == 0 && x.PromotionCode.ToLower().Equals(code.ToLower())).ToList();
            if (defineLevels != null && defineLevels.Count > 0)
            {
                foreach (var item in defineLevels)
                {
                    item.DeleteFlag = 1;
                    item.UpdatedBy = userLogin;
                    item.UpdatedDate = DateTime.Now;
                }
                _dbPromotionDefinitionStructure.UpdateRange(defineLevels);
            }

            var productForSales = _dbPromotionDefinitionProductForSale.GetAllQueryable(x => x.DeleteFlag == 0 && x.PromotionCode.ToLower().Equals(code.ToLower())).ToList();
            if (productForSales != null && productForSales.Count > 0)
            {
                foreach (var item in productForSales)
                {
                    item.DeleteFlag = 1;
                    item.UpdatedBy = userLogin;
                    item.UpdatedDate = DateTime.Now;
                }
                _dbPromotionDefinitionProductForSale.UpdateRange(productForSales);
            }

            var productForGifts = _dbPromotionDefinitionProductForGift.GetAllQueryable(x => x.DeleteFlag == 0 && x.PromotionCode.ToLower().Equals(code.ToLower())).ToList();
            if (productForGifts != null && productForGifts.Count > 0)
            {
                foreach (var item in productForGifts)
                {
                    item.DeleteFlag = 1;
                    item.UpdatedBy = userLogin;
                    item.UpdatedDate = DateTime.Now;
                }
                _dbPromotionDefinitionProductForGift.UpdateRange(productForGifts);
            }

            return true;
        }

        public TpPromotionModel GetDetailPromotionByCode(string code)
        {
            DateTime now = DateTime.Now;
            var promotionData = _dbPromotion.FirstOrDefault(x => x.Code.ToLower().Equals(code.ToLower()));
            if (promotionData != null) promotionData.IsDonateApplyBudget = promotionData.IsDonateApplyBudget == null ? false : promotionData.IsDonateApplyBudget;
            var promotion = _mapper.Map<TpPromotionModel>(promotionData);
            if (promotion != null)
            {
                // get List Scope
                if (promotion.ScopeType.Equals(CommonData.PromotionSetting.ScopeSalesTerritoryLevel))
                {
                    var listScopeSales = (from scope in _dbPromotionScopeTerritory
                                          .GetAllQueryable(x => x.DeleteFlag == 0 && x.PromotionCode.ToLower().Equals(code.ToLower())).AsNoTracking()
                                          join territory in _dbTerritoryValue
                                          .GetAllQueryable(x => x.TerritoryLevelCode.ToLower().Equals(promotion.ScopeSaleTerritoryLevel.ToLower())).AsNoTracking()
                                          on scope.SalesTerritoryValue equals territory.Code into emptyScope
                                          from territory in emptyScope.DefaultIfEmpty()
                                          select new TpSalesTerritoryValueModel()
                                          {
                                              Code = scope.SalesTerritoryValue,
                                              TerritoryLevelCode = promotion.ScopeSaleTerritoryLevel,
                                              Description = (territory != null) ? territory.Description : string.Empty
                                          }).AsNoTracking().ToList();
                    promotion.ListScopeSalesTerritory = new List<TpSalesTerritoryValueModel>();
                    promotion.ListScopeSalesTerritory = listScopeSales;
                }
                else if (promotion.ScopeType.Equals(CommonData.PromotionSetting.ScopeDSA))
                {
                    var listScopeSales = (from scope in _dbPromotionScopeDsa
                                          .GetAllQueryable(x => x.DeleteFlag == 0 && x.PromotionCode.ToLower().Equals(code.ToLower())).AsNoTracking()
                                          join dsa in _dbDsa.GetAllQueryable(x => x.EffectiveDate <= now && (!x.UntilDate.HasValue || x.UntilDate.Value >= now)).AsNoTracking()
                                          on scope.ScopeDsaValue equals dsa.Code into emptyScope
                                          from dsa in emptyScope.DefaultIfEmpty()
                                          select new TpSalesOrgDsaModel()
                                          {
                                              Code = scope.ScopeDsaValue,
                                              Description = (dsa != null) ? dsa.Description : string.Empty
                                          }).AsNoTracking().ToList();
                    promotion.ListScopeDSA = new List<TpSalesOrgDsaModel>();
                    promotion.ListScopeDSA = listScopeSales;
                }

                if (promotion.ApplicableObjectType.Equals(CommonData.PromotionSetting.ObjectCustomerAttributes))
                {
                    var listCustomerSetting = (from cussetting in _dbPromotionObjectCustomerAttributeLevel
                                               .GetAllQueryable(x => x.DeleteFlag == 0 && x.PromotionCode.ToLower().Equals(code.ToLower())).AsNoTracking()
                                               join csetting in _dbCustomerSetting.GetAllQueryable().AsNoTracking()
                                               on cussetting.CustomerAttributerLevel equals csetting.AttributeId into emptyCustomerSetting
                                               from csetting in emptyCustomerSetting.DefaultIfEmpty()
                                               orderby cussetting.CustomerAttributerLevel
                                               select new CustomerSettingModel()
                                               {
                                                   Id = cussetting.Id,
                                                   AttributeID = cussetting.CustomerAttributerLevel,
                                                   AttributeName = (csetting != null) ? csetting.AttributeName : string.Empty,
                                                   Description = (csetting != null) ? csetting.Description : string.Empty,
                                                   IsCustomerAttribute = csetting.IsCustomerAttribute,
                                                   IsChecked = cussetting.IsApply,
                                                   CustomerSettingId = csetting.Id
                                               }).AsNoTracking().ToList();
                    promotion.ListCustomerSetting = new List<CustomerSettingModel>();
                    promotion.ListCustomerSetting = listCustomerSetting;

                    List<CustomerAttributeModel> listCustomerAttributeModel = new List<CustomerAttributeModel>();
                    var lstCustomerAttributes = _dbCustomerAttribute.GetAllQueryable(x => x.EffectiveDate <= now && (!x.ValidUntil.HasValue || x.ValidUntil.Value >= now)).AsNoTracking().AsQueryable();
                    var lstCustomerSettingApply = listCustomerSetting.Where(x => x.IsChecked).ToList();
                    foreach (var item in lstCustomerSettingApply)
                    {
                        var tempListCustomerAttributes = lstCustomerAttributes.Where(x => x.CustomerSettingId == item.CustomerSettingId).AsNoTracking().AsQueryable();

                        var listCustomerValue = (from customervalue in _dbTpPromotionObjectCustomerAttributeValues
                                               .GetAllQueryable(x => x.DeleteFlag == 0
                                               && x.PromotionCode.ToLower().Equals(code.ToLower())
                                               && x.CustomerAttributerLevel.ToLower().Equals(item.AttributeID.ToLower())).AsNoTracking()
                                                 join customerattribute in tempListCustomerAttributes
                                                 on customervalue.CustomerAttributerValue.ToLower() equals customerattribute.Code.ToLower() into emptyCustomervalue
                                                 from customerattribute in emptyCustomervalue.DefaultIfEmpty()
                                                 select new CustomerAttributeModel()
                                                 {
                                                     Id = customervalue.Id,
                                                     AttributeMaster = customervalue.CustomerAttributerLevel,
                                                     Code = customervalue.CustomerAttributerValue,
                                                     Description = (customerattribute != null) ? customerattribute.Description : string.Empty,
                                                 }).AsNoTracking().ToList();

                        listCustomerAttributeModel.AddRange(listCustomerValue);
                    }
                    promotion.ListCustomerAttribute = new List<CustomerAttributeModel>();
                    promotion.ListCustomerAttribute = listCustomerAttributeModel;
                }
                else if (promotion.ApplicableObjectType.Equals(CommonData.PromotionSetting.ObjectCustomerShipto))
                {
                    // Get List Applicable Object
                    var listCustomerShipto = (from customershipto in _dbCustomerShipto.GetAllQueryable().AsNoTracking()
                                              join customer in _dbCustomerInformation.GetAllQueryable().AsNoTracking()
                                              on customershipto.CustomerInfomationId equals customer.Id into emptyCustomershipto
                                              from customer in emptyCustomershipto.DefaultIfEmpty()
                                              select new CustomerShiptoModel()
                                              {
                                                  CustomerCode = customer.CustomerCode,
                                                  ShiptoCode = customershipto.ShiptoCode,
                                                  Address = customershipto.Address
                                              }).AsNoTracking().AsQueryable();

                    var lstDataCustomerShipto = (from data in _dbTpPromotionObjectCustomerShipto
                                          .GetAllQueryable(x => x.PromotionCode.ToLower().Equals(code.ToLower())).AsNoTracking()
                                                 join customershipto in listCustomerShipto on
                                                 new { customer_code = data.CustomerCode, customer_shipto_code = data.CustomerShiptoCode } equals
                                                 new { customer_code = customershipto.CustomerCode, customer_shipto_code = customershipto.ShiptoCode }
                                                 into temptyCustomerShipto
                                                 from customershipto in temptyCustomerShipto.DefaultIfEmpty()
                                                 select new CustomerShiptoModel()
                                                 {
                                                     CustomerCode = data.CustomerCode,
                                                     ShiptoCode = data.CustomerShiptoCode,
                                                     Address = (customershipto != null) ? customershipto.Address : string.Empty
                                                 }).AsNoTracking().ToList();
                    promotion.ListCustomerShipto = new List<CustomerShiptoModel>();
                    promotion.ListCustomerShipto = lstDataCustomerShipto;
                }

                var lstLevel = _dbPromotionDefinitionStructure
                                              .GetAllQueryable(x => x.PromotionCode.ToLower().Equals(code.ToLower())).AsNoTracking().ToList();


                var lstDefinitionStructure = _mapper.Map<List<TpPromotionDefinitionStructureModel>>(lstLevel);

                var lstSKU = _dbInventoryItem.GetAllQueryable(x => x.DelFlg == 0).AsNoTracking().ToList();
                var lstItemGroup = _dbItemGroup.GetAllQueryable().AsNoTracking().ToList();
                var lstItemHierarchy = _dbItemAttribute
                    .GetAllQueryable(x => x.DeleteFlag == 0 && x.EffectiveDate <= now && (!x.ValidUntilDate.HasValue || x.ValidUntilDate.Value >= now)).AsNoTracking().ToList();
                var lstUom = _dbUom.GetAllQueryable(x => x.DeleteFlag == 0
                && x.EffectiveDateFrom <= now && (!x.ValidUntil.HasValue || x.ValidUntil.Value >= now)).AsNoTracking().ToList();
                var lstBudget = _dbPromotionBudget.GetAllQueryable(x => x.DeleteFlag == 0
               && (x.Status.Equals(CommonData.PromotionSetting.StatusCanLinkPromotion) || x.Status.Equals(CommonData.PromotionSetting.StatusLinkedPromotion))).AsNoTracking().ToList();
                // Get Product For Sale
                var lstAllProductForSaleByPromotion = _dbPromotionDefinitionProductForSale
                                        .GetAllQueryable(x => x.DeleteFlag == 0 && x.PromotionCode.ToLower().Equals(code.ToLower())).AsNoTracking().ToList();

                foreach (var item in lstDefinitionStructure)
                {

                    if (item.IsGiftApplyBudget)
                    {
                        item.BudgetCodeForGift = item.GiftApplyBudgetCode;
                        item.BudgetTypeOfGift = "01";
                        item.BudgetAllocationLevelOfGift = lstBudget.Where(o => o.Code.Equals(item.BudgetCodeForGift)).FirstOrDefault().BudgetAllocationLevel;
                    }
                    if (item.IsDonateApplyBudget)
                    {
                        item.BudgetCodeForDonate = item.DonateApplyBudgetCode;
                        item.BudgetTypeOfDonate = "02";
                        item.BudgetAllocationLevelOfDonate = lstBudget.Where(o => o.Code.Equals(item.BudgetCodeForDonate)).FirstOrDefault().BudgetAllocationLevel;
                    }
                    item.Allowance = item.IsDonateAllowance;
                    if (item.ProductTypeForSale.ToLower().Equals(CommonData.PromotionSetting.SKU))
                    {
                        var lstProduct = (from data in lstAllProductForSaleByPromotion
                                          join sku in lstSKU on data.ProductCode equals sku.InventoryItemId into emptySku
                                          from sku in emptySku.DefaultIfEmpty()
                                          join uom in lstUom on data.Packing equals uom.UomId into emptyUom
                                          from uom in emptyUom.DefaultIfEmpty()
                                          where data.LevelCode.ToLower().Equals(item.LevelCode.ToLower())
                                          select new TpPromotionDefinitionProductForSaleModel()
                                          {
                                              Id = data.Id,
                                              PromotionCode = code,
                                              LevelCode = item.LevelCode,
                                              ProductCode = data.ProductCode,
                                              ProductDescription = (sku != null) ? sku.Description : string.Empty,
                                              Packing = data.Packing,
                                              PackingDescription = (uom != null) ? uom.Description : string.Empty,
                                              SellNumber = data.SellNumber
                                          }).ToList();

                        item.ListProductForSales.AddRange(lstProduct);
                    }
                    else if (item.ProductTypeForSale.ToLower().Equals(CommonData.PromotionSetting.ItemGroup))
                    {
                        var lstProduct = (from data in lstAllProductForSaleByPromotion
                                          join ig in lstItemGroup on data.ProductCode equals ig.Code into emptyIg
                                          from ig in emptyIg.DefaultIfEmpty()
                                          join uom in lstUom on data.Packing equals uom.UomId into emptyUom
                                          from uom in emptyUom.DefaultIfEmpty()
                                          where data.LevelCode.ToLower().Equals(item.LevelCode.ToLower())
                                          select new TpPromotionDefinitionProductForSaleModel()
                                          {
                                              Id = data.Id,
                                              PromotionCode = code,
                                              LevelCode = item.LevelCode,
                                              ProductCode = data.ProductCode,
                                              ProductDescription = (ig != null) ? ig.Description : string.Empty,
                                              Packing = data.Packing,
                                              PackingDescription = (uom != null) ? uom.Description : string.Empty,
                                              SellNumber = data.SellNumber
                                          }).ToList();

                        item.ListProductForSales.AddRange(lstProduct);
                    }
                    else
                    {
                        var lstProduct = (from data in lstAllProductForSaleByPromotion
                                          join ih in lstItemHierarchy on data.ProductCode equals ih.ItemAttributeCode into emptyIg
                                          from ih in emptyIg.DefaultIfEmpty()
                                          join uom in lstUom on data.Packing equals uom.UomId into emptyUom
                                          from uom in emptyUom.DefaultIfEmpty()
                                          where item.ItemHierarchyLevelForSale.Equals(ih.ItemAttributeMaster) && data.LevelCode.ToLower().Equals(item.LevelCode.ToLower())
                                          select new TpPromotionDefinitionProductForSaleModel()
                                          {
                                              Id = data.Id,
                                              PromotionCode = code,
                                              LevelCode = item.LevelCode,
                                              ProductCode = data.ProductCode,
                                              ProductDescription = (ih != null) ? ih.Description : string.Empty,
                                              Packing = data.Packing,
                                              PackingDescription = (uom != null) ? uom.Description : string.Empty,
                                              SellNumber = data.SellNumber
                                          }).ToList();

                        item.ListProductForSales.AddRange(lstProduct);
                    }
                }

                // Get Product For Gift
                var lstAllProductForGiftByPromotion = _dbPromotionDefinitionProductForGift
                                        .GetAllQueryable(x => x.DeleteFlag == 0 && x.PromotionCode.ToLower().Equals(code.ToLower())).AsNoTracking().ToList();
                foreach (var item in lstDefinitionStructure)
                {
                    if (item.ProductTypeForGift.ToLower().Equals(CommonData.PromotionSetting.SKU))
                    {
                        var lstProduct = (from data in lstAllProductForGiftByPromotion
                                          join sku in lstSKU on data.ProductCode equals sku.InventoryItemId into emptySku
                                          from sku in emptySku.DefaultIfEmpty()
                                          join uom in lstUom on data.Packing equals uom.UomId into emptyUom
                                          from uom in emptyUom.DefaultIfEmpty()
                                          join bud in lstBudget on data.BudgetCode equals bud.Code into emptyBudget
                                          from bud in emptyBudget.DefaultIfEmpty()
                                          where data.LevelCode.ToLower().Equals(item.LevelCode.ToLower())
                                          select new TpPromotionDefinitionProductForGiftModel()
                                          {
                                              Id = data.Id,
                                              PromotionCode = code,
                                              LevelCode = item.LevelCode,
                                              ProductCode = data.ProductCode,
                                              ProductDescription = (sku != null) ? sku.Description : string.Empty,
                                              Packing = data.Packing,
                                              PackingDescription = (uom != null) ? uom.Description : string.Empty,
                                              NumberOfGift = data.NumberOfGift,
                                              BudgetCode = data.BudgetCode,
                                              BudgetName = (bud != null) ? bud.Name : string.Empty,
                                              IsDefaultProduct = data.IsDefaultProduct,
                                              Exchange = data.Exchange
                                          }).ToList();

                        item.ListProductForGifts.AddRange(lstProduct);
                    }
                    else if (item.ProductTypeForGift.ToLower().Equals(CommonData.PromotionSetting.ItemGroup))
                    {
                        var lstProduct = (from data in lstAllProductForGiftByPromotion
                                          join ig in lstItemGroup on data.ProductCode equals ig.Code into emptyIg
                                          from ig in emptyIg.DefaultIfEmpty()
                                          join uom in lstUom on data.Packing equals uom.UomId into emptyUom
                                          from uom in emptyUom.DefaultIfEmpty()
                                          join bud in lstBudget on data.BudgetCode equals bud.Code into emptyBudget
                                          from bud in emptyBudget.DefaultIfEmpty()
                                          where data.LevelCode.ToLower().Equals(item.LevelCode.ToLower())
                                          select new TpPromotionDefinitionProductForGiftModel()
                                          {
                                              Id = data.Id,
                                              PromotionCode = code,
                                              LevelCode = item.LevelCode,
                                              ProductCode = data.ProductCode,
                                              ProductDescription = (ig != null) ? ig.Description : string.Empty,
                                              Packing = data.Packing,
                                              PackingDescription = (uom != null) ? uom.Description : string.Empty,
                                              NumberOfGift = data.NumberOfGift,
                                              BudgetCode = data.BudgetCode,
                                              BudgetName = (bud != null) ? bud.Name : string.Empty,
                                              IsDefaultProduct = data.IsDefaultProduct,
                                              Exchange = data.Exchange
                                          }).ToList();

                        item.ListProductForGifts.AddRange(lstProduct);
                    }
                    else
                    {
                        var lstProduct = (from data in lstAllProductForGiftByPromotion
                                          join ih in lstItemHierarchy on data.ProductCode equals ih.ItemAttributeCode into emptyIg
                                          from ih in emptyIg.DefaultIfEmpty()
                                          join uom in lstUom on data.Packing equals uom.UomId into emptyUom
                                          from uom in emptyUom.DefaultIfEmpty()
                                          join bud in lstBudget on data.BudgetCode equals bud.Code into emptyBudget
                                          from bud in emptyBudget.DefaultIfEmpty()
                                          where item.ItemHierarchyLevelForGift.Equals(ih.ItemAttributeMaster) && data.LevelCode.ToLower().Equals(item.LevelCode.ToLower())
                                          select new TpPromotionDefinitionProductForGiftModel()
                                          {
                                              Id = data.Id,
                                              PromotionCode = code,
                                              LevelCode = item.LevelCode,
                                              ProductCode = data.ProductCode,
                                              ProductDescription = (ih != null) ? ih.Description : string.Empty,
                                              Packing = data.Packing,
                                              PackingDescription = (uom != null) ? uom.Description : string.Empty,
                                              NumberOfGift = data.NumberOfGift,
                                              BudgetCode = data.BudgetCode,
                                              BudgetName = (bud != null) ? bud.Name : string.Empty,
                                              IsDefaultProduct = data.IsDefaultProduct,
                                              Exchange = data.Exchange,
                                          }).ToList();
                        item.ListProductForGifts.AddRange(lstProduct);
                    }
                }
                promotion.ListDefinitionStructure.AddRange(lstDefinitionStructure);
            }
            return promotion;
        }

        public async Task<TpPromotionModel> GetDetailPromotionByCodeDapper(string code)
        {
            TpPromotionModel promotion = new TpPromotionModel();
            var sqlMultipe = new StringBuilder();

            // promotion
            var sqlMain = @$"select * from ""TpPromotions"" tp where ""Code""  = '{code}';";
            promotion = await _dapper.QuerySingle<TpPromotionModel>(sqlMain);

            // Promotion Territory Scope
            if (promotion.ScopeType == CommonData.PromotionSetting.ScopeSalesTerritoryLevel)
            {
                sqlMultipe.AppendLine(@$"select tpst.""SalesTerritoryValue"" as ""Code"", tpst.""ScopeSaleTerritoryLevel"" as ""TerritoryLevelCode"", stv.""Description"" from ""TpPromotionScopeTerritorys"" tpst
                                    join ""SC_TerritoryValues"" stv on tpst.""SalesTerritoryValue"" = stv.""Code""
                                    where tpst.""PromotionCode""  = '{code}' and tpst.""DeleteFlag""  = 0;");
            }

            // Promotion Dsa Scope
            if (promotion.ScopeType == CommonData.PromotionSetting.ScopeDSA)
            {
                sqlMultipe.AppendLine(@$"select tpsd.""ScopeDsaValue"" as ""Code"", ddsa.""Description"" from ""TpPromotionScopeDsas"" tpsd 
                                        join ""DSA_DistributorSellingAreas"" ddsa on tpsd.""ScopeDsaValue"" = ddsa.""Code"" 
                                        where tpsd.""PromotionCode"" = '{code}' and tpsd.""DeleteFlag"" = 0;
                                        ");
            }

            // Promotion Customer Attribute
            if (promotion.ApplicableObjectType == CommonData.PromotionSetting.ObjectCustomerAttributes)
            {
                sqlMultipe.AppendLine(@$"select tpocal.""Id"", tpocal.""CustomerAttributerLevel"" as ""AttributeID"", cs.""AttributeName"", cs.""Description"", cs.""IsCustomerAttribute"", tpocal.""IsApply"" as ""IsChecked"", cs.""Id"" as ""CustomerSettingId"" 
                                    from ""TpPromotionObjectCustomerAttributeLevels"" tpocal 
                                    join ""CustomerSettings"" cs on tpocal.""CustomerAttributerLevel"" = cs.""AttributeID"" 
                                    where tpocal.""PromotionCode"" = '{code}' and tpocal.""DeleteFlag"" = 0;");

                sqlMultipe.AppendLine(@$"select tpocav.""Id"", tpocav.""CustomerAttributerLevel"" as ""AttributeMaster"", tpocav.""CustomerAttributerValue"" as ""Code"", ca.""Description"" from ""TpPromotionObjectCustomerAttributeValues"" tpocav 
                                        join ""CustomerSettings"" cs on tpocav.""CustomerAttributerLevel"" = cs.""AttributeID"" 
                                        join ""CustomerAttributes"" ca on (tpocav.""CustomerAttributerValue"" = ca.""Code"" and cs.""Id"" = ca.""CustomerSettingId"" )
                                        where tpocav.""PromotionCode"" = '{code}' and tpocav.""DeleteFlag"" = 0;");
            }

            // Promotion Customer Shipto
            if (promotion.ApplicableObjectType == CommonData.PromotionSetting.ObjectCustomerShipto)
            {
                sqlMultipe.AppendLine(@$"select tpocs.""CustomerCode"", tpocs.""CustomerShiptoCode"" as ""ShiptoCode"", cs.""Address"" from ""TpPromotionObjectCustomerShiptos"" tpocs 
                                        join ""CustomerInformations"" ci on tpocs.""CustomerCode"" = ci.""CustomerCode"" 
                                        join ""CustomerShiptos"" cs on (ci.""Id"" = cs.""CustomerInfomationId"" and tpocs.""CustomerShiptoCode"" = cs.""ShiptoCode"")
                                        where tpocs.""PromotionCode"" = '{code}' and tpocs.""DeleteFlag"" = 0;");
            }

            // Promotion Definition Structures
            sqlMultipe.AppendLine(@$"select * from ""TpPromotionDefinitionStructures"" tpds 
                                    where tpds.""PromotionCode"" = '{code}' and tpds.""DeleteFlag"" = 0;");


            // Promotion Product for sales
            sqlMultipe.AppendLine(@$"select tpdpfs.*, ii.""Description"", u.""Description"" from ""TpPromotionDefinitionProductForSales"" tpdpfs
                                    join ""InventoryItems"" ii on tpdpfs.""ProductCode"" = ii.""InventoryItemId"" 
                                    join ""Uoms"" u on tpdpfs.""Packing"" = u.""UomId"" 
                                    where tpdpfs.""PromotionCode"" = '{code}' and tpdpfs.""DeleteFlag"" = 0;

                                    select tpdpfs.*, ig.""Description"", u.""Description"" from ""TpPromotionDefinitionProductForSales"" tpdpfs
                                    join ""ItemGroups"" ig on tpdpfs.""ProductCode"" = ig.""Code""  
                                    join ""Uoms"" u on tpdpfs.""Packing"" = u.""UomId"" 
                                    where tpdpfs.""PromotionCode"" = '{code}' and tpdpfs.""DeleteFlag"" = 0;

                                    select tpdpfs.*, ia.""Description"", u.""Description"" from ""TpPromotionDefinitionProductForSales"" tpdpfs
                                    join ""ItemAttributes"" ia on (tpdpfs.""ItemHierarchyLevelForSale"" = ia.""ItemAttributeMaster"" and  tpdpfs.""ProductCode"" = ia.""ItemAttributeCode"" )
                                    join ""Uoms"" u on tpdpfs.""Packing"" = u.""UomId"" 
                                    where tpdpfs.""PromotionCode"" = '{code}' and tpdpfs.""DeleteFlag"" = 0;");

            // Promotion Product for gifts
            // Promotion Product for sales
            sqlMultipe.AppendLine(@$"select tpdpfg.*, ii.""Description"", u.""Description"" from ""TpPromotionDefinitionProductForGifts"" tpdpfg 
                                join ""InventoryItems"" ii on tpdpfg.""ProductCode"" = ii.""InventoryItemId"" 
                                join ""Uoms"" u on tpdpfg.""Packing"" = u.""UomId"" 
                                join ""TpBudgets"" tb on tpdpfg.""BudgetCode"" = tb.""Code"" 
                                where tpdpfg.""PromotionCode"" = '{code}' and tpdpfg.""DeleteFlag"" = 0;

                                select tpdpfg.*, ig.""Description"", u.""Description"" from ""TpPromotionDefinitionProductForGifts"" tpdpfg 
                                join ""ItemGroups"" ig on tpdpfg.""ProductCode"" = ig.""Code""  
                                join ""Uoms"" u on tpdpfg.""Packing"" = u.""UomId"" 
                                join ""TpBudgets"" tb on tpdpfg.""BudgetCode"" = tb.""Code"" 
                                where tpdpfg.""PromotionCode"" = '{code}' and tpdpfg.""DeleteFlag"" = 0;

                                select tpdpfg.*, ia.""Description"", u.""Description"" from ""TpPromotionDefinitionProductForGifts"" tpdpfg
                                join ""ItemAttributes"" ia on (tpdpfg.""ItemHierarchyLevelForGift"" = ia.""ItemAttributeMaster"" and  tpdpfg.""ProductCode"" = ia.""ItemAttributeCode"" )
                                join ""Uoms"" u on tpdpfg.""Packing"" = u.""UomId"" 
                                join ""TpBudgets"" tb on tpdpfg.""BudgetCode"" = tb.""Code"" 
                                where tpdpfg.""PromotionCode"" = '{code}' and tpdpfg.""DeleteFlag"" = 0;");

            var tempResult = await _dapper.QueryMultiple(sqlMultipe.ToString());

            if (promotion.ScopeType == CommonData.PromotionSetting.ScopeSalesTerritoryLevel)
            {
                promotion.ListScopeSalesTerritory = tempResult.Read<TpSalesTerritoryValueModel>().ToList();
            }

            if (promotion.ScopeType == CommonData.PromotionSetting.ScopeDSA)
            {
                promotion.ListScopeDSA = tempResult.Read<TpSalesOrgDsaModel>().ToList();
            }

            if (promotion.ApplicableObjectType == CommonData.PromotionSetting.ObjectCustomerAttributes)
            {
                promotion.ListCustomerSetting = tempResult.Read<CustomerSettingModel>().ToList();
                promotion.ListCustomerAttribute = tempResult.Read<CustomerAttributeModel>().ToList();
            }

            if (promotion.ApplicableObjectType == CommonData.PromotionSetting.ObjectCustomerShipto)
            {
                promotion.ListCustomerShipto = tempResult.Read<CustomerShiptoModel>().ToList();
            }

            promotion.ListDefinitionStructure = tempResult.Read<TpPromotionDefinitionStructureModel>().ToList();


            if (promotion.ListDefinitionStructure != null && promotion.ListDefinitionStructure.Count > 0)
            {
                var lstSumProductForSale1 = tempResult.Read<TpPromotionDefinitionProductForSaleModel>().ToList();
                var lstSumProductForSale2 = tempResult.Read<TpPromotionDefinitionProductForSaleModel>().ToList();
                var lstSumProductForSale3 = tempResult.Read<TpPromotionDefinitionProductForSaleModel>().ToList();
                var lstSumProductForSale = new List<TpPromotionDefinitionProductForSaleModel>();
                lstSumProductForSale.AddRange(lstSumProductForSale1);
                lstSumProductForSale.AddRange(lstSumProductForSale2);
                lstSumProductForSale.AddRange(lstSumProductForSale3);

                var lstSumProductForGift1 = tempResult.Read<TpPromotionDefinitionProductForGiftModel>().ToList();
                var lstSumProductForGift2 = tempResult.Read<TpPromotionDefinitionProductForGiftModel>().ToList();
                var lstSumProductForGift3 = tempResult.Read<TpPromotionDefinitionProductForGiftModel>().ToList();
                var lstSumProductForGift = new List<TpPromotionDefinitionProductForGiftModel>();
                lstSumProductForGift.AddRange(lstSumProductForGift1);
                lstSumProductForGift.AddRange(lstSumProductForGift2);
                lstSumProductForGift.AddRange(lstSumProductForGift3);

                foreach (var item in promotion.ListDefinitionStructure)
                {
                    item.ListProductForSales = lstSumProductForSale.Where(x => x.LevelCode == item.LevelCode).ToList();
                    item.ListProductForGifts = lstSumProductForGift.Where(x => x.LevelCode == item.LevelCode).ToList();
                }
            }

            return promotion;
        }
        public async Task UpdateSyncedFlagAsync(string promotionCode, string userLogin)
        {
            var promotion = _dbPromotion.FirstOrDefault(x => x.DeleteFlag == 0 && x.Code.ToLower().Equals(promotionCode.ToLower()));
            if (promotion == default) return;
            promotion.IsSync = true;
            promotion.UpdatedDate = DateTime.Now;
            promotion.UpdatedBy = userLogin;
            _dbPromotion.Update(promotion);
        }

        public bool UpdatePromotion(TpPromotionModel input, string userLogin)
        {
            var promotion = _dbPromotion.FirstOrDefault(x => x.DeleteFlag == 0 && x.Code.ToLower().Equals(input.Code.ToLower()));
            if (promotion != null)
            {
                var promotionId = promotion.Id;
                var username = promotion.UserName;
                var createDate = promotion.CreatedDate;
                var createBy = promotion.CreatedBy;
                promotion = _mapper.Map<TpPromotion>(input);
                promotion.Id = promotionId;
                promotion.UserName = username;
                promotion.CreatedDate = createDate;
                promotion.CreatedBy = createBy;
                promotion.UpdatedDate = DateTime.Now;
                promotion.UpdatedBy = userLogin;
                // Check budget
                foreach (var item in input.ListDefinitionStructure)
                {
                    if (item.IsApplyBudget)
                    {
                        promotion.IsApplyBudget = true;
                    }
                    if (item.IsDonateApplyBudget)
                    {
                        promotion.IsDonateApplyBudget = true;
                    }
                }
                _dbPromotion.Update(promotion);

                // Scope
                var scopeSalesTerritorys = _dbPromotionScopeTerritory.GetAllQueryable(x => x.PromotionCode.ToLower().Equals(promotion.Code.ToLower()));
                _dbPromotionScopeTerritory.DeleteRange(scopeSalesTerritorys);

                var scopeDsas = _dbPromotionScopeDsa.GetAllQueryable(x => x.PromotionCode.ToLower().Equals(promotion.Code.ToLower()));
                _dbPromotionScopeDsa.DeleteRange(scopeDsas);

                if (promotion.ScopeType.ToLower().Equals(CommonData.PromotionSetting.ScopeSalesTerritoryLevel.ToLower()))
                {
                    if (input.ListScopeSalesTerritory != null && input.ListScopeSalesTerritory.Count > 0)
                    {
                        var listScope = GetScopeTerritorys(input.ListScopeSalesTerritory, promotion.Code, promotion.SaleOrg, promotion.ScopeSaleTerritoryLevel, userLogin);
                        _dbPromotionScopeTerritory.InsertRange(listScope);
                    }
                }
                else if (promotion.ScopeType.ToLower().Equals(CommonData.PromotionSetting.ScopeDSA.ToLower()))
                {
                    if (input.ListScopeDSA != null && input.ListScopeDSA.Count > 0)
                    {
                        var listScope = GetScopeDsas(input.ListScopeDSA, promotion.Code, promotion.SaleOrg, userLogin);
                        _dbPromotionScopeDsa.InsertRange(listScope);
                    }
                }

                // Applicable Object
                var objectCusSettings = _dbPromotionObjectCustomerAttributeLevel.GetAllQueryable(x => x.PromotionCode.ToLower().Equals(promotion.Code.ToLower()));
                _dbPromotionObjectCustomerAttributeLevel.DeleteRange(objectCusSettings);

                var objectCusAttributes = _dbTpPromotionObjectCustomerAttributeValues.GetAllQueryable(x => x.PromotionCode.ToLower().Equals(promotion.Code.ToLower()));
                _dbTpPromotionObjectCustomerAttributeValues.DeleteRange(objectCusAttributes);

                var objectCustomerShipto = _dbTpPromotionObjectCustomerShipto.GetAllQueryable(x => x.PromotionCode.ToLower().Equals(promotion.Code.ToLower()));
                _dbTpPromotionObjectCustomerShipto.DeleteRange(objectCustomerShipto);

                if (promotion.ApplicableObjectType.ToLower().Equals(CommonData.PromotionSetting.ObjectCustomerAttributes.ToLower()))
                {
                    // Insert Customer Setting
                    if (input.ListCustomerSetting != null && input.ListCustomerSetting.Count > 0)
                    {
                        var listCustomerSetting = GetObjectCustomerAttributeLevels(input.ListCustomerSetting, promotion.Code, userLogin);
                        _dbPromotionObjectCustomerAttributeLevel.InsertRange(listCustomerSetting);
                    }

                    // Insert Customer Attribute
                    if (input.ListCustomerAttribute != null && input.ListCustomerAttribute.Count > 0)
                    {
                        var listCustomerAttribute = GetObjectCustomerAttributeValues(input.ListCustomerAttribute, promotion.Code, userLogin);
                        _dbTpPromotionObjectCustomerAttributeValues.InsertRange(listCustomerAttribute);
                    }
                }
                else if (promotion.ApplicableObjectType.ToLower().Equals(CommonData.PromotionSetting.ObjectCustomerShipto.ToLower()))
                {
                    // Insert Customer Shipto
                    if (input.ListCustomerShipto != null && input.ListCustomerShipto.Count > 0)
                    {
                        var listCustomerShipto = GetObjectCustomerShipto(input.ListCustomerShipto, promotion.Code, userLogin);
                        _dbTpPromotionObjectCustomerShipto.InsertRange(listCustomerShipto);
                    }
                }

                // Definition Structure
                var structures = _dbPromotionDefinitionStructure.GetAllQueryable(x => x.PromotionCode.ToLower().Equals(promotion.Code.ToLower()));
                _dbPromotionDefinitionStructure.DeleteRange(structures);

                var productForSales = _dbPromotionDefinitionProductForSale.GetAllQueryable(x => x.PromotionCode.ToLower().Equals(promotion.Code.ToLower()));
                _dbPromotionDefinitionProductForSale.DeleteRange(productForSales);

                var productForGift = _dbPromotionDefinitionProductForGift.GetAllQueryable(x => x.PromotionCode.ToLower().Equals(promotion.Code.ToLower()));
                _dbPromotionDefinitionProductForGift.DeleteRange(productForGift);

                List<TpPromotionDefinitionStructure> listPromotionStructure = new List<TpPromotionDefinitionStructure>();
                List<TpPromotionDefinitionProductForSale> listProductForSale = new List<TpPromotionDefinitionProductForSale>();
                List<TpPromotionDefinitionProductForGift> listProductForGift = new List<TpPromotionDefinitionProductForGift>();
                if (input.ListDefinitionStructure != null && input.ListDefinitionStructure.Count > 0)
                {
                    foreach (var item in input.ListDefinitionStructure)
                    {
                        // Create Definition Structure
                        var structure = _mapper.Map<TpPromotionDefinitionStructure>(item);
                        structure.Id = Guid.NewGuid();
                        structure.CreatedBy = userLogin;
                        structure.CreatedDate = DateTime.Now;
                        structure.DeleteFlag = 0;
                        if (structure.IsApplyBudget == true)
                        {
                            structure.IsGiftApplyBudget = structure.IsApplyBudget;
                        }
                        listPromotionStructure.Add(structure);

                        // Create Product For Sale
                        if (item.ListProductForSales != null && item.ListProductForSales.Count > 0)
                        {
                            foreach (var productSale in item.ListProductForSales)
                            {
                                listProductForSale.Add(new TpPromotionDefinitionProductForSale()
                                {
                                    Id = Guid.NewGuid(),
                                    PromotionCode = promotion.Code,
                                    LevelCode = productSale.LevelCode,
                                    ProductTypeForSale = item.ProductTypeForSale,
                                    ItemHierarchyLevelForSale = item.ItemHierarchyLevelForSale,
                                    ProductCode = productSale.ProductCode,
                                    Packing = productSale.Packing,
                                    SellNumber = productSale.SellNumber,
                                    CreatedBy = userLogin,
                                    CreatedDate = DateTime.Now,
                                    DeleteFlag = 0
                                });
                            }
                        }

                        // Create Product For Gift
                        if (item.ListProductForGifts != null && item.ListProductForGifts.Count > 0)
                        {
                            foreach (var productGift in item.ListProductForGifts)
                            {
                                listProductForGift.Add(new TpPromotionDefinitionProductForGift()
                                {
                                    Id = Guid.NewGuid(),
                                    PromotionCode = promotion.Code,
                                    LevelCode = productGift.LevelCode,
                                    ProductTypeForGift = item.ProductTypeForGift,
                                    ItemHierarchyLevelForGift = item.ItemHierarchyLevelForGift,
                                    ProductCode = productGift.ProductCode,
                                    Packing = productGift.Packing,
                                    NumberOfGift = productGift.NumberOfGift,
                                    BudgetCode = productGift.BudgetCode,
                                    IsDefaultProduct = productGift.IsDefaultProduct,
                                    Exchange = productGift.Exchange,
                                    CreatedBy = userLogin,
                                    CreatedDate = DateTime.Now,
                                    DeleteFlag = 0
                                });
                            }
                        }
                    }
                    _dbPromotionDefinitionStructure.InsertRange(listPromotionStructure);
                    _dbPromotionDefinitionProductForSale.InsertRange(listProductForSale);
                    _dbPromotionDefinitionProductForGift.InsertRange(listProductForGift);
                }

                // Update Status Budget
                var budgets = (from bud in _dbPromotionBudget.GetAllQueryable(x => x.Status.Equals(CommonData.PromotionSetting.StatusCanLinkPromotion) && x.DeleteFlag == 0).ToList()
                               join pfg in listProductForGift on bud.Code equals pfg.BudgetCode
                               select bud).ToList();
                budgets.ForEach(x => x.Status = CommonData.PromotionSetting.StatusLinkedPromotion);
                _dbPromotionBudget.UpdateRange(budgets);
            }
            return true;
        }

        public bool ConfirmPromotion(ConfirmPromotionReq request)
        {
            var promotion = _dbPromotion.GetAllQueryable(x => x.Code == request.Code && x.DeleteFlag == 0).FirstOrDefault();
            promotion.Status = request.Status;
            promotion.UserName = request.UserName;
            promotion.UpdatedDate = DateTime.Now;
            promotion.UpdatedBy = request.UserName;

            var result = _dbPromotion.Update(promotion);
            if (result != null)
            {
                return true;
            }
            return false;
        }

        public TpPromotionModel GetGeneralPromotionByCode(string code)
        {
            return _mapper.Map<TpPromotionModel>(_dbPromotion.FirstOrDefault(x => x.Code.ToLower().Equals(code.ToLower())));
        }

        public IQueryable<TpPromotionGeneralModel> GetListPromotionGeneral()
        {
            var systemSettings = _dbSystemSetting.GetAllQueryable(x => x.IsActive).AsNoTracking().AsQueryable();

            var results = (from p in _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                           join status in systemSettings.Where(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.PromotionStatus.ToLower()))
                           on p.Status equals status.SettingKey into emptyStatus
                           from status in emptyStatus.DefaultIfEmpty()
                           join scope in systemSettings.Where(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.MktScope.ToLower()))
                           on p.ScopeType equals scope.SettingKey into emptyScope
                           from scope in emptyScope.DefaultIfEmpty()
                           join applicable in systemSettings.Where(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.ApplicableObject.ToLower()))
                           on p.ApplicableObjectType equals applicable.SettingKey into emptyApplicable
                           from applicable in emptyApplicable.DefaultIfEmpty()
                           select new TpPromotionGeneralModel()
                           {
                               Id = p.Id,
                               PromotionType = p.PromotionType,
                               Code = p.Code,
                               ShortName = p.ShortName,
                               FullName = p.FullName,
                               Status = p.Status,
                               StatusDescription = (status != null) ? status.Description : string.Empty,
                               Scheme = p.Scheme,
                               EffectiveDateFrom = p.EffectiveDateFrom,
                               ValidUntil = p.ValidUntil,
                               SaleOrg = p.SaleOrg,
                               SicCode = p.SicCode,
                               SettlementFrequency = p.SettlementFrequency,
                               FrequencyPromotion = p.FrequencyPromotion,
                               ScopeType = p.ScopeType,
                               ScopeTypeDescription = (scope != null) ? scope.Description : string.Empty,
                               ApplicableObjectType = p.ApplicableObjectType,
                               ApplicableObjectTypeDescription = (applicable != null) ? applicable.Description : string.Empty,
                               IsApplyBudget = p.IsApplyBudget,
                               UserName = p.UserName
                           }).AsQueryable();

            return results;
        }

        public IQueryable<TpPromotionGeneralModel> GetListPromotionGeneralForSelettlement(string calendar)
        {
            string saleCalendar = string.Empty;
            if (!string.IsNullOrEmpty(calendar))
            {
                saleCalendar = calendar.ToLower();
            }
            //var lstPromotionCodeUseds = _dpTpSettlementObject.GetAllQueryable(x => x.DeleteFlag == 0
            //&& x.ProgramType.Equals(CommonData.PromotionSetting.PromotionProgram))
            //    .Select(y => y.PromotionDiscountCode.ToLower()).AsNoTracking().AsQueryable();

            var systemSettings = _dbSystemSetting.GetAllQueryable(x => x.IsActive).AsNoTracking().AsQueryable();
            DateTime now = DateTime.Now;

            //var result1s = (from p in _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0 && x.Status.ToLower().Equals(CommonData.PromotionSetting.Confirmed.ToLower())).AsNoTracking()
            //                join status in systemSettings.Where(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.PromotionStatus.ToLower()))
            //                on p.Status equals status.SettingKey into emptyStatus
            //                from status in emptyStatus.DefaultIfEmpty()
            //                where p.SettlementFrequency == CommonData.PromotionSetting.AccordingToTheProgram && p.ValidUntil <= now
            //                && (lstPromotionCodeUseds.All(x => !x.Equals(p.Code.ToLower())))
            //                select new TpPromotionGeneralModel()
            //                {
            //                    Id = p.Id,
            //                    PromotionType = p.PromotionType,
            //                    Code = p.Code,
            //                    ShortName = p.ShortName,
            //                    FullName = p.FullName,
            //                    Status = p.Status,
            //                    StatusDescription = (status != null) ? status.Description : string.Empty,
            //                    Scheme = p.Scheme,
            //                    EffectiveDateFrom = p.EffectiveDateFrom,
            //                    ValidUntil = p.ValidUntil,
            //                    SaleOrg = p.SaleOrg,
            //                    SicCode = p.SicCode,
            //                    SettlementFrequency = p.SettlementFrequency,
            //                    FrequencyPromotion = p.FrequencyPromotion,
            //                    ScopeType = p.ScopeType,
            //                    ApplicableObjectType = p.ApplicableObjectType,
            //                    IsApplyBudget = p.IsApplyBudget,
            //                    UserName = p.UserName
            //                }).AsQueryable();

            //var lstPromotion = (from p in _dpTpSettlementObject.GetAllQueryable(x => x.DeleteFlag == 0 && x.ProgramType.Equals(CommonData.PromotionSetting.PromotionProgram)).AsNoTracking()
            //                    join s in _dpTpSettlement.GetAllQueryable(x => x.DeleteFlag == 0 && x.ProgramType.Equals(CommonData.PromotionSetting.PromotionProgram)).AsNoTracking()
            //                    on p.SettlementCode.ToLower() equals s.Code.ToLower()
            //                    where s.SaleCalendarCode.ToLower().Equals(saleCalendar.ToLower())
            //                    select new TpSettlementObjectWithCalendarModel()
            //                    {
            //                        ProgramType = p.ProgramType,
            //                        SettlementCode = p.SettlementCode,
            //                        PromotionDiscountCode = p.PromotionDiscountCode,
            //                        SaleCalendarCode = s.SaleCalendarCode
            //                    }).AsQueryable();

            //var result2s = (from p in _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0 && x.Status.ToLower().Equals(CommonData.PromotionSetting.Confirmed.ToLower())).AsNoTracking()
            //                join status in systemSettings.Where(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.PromotionStatus.ToLower()))
            //                on p.Status equals status.SettingKey into emptyStatus
            //                from status in emptyStatus.DefaultIfEmpty()
            //                where p.SettlementFrequency == CommonData.PromotionSetting.SaleCalendar
            //                && lstPromotion.All(x => !x.PromotionDiscountCode.ToLower().Equals(p.Code.ToLower()))
            //                select new TpPromotionGeneralModel()
            //                {
            //                    Id = p.Id,
            //                    PromotionType = p.PromotionType,
            //                    Code = p.Code,
            //                    ShortName = p.ShortName,
            //                    FullName = p.FullName,
            //                    Status = p.Status,
            //                    StatusDescription = (status != null) ? status.Description : string.Empty,
            //                    Scheme = p.Scheme,
            //                    EffectiveDateFrom = p.EffectiveDateFrom,
            //                    ValidUntil = p.ValidUntil,
            //                    SaleOrg = p.SaleOrg,
            //                    SicCode = p.SicCode,
            //                    SettlementFrequency = p.SettlementFrequency,
            //                    FrequencyPromotion = p.FrequencyPromotion,
            //                    ScopeType = p.ScopeType,
            //                    ApplicableObjectType = p.ApplicableObjectType,
            //                    IsApplyBudget = p.IsApplyBudget,
            //                    UserName = p.UserName
            //                }).AsQueryable();

            //return result1s.Union(result2s);

            return (from p in _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0 && x.Status == CommonData.PromotionSetting.Confirmed.ToLower()).AsNoTracking()
                    join status in systemSettings.Where(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.PromotionStatus.ToLower()))
                    on p.Status equals status.SettingKey into emptyStatus
                    from status in emptyStatus.DefaultIfEmpty()
                    where p.EffectiveDateFrom <= now
                    select new TpPromotionGeneralModel()
                    {
                        Id = p.Id,
                        PromotionType = p.PromotionType,
                        Code = p.Code,
                        ShortName = p.ShortName,
                        FullName = p.FullName,
                        Status = p.Status,
                        StatusDescription = (status != null) ? status.Description : string.Empty,
                        Scheme = p.Scheme,
                        EffectiveDateFrom = p.EffectiveDateFrom,
                        ValidUntil = p.ValidUntil,
                        SaleOrg = p.SaleOrg,
                        SicCode = p.SicCode,
                        SettlementFrequency = p.SettlementFrequency,
                        FrequencyPromotion = p.FrequencyPromotion,
                        ScopeType = p.ScopeType,
                        ApplicableObjectType = p.ApplicableObjectType,
                        IsApplyBudget = p.IsApplyBudget,
                        UserName = p.UserName
                    }).AsQueryable();
        }

        public List<TpPromotionDefinitionProductForGiftModel> GetListProductForGiftByPromotionCode(string promotionCode)
        {
            DateTime now = DateTime.Now;
            var result = new List<TpPromotionDefinitionProductForGiftModel>();
            var promotionStructures = _dbPromotionDefinitionStructure
                .GetAllQueryable(x => x.PromotionCode.ToLower().Equals(promotionCode.ToLower())).AsNoTracking().ToList();

            var productForGifts = _dbPromotionDefinitionProductForGift.GetAllQueryable(x => x.DeleteFlag == 0
            && x.PromotionCode.ToLower().Equals(promotionCode.ToLower())).AsNoTracking();

            var lstSkus = _dbInventoryItem.GetAllQueryable(x => x.DelFlg == 0).AsNoTracking();
            var lstItemGroups = _dbItemGroup.GetAllQueryable().AsNoTracking();
            var lstAttributes = _dbItemAttribute.GetAllQueryable(x => x.DeleteFlag == 0
            && x.EffectiveDate <= now && (!x.ValidUntilDate.HasValue || x.ValidUntilDate.Value >= now)).AsNoTracking();

            var lstUoms = _dbUom.GetAllQueryable(x => x.DeleteFlag == 0
            && x.EffectiveDateFrom <= now && (!x.ValidUntil.HasValue || x.ValidUntil.Value >= now)).AsNoTracking();

            var lstBudgets = _dbPromotionBudget.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking();

            foreach (var item in promotionStructures)
            {
                if (item.ProductTypeForGift.Equals(CommonData.PromotionSetting.SKU))
                {
                    var productByLevel = (from gift in productForGifts.Where(x => x.LevelCode.ToLower().Equals(item.LevelCode.ToLower()))
                                          join sku in lstSkus on gift.ProductCode equals sku.InventoryItemId
                                          join uom in lstUoms on gift.Packing equals uom.UomId
                                          join bud in lstBudgets on gift.BudgetCode equals bud.Code into EmptyBud
                                          from bud in EmptyBud.DefaultIfEmpty()
                                          select new TpPromotionDefinitionProductForGiftModel()
                                          {
                                              Id = gift.Id,
                                              PromotionCode = gift.PromotionCode,
                                              LevelCode = gift.LevelCode,
                                              ProductCode = gift.ProductCode,
                                              ProductDescription = sku.Description,
                                              Packing = gift.Packing,
                                              PackingDescription = uom.Description,
                                              NumberOfGift = gift.NumberOfGift,
                                              BudgetCode = gift.BudgetCode,
                                              BudgetName = (bud != null) ? bud.Name : string.Empty,
                                              IsDefaultProduct = gift.IsDefaultProduct,
                                              Exchange = gift.Exchange,
                                              ProductType = item.ProductTypeForGift
                                          }).ToList();
                    result.AddRange(productByLevel);
                }
                else if (item.ProductTypeForGift.Equals(CommonData.PromotionSetting.ItemGroup))
                {
                    var productByLevel = (from gift in productForGifts.Where(x => x.LevelCode.ToLower().Equals(item.LevelCode.ToLower()))
                                          join ig in lstItemGroups on gift.ProductCode equals ig.Code
                                          join uom in lstUoms on gift.Packing equals uom.UomId
                                          join bud in lstBudgets on gift.BudgetCode equals bud.Code into EmptyBud
                                          from bud in EmptyBud.DefaultIfEmpty()
                                          select new TpPromotionDefinitionProductForGiftModel()
                                          {
                                              Id = gift.Id,
                                              PromotionCode = gift.PromotionCode,
                                              LevelCode = gift.LevelCode,
                                              ProductCode = gift.ProductCode,
                                              ProductDescription = ig.Description,
                                              Packing = gift.Packing,
                                              PackingDescription = uom.Description,
                                              NumberOfGift = gift.NumberOfGift,
                                              BudgetCode = gift.BudgetCode,
                                              BudgetName = (bud != null) ? bud.Name : string.Empty,
                                              IsDefaultProduct = gift.IsDefaultProduct,
                                              Exchange = gift.Exchange,
                                              ProductType = item.ProductTypeForGift
                                          }).ToList();
                    result.AddRange(productByLevel);
                }
                else
                {
                    var productByLevel = (from gift in productForGifts.Where(x => x.LevelCode.ToLower().Equals(item.LevelCode.ToLower()))
                                          from att in lstAttributes.Where(x => item.ItemHierarchyLevelForGift.ToLower().Equals(x.ItemAttributeMaster.ToLower())).DefaultIfEmpty()
                                          join uom in lstUoms on gift.Packing equals uom.UomId
                                          join bud in lstBudgets on gift.BudgetCode equals bud.Code into EmptyBud
                                          from bud in EmptyBud.DefaultIfEmpty()
                                          select new TpPromotionDefinitionProductForGiftModel()
                                          {
                                              Id = gift.Id,
                                              PromotionCode = gift.PromotionCode,
                                              LevelCode = gift.LevelCode,
                                              ProductCode = gift.ProductCode,
                                              ProductDescription = att.Description,
                                              Packing = gift.Packing,
                                              PackingDescription = uom.Description,
                                              NumberOfGift = gift.NumberOfGift,
                                              BudgetCode = gift.BudgetCode,
                                              BudgetName = (bud != null) ? bud.Name : string.Empty,
                                              IsDefaultProduct = gift.IsDefaultProduct,
                                              Exchange = gift.Exchange,
                                              ProductType = item.ProductTypeForGift
                                          }).ToList();
                    result.AddRange(productByLevel);
                }
            }
            return result;
        }

        public List<TpBudgetModel> GetListBudgetByPromotionCode(string promotionCode)
        {
            var result = new List<TpBudgetModel>();
            var lstBudgets = _dbPromotionBudget.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking();
            var lstBudgetDefines = _dpTpBudgetDefine.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking();
            var lstBudgetAllotment = _dbTpBudgetAllotment.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking();
            var lstTerritorys = _dbTerritoryValue.GetAllQueryable(x => !x.IsDeleted).AsNoTracking();

            var lstBugetForGifts = _dbPromotionDefinitionProductForGift.GetAllQueryable(x => x.DeleteFlag == 0
            && x.PromotionCode.ToLower().Equals(promotionCode.ToLower()) && !string.IsNullOrEmpty(x.BudgetCode)).ToList();
            var lstBudgetForDonations = _dbPromotionDefinitionStructure.GetAllQueryable(x => x.DeleteFlag == 0
            && x.PromotionCode.ToLower().Equals(promotionCode.ToLower()) && !string.IsNullOrEmpty(x.BudgetForDonation)).ToList();

            foreach (var item in lstBugetForGifts)
            {
                var budget = GetBudgetByCode(item.BudgetCode, lstBudgets, lstBudgetDefines, lstBudgetAllotment, lstTerritorys);
                if (budget != null)
                {
                    result.Add(budget);
                }
            }

            foreach (var item in lstBudgetForDonations)
            {
                var budget = GetBudgetByCode(item.BudgetForDonation, lstBudgets, lstBudgetDefines, lstBudgetAllotment, lstTerritorys);
                if (budget != null)
                {
                    result.Add(budget);
                }
            }
            return result.GroupBy(p => p.Id).Select(g => g.First()).ToList();
        }

        public TpBudgetModel GetBudgetByCode(string code, IQueryable<TpBudget> lstBudgets, IQueryable<TpBudgetDefine> lstBudgetDefines
            , IQueryable<TpBudgetAllotment> lstBudgetAllotment, IQueryable<ScTerritoryValue> lstTerritorys)
        {
            var returnValue = _mapper.Map<TpBudgetModel>(lstBudgets.Where(m => m.Code == code && m.DeleteFlag == 0).FirstOrDefault());
            if (returnValue != null)
            {
                returnValue.BudgetDefineModel = _mapper.Map<TpBudgetDefineModel>(lstBudgetDefines.Where(m => m.BudgetCode == returnValue.Code && m.DeleteFlag == 0).FirstOrDefault());
                var dataBudgetAllotmentModels = (from b in lstBudgetAllotment.Where(x => x.BudgetCode.ToLower().Equals(code.ToLower()))
                                                 join t in lstTerritorys on
                                                 new { code = b.SalesTerritoryValueCode, territoryLevelCode = returnValue.BudgetAllocationLevel } equals
                                                 new { code = t.Code, territoryLevelCode = t.TerritoryLevelCode } into tData
                                                 from t in tData.DefaultIfEmpty()
                                                 select new TpBudgetAllotmentModel()
                                                 {
                                                     Id = b.Id,
                                                     BudgetCode = b.BudgetCode,
                                                     BudgetQuantityDetail = b.BudgetQuantityDetail,
                                                     BudgetQuantityLimitDetail = b.BudgetQuantityLimitDetail,
                                                     SalesTerritoryValueCode = b.SalesTerritoryValueCode,
                                                     FlagBudgetQuantityLimitDetail = b.FlagBudgetQuantityLimitDetail,
                                                     SalesTerritoryValueDescription = (t != null) ? t.Description : string.Empty
                                                 }).ToList();
                returnValue.BudgetAllotmentModels = new List<TpBudgetAllotmentModel>();
                returnValue.BudgetAllotmentModels = dataBudgetAllotmentModels;
            }
            return returnValue;
        }

        public PromotionDefinitionForSettlementModel GetPromotionDefinitionForSettlement(string promotionCode)
        {
            var lstLevel = _dbPromotionDefinitionStructure.FirstOrDefault(x => x.PromotionCode.ToLower().Equals(promotionCode.ToLower()) && x.DeleteFlag == 0);
            if (lstLevel != null)
            {
                return _mapper.Map<PromotionDefinitionForSettlementModel>(lstLevel);
            }
            return null;
        }

        public async Task<PromotionInitialModel> GetDataInitialPromotionMain()
        {
            PromotionInitialModel result = new PromotionInitialModel();
            DateTime now = DateTime.Now;

            result.SystemSettingModels = await (from x in _dbSystemSetting.GetAllQueryable(x => x.IsActive)
                                                select new SystemSettingModel()
                                                {
                                                    Id = x.Id,
                                                    SettingType = x.SettingType,
                                                    SettingKey = x.SettingKey,
                                                    SettingValue = x.SettingValue,
                                                    Description = x.Description
                                                }).ToListAsync();

            result.TpSalesOrganizationModels = await (from org in _dbSaleOrg.GetAllQueryable(x => !x.IsDeleted && x.IsActive
                         && (x.EffectiveDate <= now && (!x.UntilDate.HasValue || (x.UntilDate.Value >= now))))
                                                      select new TpSalesOrganizationModel()
                                                      {
                                                          Id = org.Id.ToString(),
                                                          Code = org.Code,
                                                          TerritoryStructureCode = org.TerritoryStructureCode,
                                                          Description = org.Description,
                                                          EffectiveDate = org.EffectiveDate,
                                                          UntilDate = org.UntilDate,
                                                          IsActive = org.IsActive,
                                                          IsSpecificChanel = org.IsSpecificChanel,
                                                          Channel = org.Channel,
                                                          IsSpecificSIC = (org.IsSpecificSic != null) && org.IsSpecificSic.Value,
                                                          SIC = org.Sic
                                                      }).ToListAsync();

            result.SicPrimaryModels = await (from x in _dbPrimarySic.GetAllQueryable(x => x.DeleteFlag == 0 && x.EffectiveDate <= now && (!x.ValidUntil.HasValue || (x.ValidUntil.Value >= now)))
                                             select new SicPrimaryModel()
                                             {
                                                 Id = x.Id,
                                                 Code = x.Code,
                                                 Description = $"{x.Code} - {x.Description}",
                                                 Status = x.Status
                                             }).ToListAsync();

            result.UomsModels = await (from x in _dbUom.GetAllQueryable(x => x.DeleteFlag == 0 && x.EffectiveDateFrom <= now && (!x.ValidUntil.HasValue || (x.ValidUntil.Value >= now)))
                                       select new UomsModel()
                                       {
                                           Id = x.Id,
                                           UomId = x.UomId,
                                           Description = x.Description
                                       }).ToListAsync();

            if (result.TpSalesOrganizationModels != null && result.TpSalesOrganizationModels.Count > 0)
            {
                result.TpTerritoryStructureLevelModels = await (from level in _dbSalesTerritoryLevel.GetAllQueryable(x => !x.IsDeleted
                    && x.TerritoryStructureCode.ToLower().Equals(result.TpSalesOrganizationModels.FirstOrDefault().TerritoryStructureCode))
                                                                select new TpTerritoryStructureLevelModel()
                                                                {
                                                                    Id = level.Id.ToString(),
                                                                    TerritoryStructureCode = level.TerritoryStructureCode,
                                                                    Description = level.Description,
                                                                    Level = level.Level,
                                                                    TerritoryLevelCode = level.TerritoryLevelCode
                                                                }).ToListAsync();
            }

            result.CustomerSettingModels = await (from x in _dbCustomerSetting.GetAllQueryable()
                                                  select new CustomerSettingModel()
                                                  {
                                                      Id = x.Id,
                                                      AttributeID = x.AttributeId,
                                                      AttributeName = x.AttributeName,
                                                      Description = x.Description,
                                                      IsDistributorAttribute = x.IsDistributorAttribute,
                                                      IsCustomerAttribute = x.IsCustomerAttribute,
                                                      Used = x.Used,
                                                  }).ToListAsync();

            result.ItemSettingModels = await (from x in _dbItemSetting.GetAllQueryable()
                                              select new ItemSettingModel()
                                              {
                                                  Id = x.Id,
                                                  AttributeId = x.AttributeId,
                                                  AttributeName = x.AttributeName,
                                                  Description = x.Description,
                                                  IsHierarchy = x.IsHierarchy,
                                                  Used = x.Used,
                                                  Level = x.Level
                                              }).ToListAsync();

            result.TpBudgetListModels = await (from x in _dbPromotionBudget.GetAllQueryable(x => x.DeleteFlag == 0 && x.Status != CommonData.PromotionSetting.StatusDefining)
                                               select new TpBudgetListModel()
                                               {
                                                   Id = x.Id,
                                                   Code = x.Code,
                                                   Name = x.Name,
                                                   BudgetType = x.BudgetType,
                                               }).ToListAsync();

            result.InventoryItemModels = await (from x in _dbInventoryItem.GetAllQueryable(x => x.DelFlg == 0 && x.Status == CommonData.ItemStatus.Active)
                                                select new InventoryItemModel()
                                                {
                                                    Id = x.Id,
                                                    InventoryItemId = x.InventoryItemId,
                                                    Description = x.Description
                                                }).ToListAsync();

            return result;
        }

        public async Task<PromotionInitialModel> GetDataInitialPromotionMainByDapper()
        {
            PromotionInitialModel result = new PromotionInitialModel();

            var multipeSql = @$"select ss.""Id"", ss.""SettingType"", ss.""SettingKey"", ss.""SettingValue"",ss.""Description""  from ""SystemSettings"" ss where ""IsActive"" = true;
                                select concat(ssos.""Id"") as Id, ssos.""Code"", ssos.""TerritoryStructureCode"", ssos.""Description"" from ""SC_SalesOrganizationStructures"" ssos where ssos.""IsDeleted"" <> true and ssos.""EffectiveDate"" <= now() and (ssos.""UntilDate"" is null or ssos.""UntilDate"" >= now()) and ssos.""IsActive"" = true;
                                select ps.""Id"", ps.""Code"", concat(ps.""Code"", ' - ', ps.""Description"") as Description, ps.""Status"" from ""PrimarySics"" ps where ps.""DeleteFlag"" = 0 and ps.""EffectiveDate"" <= now() and (ps.""ValidUntil"" is null or ps.""ValidUntil"" > now());
                                select u.""Id"", u.""UomId"", u.""Description"" from ""Uoms"" u where u.""DeleteFlag"" = 0 and u.""EffectiveDateFrom"" <= now() and(u.""ValidUntil"" is null or u.""ValidUntil"" > now());
                                select cs.""Id"", cs.""AttributeID"", cs.""AttributeName"", cs.""Description"", cs.""IsDistributorAttribute"", cs.""IsCustomerAttribute"", cs.""Used"" from ""CustomerSettings"" cs;
                                select is2.* from ""ItemSettings"" is2 ;                                
                                select tb.""Id"", tb.""Code"", tb.""Name"", tb.""BudgetType"" from ""TpBudgets"" tb where tb.""DeleteFlag"" = 0 and tb.""Status"" <> '01';
                                select ii.""Id"", ii.""InventoryItemId"", ii.""Description"" from ""InventoryItems"" ii where ii.""DelFlg"" = 0 and ii.""Status"" = '1'; 
                                select concat(stsd.""Id"") as Id, stsd.""TerritoryStructureCode"", stsd.""Description"", stsd.""Level"", stsd.""TerritoryLevelCode"" from ""SC_TerritoryStructureDetails"" stsd where stsd.""IsDeleted"" <> true and stsd.""TerritoryStructureCode"" = 
                                    (select ssos.""TerritoryStructureCode"" from ""SC_SalesOrganizationStructures"" ssos where ssos.""IsDeleted"" <> true and ssos.""EffectiveDate"" <= now() and  (ssos.""UntilDate"" is null or ssos.""UntilDate"" >= now()) limit 1);
                                ";
            var tempResult = await _dapper.QueryMultiple(multipeSql);
            result.SystemSettingModels = tempResult.Read<SystemSettingModel>().ToList();
            result.TpSalesOrganizationModels = tempResult.Read<TpSalesOrganizationModel>().ToList();
            result.SicPrimaryModels = tempResult.Read<SicPrimaryModel>().ToList();
            result.UomsModels = tempResult.Read<UomsModel>().ToList();
            result.CustomerSettingModels = tempResult.Read<CustomerSettingModel>().ToList();
            result.ItemSettingModels = tempResult.Read<ItemSettingModel>().ToList();
            result.TpBudgetListModels = tempResult.Read<TpBudgetListModel>().ToList();
            result.InventoryItemModels = tempResult.Read<InventoryItemModel>().ToList();
            result.TpTerritoryStructureLevelModels = tempResult.Read<TpTerritoryStructureLevelModel>().ToList();
            return result;
        }

        #region External API
        public async Task<List<PromotionExternalModel>> GetListPromotionByCustomer(ListPromotionAndDiscountRequestModel request)
        {
            var now = DateTime.Now;
            List<PromotionExternalModel> result = new List<PromotionExternalModel>();
            try
            {
                var dbCustomerInfo = _dbCustomerInformation.GetAllQueryable(x => x.CustomerCode == request.CustomerCode).AsNoTracking();
                var dbCustomerShipto = _dbCustomerShipto.GetAllQueryable(x => x.ShiptoCode == request.ShiptoCode);

                // List By Scope Territory
                var lstByScopeTerritory = await _dbPromotionScopeTerritory.GetAllQueryable(x => x.SaleOrg == request.SaleOrgCode
                                                && ((x.ScopeSaleTerritoryLevel == CommonData.TerritoryLevelSetting.Branch && x.SalesTerritoryValue == request.Branch)
                                                || (x.ScopeSaleTerritoryLevel == CommonData.TerritoryLevelSetting.Region && x.SalesTerritoryValue == request.Region)
                                                || (x.ScopeSaleTerritoryLevel == CommonData.TerritoryLevelSetting.SubRegion && x.SalesTerritoryValue == request.SubRegion)
                                                || (x.ScopeSaleTerritoryLevel == CommonData.TerritoryLevelSetting.Area && x.SalesTerritoryValue == request.Area)
                                                || (x.ScopeSaleTerritoryLevel == CommonData.TerritoryLevelSetting.SubArea && x.SalesTerritoryValue == request.SubArea)))
                                                .Select(x => x.PromotionCode).ToListAsync();

                // List By Scope Dsa
                var lstByScopeDsa = await _dbPromotionScopeDsa.GetAllQueryable(x => x.SaleOrg == request.SaleOrgCode
                                    && x.ScopeDsaValue == request.DsaCode).Select(x => x.PromotionCode).ToListAsync();

                // List promotion For Object Applicable Customer Attribute.
                var CustomerObjects = await (from ci in dbCustomerInfo
                                             join cs in dbCustomerShipto
                                             on ci.Id equals cs.CustomerInfomationId
                                             join cda in _dbCustomerDsm.GetAllQueryable()
                                             on cs.Id equals cda.CustomerShiptoId
                                             join ca in _dbCustomerAttribute.GetAllQueryable(x => x.EffectiveDate <= now && (!x.ValidUntil.HasValue || x.ValidUntil.Value >= now))
                                             on cda.CustomerAttributeId equals ca.Id
                                             join cs2 in _dbCustomerSetting.GetAllQueryable()
                                             on ca.CustomerSettingId equals cs2.Id
                                             select new CustomerObjectApplicable()
                                             {
                                                 CustomerCode = ci.CustomerCode,
                                                 ShiptoCode = cs.ShiptoCode,
                                                 CustomerAttributeLevel = cs2.AttributeId,
                                                 CustomerAttributeValue = ca.Code
                                             }).AsNoTracking().ToListAsync();

                // Applicable Object Customer Attribute
                var lstAllTpObjectCustomerAttributes = await _dbTpPromotionObjectCustomerAttributeValues.GetAllQueryable().AsNoTracking().ToListAsync();
                var lstTpCustomerAttribute = new List<string>();
                foreach (var item in CustomerObjects)
                {
                    var tempDataScope = lstAllTpObjectCustomerAttributes.Where(x =>
                    x.CustomerAttributerLevel == item.CustomerAttributeLevel
                    && x.CustomerAttributerValue == item.CustomerAttributeValue).Select(z => z.PromotionCode);

                    lstTpCustomerAttribute.AddRange(tempDataScope);
                }

                // Applicable Object Customer Shipto
                var lstTpCustomerShipto = await _dbTpPromotionObjectCustomerShipto.GetAllQueryable(x => x.CustomerCode == request.CustomerCode
                && x.CustomerShiptoCode == request.ShiptoCode).Select(y => y.PromotionCode).AsNoTracking().ToListAsync();

                // List promotion
                var lstPromotions = await (from p in _dbPromotion.GetAllQueryable(x => x.Status == CommonData.PromotionSetting.Confirmed
                                           && x.SaleOrg == request.SaleOrgCode && x.SicCode == request.SicCode
                                           && x.EffectiveDateFrom <= DateTime.Now && x.ValidUntil >= DateTime.Now)
                                           select new PromotionExternalModel()
                                           {
                                               Id = p.Id,
                                               Code = p.Code,
                                               ShortName = p.ShortName,
                                               FullName = p.FullName,
                                               Scheme = p.Scheme,
                                               Status = p.Status,
                                               EffectiveDateFrom = p.EffectiveDateFrom,
                                               ValidUntil = p.ValidUntil,
                                               SaleOrg = p.SaleOrg,
                                               ScopeType = p.ScopeType,
                                               ApplicableObjectType = p.ApplicableObjectType,
                                               SicCode = p.SicCode
                                           }
                                      ).AsNoTracking().ToListAsync();

                // TH1. List Scope All - Applicable All
                var lstPromotion1 = lstPromotions.Where(x => x.ScopeType == CommonData.PromotionSetting.ScopeNationwide
                                    && x.ApplicableObjectType == CommonData.PromotionSetting.ObjectAllCustomer).ToList();

                // TH2. List Promotion Scope All - Customer Attribute
                var lstPromotion2 = lstPromotions.Where(x => x.ScopeType == CommonData.PromotionSetting.ScopeNationwide
                                    && lstTpCustomerAttribute.Contains(x.Code)).ToList();

                // TH3. List Promotion Scope All - Customer Shipto
                var lstPromotion3 = lstPromotions.Where(x => x.ScopeType == CommonData.PromotionSetting.ScopeNationwide
                                    && lstTpCustomerShipto.Contains(x.Code)).ToList();

                // TH4. List Promotion Scope Territory - Applicable Object all
                var lstPromotion4 = lstPromotions.Where(x => lstByScopeTerritory.Contains(x.Code)
                                    && x.ApplicableObjectType == CommonData.PromotionSetting.ObjectAllCustomer).ToList();

                // TH5. List Promotion Scope Territory - Customer Attribute
                var lstPromotion5 = lstPromotions.Where(x => lstByScopeTerritory.Contains(x.Code)
                                    && lstTpCustomerAttribute.Contains(x.Code)).ToList();

                // TH6. List Promotion Scope Dsa - Applicable Object all
                var lstPromotion6 = lstPromotions.Where(x => lstByScopeDsa.Contains(x.Code)
                                    && x.ApplicableObjectType == CommonData.PromotionSetting.ObjectAllCustomer).ToList();

                // TH7. List Promotion Scope Territory - Customer Attribute
                var lstPromotion7 = lstPromotions.Where(x => lstByScopeDsa.Contains(x.Code)
                                    && lstTpCustomerAttribute.Contains(x.Code)).ToList();

                result.AddRange(lstPromotion1);
                result.AddRange(lstPromotion2);
                result.AddRange(lstPromotion3);
                result.AddRange(lstPromotion4);
                result.AddRange(lstPromotion5);
                result.AddRange(lstPromotion6);
                result.AddRange(lstPromotion7);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private async Task<CustomerScopeModel> GetCustomerShiptoWithScope(string CustomerCode, string ShiptoCode, string RouteZoneCode)
        {
            var now = DateTime.Now;
            var dbScTerritoryMapping = _dbScTerritoryMapping.GetAllQueryable(x => !x.IsDeleted && x.EffectiveDate <= now && (!x.UntilDate.HasValue || x.UntilDate >= now)).AsNoTracking();
            var dbTerritoryValue = _dbTerritoryValue.GetAllQueryable(x => !x.IsDeleted && x.EffectiveDate <= now && (!x.UntilDate.HasValue || x.UntilDate >= now)).AsNoTracking();
            var dbCustomerInfo = _dbCustomerInformation.GetAllQueryable(x => x.CustomerCode == CustomerCode).AsNoTracking();
            var dbCustomerShipto = _dbCustomerShipto.GetAllQueryable(x => x.ShiptoCode == ShiptoCode);

            return await (from so in _dbSaleOrg.GetAllQueryable(x => !x.IsDeleted && x.IsActive && x.EffectiveDate <= now && (!x.UntilDate.HasValue || x.UntilDate >= now)).AsNoTracking()
                          join ddsa in _dbDsa.GetAllQueryable(x => !x.IsDeleted && x.IsActive && x.EffectiveDate <= now && (!x.UntilDate.HasValue || x.UntilDate >= now)).AsNoTracking()
                          on so.Code equals ddsa.SOStructureCode
                          join rz in _dbRzInfo.GetAllQueryable(x => !x.IsDeleted && x.EffectiveDate <= now && (!x.ValidUntil.HasValue || x.ValidUntil >= now) && x.RouteZoneCode == RouteZoneCode).AsNoTracking()
                          on ddsa.Code equals rz.Dsacode
                          join rzs in _dbRzShipto.GetAllQueryable(x => x.EffectiveDate <= now && (!x.ValidUntil.HasValue || x.ValidUntil >= now)).AsNoTracking()
                          on rz.RouteZoneCode equals rzs.RouteZoneCode
                          join cuss in dbCustomerShipto
                          on rzs.ShiptoId equals cuss.Id
                          join cus in dbCustomerInfo
                          on cuss.CustomerInfomationId equals cus.Id

                          join sc1 in dbScTerritoryMapping
                          on ddsa.MappingNode equals sc1.MappingNode
                          join vl1 in dbTerritoryValue
                          on sc1.TerritoryValueKey equals vl1.Key

                          join sc2 in dbScTerritoryMapping
                          on sc1.ParentMappingNode equals sc2.MappingNode into emptySc2
                          from sc2 in emptySc2.DefaultIfEmpty()
                          join vl2 in dbTerritoryValue
                          on sc2.TerritoryValueKey equals vl2.Key into emptyVl2
                          from vl2 in emptyVl2.DefaultIfEmpty()

                          join sc3 in dbScTerritoryMapping
                          on sc2.ParentMappingNode equals sc3.MappingNode into emptySc3
                          from sc3 in emptySc3.DefaultIfEmpty()
                          join vl3 in dbTerritoryValue
                          on sc3.TerritoryValueKey equals vl3.Key into emptyVl3
                          from vl3 in emptyVl3.DefaultIfEmpty()

                          join sc4 in dbScTerritoryMapping
                          on sc3.ParentMappingNode equals sc4.MappingNode into emptySc4
                          from sc4 in emptySc4.DefaultIfEmpty()
                          join vl4 in dbTerritoryValue
                          on sc4.TerritoryValueKey equals vl4.Key into emptyVl4
                          from vl4 in emptyVl4.DefaultIfEmpty()

                          join sc5 in dbScTerritoryMapping
                          on sc4.ParentMappingNode equals sc5.MappingNode into emptySc5
                          from sc5 in emptySc5.DefaultIfEmpty()
                          join vl5 in dbTerritoryValue
                          on sc5.TerritoryValueKey equals vl5.Key into emptyVl5
                          from vl5 in emptyVl5.DefaultIfEmpty()

                          join sc6 in dbScTerritoryMapping
                          on sc5.ParentMappingNode equals sc6.MappingNode into emptySc6
                          from sc6 in emptySc6.DefaultIfEmpty()
                          join vl6 in dbTerritoryValue
                          on sc6.TerritoryValueKey equals vl6.Key into emptyVl6
                          from vl6 in emptyVl6.DefaultIfEmpty()

                          select new CustomerScopeModel()
                          {
                              SaleOrgCode = so.Code,
                              TerritoryStructureCode = so.TerritoryStructureCode,
                              CustomerCode = cus.CustomerCode,
                              ShiptoCode = cuss.ShiptoCode,
                              Country = (
#pragma warning disable S3358 // Ternary operators should not be nested
                                sc1.Level == 0 ? vl1.Code :
                                  sc2.Level == 0 ? vl2.Code :
                                  sc3.Level == 0 ? vl3.Code :
                                  sc4.Level == 0 ? vl4.Code :
                                  sc5.Level == 0 ? vl5.Code :
                                  sc6.Level == 0 ? vl6.Code : string.Empty
                              ),
                              Branch = (
                                  sc1.Level == 1 ? vl1.Code :
                                  sc2.Level == 1 ? vl2.Code :
                                  sc3.Level == 1 ? vl3.Code :
                                  sc4.Level == 1 ? vl4.Code :
                                  sc5.Level == 1 ? vl5.Code :
                                  sc6.Level == 1 ? vl6.Code : string.Empty
                              ),
                              Region = (
                                  sc1.Level == 2 ? vl1.Code :
                                  sc2.Level == 2 ? vl2.Code :
                                  sc3.Level == 2 ? vl3.Code :
                                  sc4.Level == 2 ? vl4.Code :
                                  sc5.Level == 2 ? vl5.Code :
                                  sc6.Level == 2 ? vl6.Code : string.Empty
                              ),
                              SubRegion = (
                                  sc1.Level == 3 ? vl1.Code :
                                  sc2.Level == 3 ? vl2.Code :
                                  sc3.Level == 3 ? vl3.Code :
                                  sc4.Level == 3 ? vl4.Code :
                                  sc5.Level == 3 ? vl5.Code :
                                  sc6.Level == 3 ? vl6.Code : string.Empty
                              ),
                              Area = (
                                  sc1.Level == 4 ? vl1.Code :
                                  sc2.Level == 4 ? vl2.Code :
                                  sc3.Level == 4 ? vl3.Code :
                                  sc4.Level == 4 ? vl4.Code :
                                  sc5.Level == 4 ? vl5.Code :
                                  sc6.Level == 4 ? vl6.Code : string.Empty
                              ),
                              SubArea = (
                                  sc1.Level == 5 ? vl1.Code :
                                  sc2.Level == 5 ? vl2.Code :
                                  sc3.Level == 5 ? vl3.Code :
                                  sc4.Level == 5 ? vl4.Code :
                                  sc5.Level == 5 ? vl5.Code :
                                  sc6.Level == 5 ? vl6.Code : string.Empty
                              ),
#pragma warning restore S3358 // Ternary operators should not be nested
                              DSACode = ddsa.Code
                          }).FirstOrDefaultAsync();
        }

        public async Task<TpPromotionModel> GetDetailPromotionExternalByCode(string code)
        {
            DateTime now = DateTime.Now;
            var promotion = _mapper.Map<TpPromotionModel>(_dbPromotion.FirstOrDefault(x => x.Code.ToLower().Equals(code.ToLower())));
            if (promotion != null)
            {
                var lstLevel = await _dbPromotionDefinitionStructure
                                              .GetAllQueryable(x => x.PromotionCode.ToLower().Equals(code.ToLower())).AsNoTracking().ToListAsync();

                var lstDefinitionStructure = _mapper.Map<List<TpPromotionDefinitionStructureModel>>(lstLevel);

                var lstSKU = await _dbInventoryItem.GetAllQueryable(x => x.DelFlg == 0).AsNoTracking().ToListAsync();
                var lstItemGroup = await _dbItemGroup.GetAllQueryable().AsNoTracking().ToListAsync();
                var lstItemHierarchy = await _dbItemAttribute
                    .GetAllQueryable(x => x.DeleteFlag == 0 && x.EffectiveDate <= now && (!x.ValidUntilDate.HasValue || x.ValidUntilDate.Value >= now)).AsNoTracking().ToListAsync();
                var lstUom = await _dbUom.GetAllQueryable(x => x.DeleteFlag == 0
                && x.EffectiveDateFrom <= now && (!x.ValidUntil.HasValue || x.ValidUntil.Value >= now)).AsNoTracking().ToListAsync();
                var lstBudget = await _dbPromotionBudget.GetAllQueryable(x => x.DeleteFlag == 0
               && (x.Status.Equals(CommonData.PromotionSetting.StatusCanLinkPromotion) || x.Status.Equals(CommonData.PromotionSetting.StatusLinkedPromotion))).AsNoTracking().ToListAsync();
                // Get Product For Sale
                var lstAllProductForSaleByPromotion = await _dbPromotionDefinitionProductForSale
                                        .GetAllQueryable(x => x.DeleteFlag == 0 && x.PromotionCode.ToLower().Equals(code.ToLower())).AsNoTracking().ToListAsync();

                foreach (var item in lstDefinitionStructure)
                {
                    if (item.IsGiftApplyBudget)
                    {
                        item.BudgetCodeForGift = item.GiftApplyBudgetCode;
                        item.BudgetTypeOfGift = "01";
                        item.BudgetAllocationLevelOfGift = lstBudget.Where(o => o.Code.Equals(item.BudgetCodeForGift)).FirstOrDefault().BudgetAllocationLevel;
                    }
                    if (item.IsDonateApplyBudget)
                    {
                        item.BudgetCodeForDonate = item.DonateApplyBudgetCode;
                        item.BudgetTypeOfDonate = "02";
                        item.BudgetAllocationLevelOfDonate = lstBudget.Where(o => o.Code.Equals(item.BudgetCodeForDonate)).FirstOrDefault().BudgetAllocationLevel;
                    }
                    item.Allowance = item.IsDonateAllowance;
                    if (item.ProductTypeForSale.ToLower().Equals(CommonData.PromotionSetting.SKU))
                    {
                        var lstProduct = (from data in lstAllProductForSaleByPromotion
                                          join sku in lstSKU on data.ProductCode equals sku.InventoryItemId into emptySku
                                          from sku in emptySku.DefaultIfEmpty()
                                          join uom in lstUom on data.Packing equals uom.UomId into emptyUom
                                          from uom in emptyUom.DefaultIfEmpty()
                                          where data.LevelCode.ToLower().Equals(item.LevelCode.ToLower())
                                          select new TpPromotionDefinitionProductForSaleModel()
                                          {
                                              Id = data.Id,
                                              PromotionCode = code,
                                              LevelCode = item.LevelCode,
                                              ProductType = item.ProductTypeForSale,
                                              ProductCode = data.ProductCode,
                                              ProductDescription = (sku != null) ? sku.Description : string.Empty,
                                              Packing = data.Packing,
                                              PackingDescription = (uom != null) ? uom.Description : string.Empty,
                                              SellNumber = data.SellNumber
                                          }).ToList();

                        item.ListProductForSales.AddRange(lstProduct);
                    }
                    else if (item.ProductTypeForSale.ToLower().Equals(CommonData.PromotionSetting.ItemGroup))
                    {
                        var lstProduct = (from data in lstAllProductForSaleByPromotion
                                          join ig in lstItemGroup on data.ProductCode equals ig.Code into emptyIg
                                          from ig in emptyIg.DefaultIfEmpty()
                                          join uom in lstUom on data.Packing equals uom.UomId into emptyUom
                                          from uom in emptyUom.DefaultIfEmpty()
                                          where data.LevelCode.ToLower().Equals(item.LevelCode.ToLower())
                                          select new TpPromotionDefinitionProductForSaleModel()
                                          {
                                              Id = data.Id,
                                              PromotionCode = code,
                                              LevelCode = item.LevelCode,
                                              ProductType = item.ProductTypeForSale,
                                              ProductCode = data.ProductCode,
                                              ProductDescription = (ig != null) ? ig.Description : string.Empty,
                                              Packing = data.Packing,
                                              PackingDescription = (uom != null) ? uom.Description : string.Empty,
                                              SellNumber = data.SellNumber
                                          }).ToList();

                        item.ListProductForSales.AddRange(lstProduct);
                    }
                    else
                    {
                        var lstProduct = (from data in lstAllProductForSaleByPromotion
                                          join ih in lstItemHierarchy on data.ProductCode equals ih.ItemAttributeCode into emptyIg
                                          from ih in emptyIg.DefaultIfEmpty()
                                          join uom in lstUom on data.Packing equals uom.UomId into emptyUom
                                          from uom in emptyUom.DefaultIfEmpty()
                                          where item.ItemHierarchyLevelForSale.Equals(ih.ItemAttributeMaster) && data.LevelCode.ToLower().Equals(item.LevelCode.ToLower())
                                          select new TpPromotionDefinitionProductForSaleModel()
                                          {
                                              Id = data.Id,
                                              PromotionCode = code,
                                              LevelCode = item.LevelCode,
                                              ProductType = item.ProductTypeForSale,
                                              ProductCode = data.ProductCode,
                                              ProductDescription = (ih != null) ? ih.Description : string.Empty,
                                              Packing = data.Packing,
                                              PackingDescription = (uom != null) ? uom.Description : string.Empty,
                                              SellNumber = data.SellNumber
                                          }).ToList();

                        item.ListProductForSales.AddRange(lstProduct);
                    }
                }

                // Get Product For Gift
                var lstAllProductForGiftByPromotion = await _dbPromotionDefinitionProductForGift
                                        .GetAllQueryable(x => x.DeleteFlag == 0 && x.PromotionCode.ToLower().Equals(code.ToLower())).AsNoTracking().ToListAsync();
                foreach (var item in lstDefinitionStructure)
                {
                    if (item.ProductTypeForGift.ToLower().Equals(CommonData.PromotionSetting.SKU))
                    {
                        var lstProduct = (from data in lstAllProductForGiftByPromotion
                                          join sku in lstSKU on data.ProductCode equals sku.InventoryItemId into emptySku
                                          from sku in emptySku.DefaultIfEmpty()
                                          join uom in lstUom on data.Packing equals uom.UomId into emptyUom
                                          from uom in emptyUom.DefaultIfEmpty()
                                          join bud in lstBudget on data.BudgetCode equals bud.Code into emptyBudget
                                          from bud in emptyBudget.DefaultIfEmpty()
                                          where data.LevelCode.ToLower().Equals(item.LevelCode.ToLower())
                                          select new TpPromotionDefinitionProductForGiftModel()
                                          {
                                              Id = data.Id,
                                              PromotionCode = code,
                                              LevelCode = item.LevelCode,
                                              ProductType = item.ProductTypeForGift,
                                              ProductCode = data.ProductCode,
                                              ProductDescription = (sku != null) ? sku.Description : string.Empty,
                                              Packing = data.Packing,
                                              PackingDescription = (uom != null) ? uom.Description : string.Empty,
                                              NumberOfGift = data.NumberOfGift,
                                              BudgetCode = data.BudgetCode,
                                              BudgetName = (bud != null) ? bud.Name : string.Empty,
                                              IsDefaultProduct = data.IsDefaultProduct,
                                              Exchange = data.Exchange
                                          }).ToList();

                        item.ListProductForGifts.AddRange(lstProduct);
                    }
                    else if (item.ProductTypeForGift.ToLower().Equals(CommonData.PromotionSetting.ItemGroup))
                    {
                        var lstProduct = (from data in lstAllProductForGiftByPromotion
                                          join ig in lstItemGroup on data.ProductCode equals ig.Code into emptyIg
                                          from ig in emptyIg.DefaultIfEmpty()
                                          join uom in lstUom on data.Packing equals uom.UomId into emptyUom
                                          from uom in emptyUom.DefaultIfEmpty()
                                          join bud in lstBudget on data.BudgetCode equals bud.Code into emptyBudget
                                          from bud in emptyBudget.DefaultIfEmpty()
                                          where data.LevelCode.ToLower().Equals(item.LevelCode.ToLower())
                                          select new TpPromotionDefinitionProductForGiftModel()
                                          {
                                              Id = data.Id,
                                              PromotionCode = code,
                                              LevelCode = item.LevelCode,
                                              ProductType = item.ProductTypeForGift,
                                              ProductCode = data.ProductCode,
                                              ProductDescription = (ig != null) ? ig.Description : string.Empty,
                                              Packing = data.Packing,
                                              PackingDescription = (uom != null) ? uom.Description : string.Empty,
                                              NumberOfGift = data.NumberOfGift,
                                              BudgetCode = data.BudgetCode,
                                              BudgetName = (bud != null) ? bud.Name : string.Empty,
                                              IsDefaultProduct = data.IsDefaultProduct,
                                              Exchange = data.Exchange
                                          }).ToList();

                        item.ListProductForGifts.AddRange(lstProduct);
                    }
                    else
                    {
                        var lstProduct = (from data in lstAllProductForGiftByPromotion
                                          join ih in lstItemHierarchy on data.ProductCode equals ih.ItemAttributeCode into emptyIg
                                          from ih in emptyIg.DefaultIfEmpty()
                                          join uom in lstUom on data.Packing equals uom.UomId into emptyUom
                                          from uom in emptyUom.DefaultIfEmpty()
                                          join bud in lstBudget on data.BudgetCode equals bud.Code into emptyBudget
                                          from bud in emptyBudget.DefaultIfEmpty()
                                          where item.ItemHierarchyLevelForGift.Equals(ih.ItemAttributeMaster) && data.LevelCode.ToLower().Equals(item.LevelCode.ToLower())
                                          select new TpPromotionDefinitionProductForGiftModel()
                                          {
                                              Id = data.Id,
                                              PromotionCode = code,
                                              LevelCode = item.LevelCode,
                                              ProductType = item.ProductTypeForGift,
                                              ProductCode = data.ProductCode,
                                              ProductDescription = (ih != null) ? ih.Description : string.Empty,
                                              Packing = data.Packing,
                                              PackingDescription = (uom != null) ? uom.Description : string.Empty,
                                              NumberOfGift = data.NumberOfGift,
                                              BudgetCode = data.BudgetCode,
                                              BudgetName = (bud != null) ? bud.Name : string.Empty,
                                              IsDefaultProduct = data.IsDefaultProduct,
                                              Exchange = data.Exchange,
                                          }).ToList();
                        item.ListProductForGifts.AddRange(lstProduct);
                    }
                }
                promotion.ListDefinitionStructure.AddRange(lstDefinitionStructure);
            }
            return promotion;
        }

        public async Task<PromotionResultResponseModel> GetPromotionResult(PromotionResultRequestModel request)
        {
            PromotionResultResponseModel result = new PromotionResultResponseModel();
            result.ProductForSale = await ExternalApiGetProductForSale(request);
            result.ProductForGift = await ExternalApiGetProductForGift(request);

            return result;
        }

        public List<ItemGroupModel> ExternalGetListItemGroupByItemHierarchy(string ItemHierarchyLevel, string ItemHierarchyCode)
        {
            var itemGroups = _dbItemGroup.GetAllQueryable().AsNoTracking().AsQueryable();

            var ItemHierarchyId = _dbItemAttribute.FirstOrDefault(x => x.ItemAttributeMaster == ItemHierarchyLevel && x.ItemAttributeCode == ItemHierarchyCode);

            if (ItemHierarchyId != null && !string.IsNullOrEmpty(ItemHierarchyId.ItemAttributeCode))
            {
                switch (ItemHierarchyLevel)
                {
                    case CommonData.ItemHierarchyLevel.ItemHierarchyLevel1:
                        return _mapper.Map<List<ItemGroupModel>>(itemGroups.Where(x => x.Attribute1 == ItemHierarchyId.Id).ToList());
                    case CommonData.ItemHierarchyLevel.ItemHierarchyLevel2:
                        return _mapper.Map<List<ItemGroupModel>>(itemGroups.Where(x => x.Attribute2 == ItemHierarchyId.Id).ToList());
                    case CommonData.ItemHierarchyLevel.ItemHierarchyLevel3:
                        return _mapper.Map<List<ItemGroupModel>>(itemGroups.Where(x => x.Attribute3 == ItemHierarchyId.Id).ToList());
                    case CommonData.ItemHierarchyLevel.ItemHierarchyLevel4:
                        return _mapper.Map<List<ItemGroupModel>>(itemGroups.Where(x => x.Attribute4 == ItemHierarchyId.Id).ToList());
                    case CommonData.ItemHierarchyLevel.ItemHierarchyLevel5:
                        return _mapper.Map<List<ItemGroupModel>>(itemGroups.Where(x => x.Attribute5 == ItemHierarchyId.Id).ToList());
                    case CommonData.ItemHierarchyLevel.ItemHierarchyLevel6:
                        return _mapper.Map<List<ItemGroupModel>>(itemGroups.Where(x => x.Attribute6 == ItemHierarchyId.Id).ToList());
                    case CommonData.ItemHierarchyLevel.ItemHierarchyLevel7:
                        return _mapper.Map<List<ItemGroupModel>>(itemGroups.Where(x => x.Attribute7 == ItemHierarchyId.Id).ToList());
                    case CommonData.ItemHierarchyLevel.ItemHierarchyLevel8:
                        return _mapper.Map<List<ItemGroupModel>>(itemGroups.Where(x => x.Attribute8 == ItemHierarchyId.Id).ToList());
                    case CommonData.ItemHierarchyLevel.ItemHierarchyLevel9:
                        return _mapper.Map<List<ItemGroupModel>>(itemGroups.Where(x => x.Attribute9 == ItemHierarchyId.Id).ToList());
                    default:
                        return _mapper.Map<List<ItemGroupModel>>(itemGroups.Where(x => x.Attribute10 == ItemHierarchyId.Id).ToList());
                }
            }
            return new List<ItemGroupModel>();
        }
        #endregion

        #region Standar SKU
        public async Task<StandardSkuWithQuantity> GetStandardSkuWithQuantityBySku(string InventoryItemCode, string Uom, int quantity, string DistributorCode)
        {
            StandardSkuWithQuantity result = new StandardSkuWithQuantity();
            result.ItemCode = InventoryItemCode;
            result.UomCode = Uom;
            result.Available = quantity;
            var itemWithStocks = await _dbInvAllocationDetail.GetAllQueryable(x => x.ItemCode == InventoryItemCode && x.DistributorCode == DistributorCode).ToListAsync();
            var baseUom = await (from ii in _dbInventoryItem.GetAllQueryable()
                                 join iu in _dbItemUom.GetAllQueryable() on ii.Id equals iu.ItemId
                                 join u1 in _dbUom.GetAllQueryable() on iu.FromUnit equals u1.Id
                                 join u2 in _dbUom.GetAllQueryable() on iu.ToUnit equals u2.Id
                                 where ii.InventoryItemId == InventoryItemCode && u1.UomId == Uom
                                 select new InventoryUomModel()
                                 {
                                     InventoryId = ii.Id,
                                     InvenrotyCode = ii.InventoryItemId,
                                     ItemGroupCode = ii.GroupId,
                                     UomFromId = iu.FromUnit,
                                     UomFromCode = u1.UomId,
                                     UomFromDes = u1.Description,
                                     UomToId = iu.ToUnit,
                                     UomToCode = u2.UomId,
                                     UomToDes = u2.Description,
                                     ConversionFactor = (int)iu.ConversionFactor
                                 }).FirstOrDefaultAsync();
            var QuantityByBaseUom = quantity;
            if (baseUom != null)
            {
                QuantityByBaseUom = quantity * baseUom.ConversionFactor;
                result.ItemCode = baseUom.UomToCode;
            }

            if (itemWithStocks.Sum(x => x.Available) >= QuantityByBaseUom)
            {
                result.IsEnoughStock = true;
            }
            else
            {
                result.IsEnoughStock = false;
            }

            return result;
        }

        public async Task<List<StandardSkuWithQuantity>> GetStandardSkuWithQuantityByItemGroup(string ItemGroupCode, string Uom, int quantity, string DistributorCode)
        {
            List<StandardSkuWithQuantity> result = new List<StandardSkuWithQuantity>();
            var lstStandarSku = await (from sku in _dbStandardSku.GetAllQueryable()
                                       join skuItem in _dbStandardSkuItem.GetAllQueryable()
                                       on sku.Id equals skuItem.StandardId
                                       join igroup in _dbItemGroup.GetAllQueryable()
                                       on sku.ItemGroupId equals igroup.Id
                                       join item in _dbInventoryItem.GetAllQueryable()
                                       on skuItem.InventoryItemId equals item.Id
                                       where ((skuItem.EffectiveDateFrom == null || (skuItem.EffectiveDateFrom.HasValue && skuItem.EffectiveDateFrom <= DateTime.Now))
                                       && (skuItem.EndDate == null || (skuItem.EndDate.HasValue && skuItem.EndDate >= DateTime.Now)))
                                       select new StandardSkuModel()
                                       {
                                           ItemGroupCode = igroup.Code,
                                           InventoryCode = item.InventoryItemId,
                                           RuleType = sku.RuleType,
                                           Priority = skuItem.Priority,
                                           Ratio = skuItem.Ratio
                                       }).ToListAsync();

            var lstSkuStocks = await _dbInvAllocationDetail.GetAllQueryable(x => !x.IsDeleted && x.ItemGroupCode == ItemGroupCode && x.DistributorCode == DistributorCode).ToListAsync();

            var baseUom = await (from ii in _dbInventoryItem.GetAllQueryable()
                                 join iu in _dbItemUom.GetAllQueryable() on ii.Id equals iu.ItemId
                                 join u1 in _dbUom.GetAllQueryable() on iu.FromUnit equals u1.Id
                                 join u2 in _dbUom.GetAllQueryable() on iu.ToUnit equals u2.Id
                                 where ii.GroupId == ItemGroupCode && u1.UomId == Uom
                                 select new InventoryUomModel()
                                 {
                                     InventoryId = ii.Id,
                                     InvenrotyCode = ii.InventoryItemId,
                                     ItemGroupCode = ii.GroupId,
                                     UomFromId = iu.FromUnit,
                                     UomFromCode = u1.UomId,
                                     UomFromDes = u1.Description,
                                     UomToId = iu.ToUnit,
                                     UomToCode = u2.UomId,
                                     UomToDes = u2.Description,
                                     ConversionFactor = (int)iu.ConversionFactor
                                 }).FirstOrDefaultAsync();
            string baseUomCode = string.Empty;
            var QuantityByBaseUom = quantity;
            if (baseUom != null)
            {
                QuantityByBaseUom = quantity * baseUom.ConversionFactor;
                baseUomCode = baseUom.UomToCode;
            }

            // Phan bo deu
            if (lstStandarSku == null)
            {
                return EvenlyDistributeStockItem(lstSkuStocks, baseUomCode, QuantityByBaseUom, Uom);
            }
            else
            {
                // Priority
                if (lstStandarSku.FirstOrDefault().RuleType == CommonData.PriorityStandard.Priority)
                {
                    var lstStandarByPriority = lstStandarSku.OrderBy(x => x.Priority).ToList();
                    return PriorityDistributeStockItem(lstStandarByPriority, lstSkuStocks, baseUomCode, QuantityByBaseUom, Uom);
                }
                // Priority by time
                else if (lstStandarSku.FirstOrDefault().RuleType == CommonData.PriorityStandard.PriorityByTime)
                {
                    var lstStandarByPriorityByTime = lstStandarSku.OrderBy(x => x.Priority).ToList();
                    return PriorityDistributeStockItem(lstStandarByPriorityByTime, lstSkuStocks, baseUomCode, QuantityByBaseUom, Uom);
                }
                // Ratio
                else
                {
                    var lstStandarByRatio = lstStandarSku.OrderBy(x => x.Ratio).ToList();
                    // Bussiness Rounding
                    string RoundingRule = string.Empty;
                    var PoParam = _dbPoRpoparameter.FirstOrDefault();
                    if (PoParam != null && !string.IsNullOrEmpty(PoParam.RoundingRule))
                    {
                        RoundingRule = PoParam.RoundingRule;
                    }
                    else
                    {
                        RoundingRule = CommonData.SystemSetting.RoundingRuleBusiness;
                    }

                    return RatioDistributeStockItem(lstStandarByRatio, lstSkuStocks, baseUomCode, QuantityByBaseUom, RoundingRule, Uom);
                }
            }
        }

        public async Task<List<StandardSkuWithQuantity>> GetStandardSkuWithQuantityByItemHierarchy(string ItemLevel, string ItemHierarchyValue, string Uom, int quantity, string DistributorCode)
        {
            var lstItemGroupByHierarchys = ExternalGetListItemGroupByItemHierarchy(ItemLevel, ItemHierarchyValue);

            if (lstItemGroupByHierarchys != null)
            {
                return await GetStandardSkuWithQuantityByItemGroup(lstItemGroupByHierarchys.FirstOrDefault().Code, Uom, quantity, DistributorCode);
            }
            return new List<StandardSkuWithQuantity>();
        }

        private List<StandardSkuWithQuantity> EvenlyDistributeStockItem(List<InvAllocationDetail> lstSkuStocks, string baseUom, int QuantityByBaseUom, string Uom)
        {
            List<StandardSkuWithQuantity> result = new List<StandardSkuWithQuantity>();
            int sumQuatity = 0;
            int number = 0;
            var lstSku = lstSkuStocks.OrderBy(x => x.Available).Select(y => y.ItemCode).Distinct().ToList();

            foreach (var item in lstSku)
            {
                StandardSkuWithQuantity itemSku = new StandardSkuWithQuantity();
                itemSku.ItemCode = item;
                //itemSku.BaseUom = baseUom;
                itemSku.UomCode = Uom;
                itemSku.Available = 0;
                itemSku.IsEnoughStock = false;

                double tb = Convert.ToDouble(QuantityByBaseUom - sumQuatity) / (lstSkuStocks.Count - number);
                var stockItems = lstSkuStocks.Where(x => x.ItemCode.Equals(item));

                if (stockItems != null)
                {
                    if (stockItems.Sum(x => x.Available) <= tb)
                    {
                        itemSku.Available = stockItems.Sum(x => x.Available);
                    }
                    else
                    {
                        itemSku.Available = GetAvaiableByCeiling(tb);
                    }
                    sumQuatity += itemSku.Available;
                    number++;
                }

                result.Add(itemSku);
            }

            return result;
        }

        private List<StandardSkuWithQuantity> PriorityDistributeStockItem(List<StandardSkuModel> lstStandardSku, List<InvAllocationDetail> lstSkuStocks, string baseUom, int QuantityByBaseUom, string Uom)
        {
            List<StandardSkuWithQuantity> result = new List<StandardSkuWithQuantity>();
            int sumQuatity = 0;

            foreach (var item in lstStandardSku)
            {
                StandardSkuWithQuantity itemSku = new StandardSkuWithQuantity();
                itemSku.ItemCode = item.InventoryCode;
                //itemSku.BaseUom = baseUom;
                itemSku.UomCode = Uom;
                itemSku.Available = 0;
                itemSku.IsEnoughStock = false;

                if (sumQuatity >= QuantityByBaseUom)
                {
                    return result;
                }
                else
                {
                    var stockItems = lstSkuStocks.Where(x => x.ItemCode.Equals(item.InventoryCode));
                    if (stockItems != null)
                    {
                        int newQuantity = QuantityByBaseUom - sumQuatity;
                        if (stockItems.Sum(x => x.Available) >= newQuantity)
                        {
                            itemSku.Available = newQuantity;
                        }
                        else
                        {
                            itemSku.Available = stockItems.Sum(x => x.Available);
                        }
                    }
                    sumQuatity += itemSku.Available;
                }

                result.Add(itemSku);
            }


            //// Check IsEnoughStock
            //if (true)
            //{

            //}

            return result;
        }

        private List<StandardSkuWithQuantity> RatioDistributeStockItem(List<StandardSkuModel> lstStandardSku, List<InvAllocationDetail> lstSkuStocks, string baseUom, int QuantityByBaseUom, string RoundingRule, string Uom)
        {
            List<StandardSkuWithQuantity> result = new List<StandardSkuWithQuantity>();
            int sumQuatity = 0;
            int sumRatio = 100;

            foreach (var item in lstStandardSku)
            {
                StandardSkuWithQuantity itemSku = new StandardSkuWithQuantity();
                itemSku.ItemCode = item.InventoryCode;
                //itemSku.BaseUom = baseUom;
                itemSku.UomCode = Uom;
                itemSku.Available = 0;
                itemSku.IsEnoughStock = false;

                if (sumQuatity >= QuantityByBaseUom)
                {
                    return result;
                }
                else
                {
                    var stockItems = lstSkuStocks.Where(x => x.ItemCode.Equals(item.InventoryCode));
                    if (stockItems != null)
                    {
                        int RemainingQuantity = QuantityByBaseUom - sumQuatity;
                        var tempData = GetAvaiableByRoundingRule(Convert.ToDouble(RemainingQuantity * item.Ratio) / sumRatio, RoundingRule);

                        if (stockItems.Sum(x => x.Available) > tempData)
                        {
                            itemSku.Available = tempData;
                        }
                        else
                        {
                            itemSku.Available = stockItems.Sum(x => x.Available);
                        }
                    }
                    sumQuatity += itemSku.Available;
                }
                result.Add(itemSku);
            }

            return result;
        }

        private int GetAvaiableByCeiling(double avaiable)
        {
            return Convert.ToInt32(Math.Floor(avaiable));
        }

        private int GetAvaiableByRoundingRule(double avaiable, string RoundingRule)
        {
            int result = 0;
            if (RoundingRule.ToLower().Equals(CommonData.SystemSetting.RoundingRuleBusiness.ToLower()))
            {
                result = Convert.ToInt32(Math.Ceiling(avaiable));
            }
            else
            {
                result = Convert.ToInt32(Math.Round(avaiable));
            }
            return result;
        }
        #endregion

        #region Promotion Result
        public async Task<PromotionProductForSaleModel> ExternalApiGetProductForSale(PromotionResultRequestModel request)
        {
            PromotionProductForSaleModel result = new PromotionProductForSaleModel();

            var promotionInfo = await _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0 && x.Code == request.PromotionCode).FirstOrDefaultAsync();
            var levelInfo = await _dbPromotionDefinitionStructure.GetAllQueryable(x => x.PromotionCode == request.PromotionCode && x.LevelCode == request.LevelCode).FirstOrDefaultAsync();
            var lstProductForSales = await _dbPromotionDefinitionProductForSale.GetAllQueryable(x => x.PromotionCode == request.PromotionCode && x.LevelCode == request.LevelCode).ToListAsync();

            if (promotionInfo == null || levelInfo == null)
            {
                return result;
            }
            else
            {
                if (promotionInfo.PromotionType == CommonData.PromotionSetting.PromotionByProduct || promotionInfo.PromotionType == CommonData.PromotionSetting.ProductGroups)
                {
                    if (promotionInfo.PromotionCheckBy)
                    {
                        var quantity = levelInfo.QuantityPurchased;

                        if (levelInfo.ProductTypeForGift == CommonData.PromotionSetting.SKU)
                        {
                            foreach (var item in lstProductForSales)
                            {
                                var product = await GetStandardSkuWithQuantityBySku(item.ProductCode, item.Packing, request.NumberOfPurchases * quantity, request.DistributorCode);
                                result.productDetails.Add(product);
                            }
                        }
                        else if (levelInfo.ProductTypeForGift == CommonData.PromotionSetting.ItemGroup)
                        {
                            foreach (var item in lstProductForSales)
                            {
                                var products = await GetStandardSkuWithQuantityByItemGroup(item.ProductCode, item.Packing, request.NumberOfPurchases * quantity, request.DistributorCode);
                                result.productDetails.AddRange(products);
                            }
                        }
                        else
                        {
                            foreach (var item in lstProductForSales)
                            {
                                var products = await GetStandardSkuWithQuantityByItemHierarchy(item.ProductCode, item.ItemHierarchyLevelForSale, item.Packing, request.NumberOfPurchases * quantity, request.DistributorCode);
                                result.productDetails.AddRange(products);
                            }
                        }
                    }
                    else
                    {
                        // Call API get price
                    }
                }
                else
                {
                    var quantity = request.NumberOfPurchases;

                    if (levelInfo.ProductTypeForGift == CommonData.PromotionSetting.SKU)
                    {
                        foreach (var item in lstProductForSales)
                        {
                            var product = await GetStandardSkuWithQuantityBySku(item.ProductCode, item.Packing, item.SellNumber * quantity, request.DistributorCode);
                            result.productDetails.Add(product);
                        }
                    }
                    else if (levelInfo.ProductTypeForGift == CommonData.PromotionSetting.ItemGroup)
                    {
                        foreach (var item in lstProductForSales)
                        {
                            var products = await GetStandardSkuWithQuantityByItemGroup(item.ProductCode, item.Packing, item.SellNumber * quantity, request.DistributorCode);
                            result.productDetails.AddRange(products);
                        }
                    }
                    else
                    {
                        foreach (var item in lstProductForSales)
                        {
                            var products = await GetStandardSkuWithQuantityByItemHierarchy(item.ProductCode, item.ItemHierarchyLevelForSale, item.Packing, item.SellNumber * quantity, request.DistributorCode);
                            result.productDetails.AddRange(products);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<PromotionProductForGift> ExternalApiGetProductForGift(PromotionResultRequestModel request)
        {
            PromotionProductForGift result = new PromotionProductForGift();
            result.productForGiftDetails = new List<ProductForGift>();

            var promotionInfo = await _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0 && x.Code == request.PromotionCode).FirstOrDefaultAsync();
            var levelInfo = await _dbPromotionDefinitionStructure.GetAllQueryable(x => x.PromotionCode == request.PromotionCode && x.LevelCode == request.LevelCode).FirstOrDefaultAsync();
            var lstProductForGifts = await _dbPromotionDefinitionProductForGift.GetAllQueryable(x => x.PromotionCode == request.PromotionCode && x.LevelCode == request.LevelCode).ToListAsync();

            if (promotionInfo == null || levelInfo == null)
            {
                return result;
            }
            else
            {

                var CustomerScope = await GetCustomerShiptoWithScope(request.CustomerCode, request.ShiptoCode, request.RouteZoneCode);
                int TypeOfReward = 0;

                if (levelInfo.IsGiftProduct)
                {
                    TypeOfReward = 1;

                    if (levelInfo.ProductTypeForGift == CommonData.PromotionSetting.SKU)
                    {
                        foreach (var item in lstProductForGifts)
                        {
                            var product = await GetStandardSkuWithQuantityBySku(item.ProductCode, item.Packing, item.NumberOfGift * request.NumberOfPurchases, request.DistributorCode);
                            var IsEnoughBg = await IsEnoughBudget(item.BudgetCode, item.NumberOfGift, 0, promotionInfo.SaleOrg,
                                CustomerScope.Branch, CustomerScope.Region, CustomerScope.SubRegion, CustomerScope.Area, CustomerScope.SubArea, request.CustomerCode, request.ShiptoCode);
                            if (IsEnoughBg)
                            {
                                product.IsEnoughBudget = true;
                                product.UomCode = item.Packing;
                            }
                            var proForGift = new ProductForGift();
                            proForGift.IsDefault = item.IsDefaultProduct;
                            proForGift.Exchange = item.Exchange;
                            proForGift.productByCodeDetails.Add(product);
                            result.productForGiftDetails.Add(proForGift);
                        }
                    }
                    else if (levelInfo.ProductTypeForGift == CommonData.PromotionSetting.ItemGroup)
                    {
                        foreach (var item in lstProductForGifts)
                        {
                            var products = await GetStandardSkuWithQuantityByItemGroup(item.ProductCode, item.Packing, item.NumberOfGift * request.NumberOfPurchases, request.DistributorCode);
                            var IsEnoughBg = await IsEnoughBudget(item.BudgetCode, item.NumberOfGift, 0, promotionInfo.SaleOrg,
                            CustomerScope.Branch, CustomerScope.Region, CustomerScope.SubRegion, CustomerScope.Area, CustomerScope.SubArea, request.CustomerCode, request.ShiptoCode);
                            if (IsEnoughBg)
                            {
                                products.ForEach(x => x.IsEnoughBudget = true);
                            }
                            var ProductForGift = new ProductForGift();
                            ProductForGift.IsDefault = item.IsDefaultProduct;
                            ProductForGift.Exchange = item.Exchange;
                            ProductForGift.productByCodeDetails.AddRange(products);
                            result.productForGiftDetails.Add(ProductForGift);
                        }
                    }
                    else
                    {
                        foreach (var item in lstProductForGifts)
                        {
                            var products = await GetStandardSkuWithQuantityByItemHierarchy(item.ProductCode, item.ItemHierarchyLevelForGift, item.Packing, item.NumberOfGift * request.NumberOfPurchases, request.DistributorCode);
                            var IsEnoughBg = await IsEnoughBudget(item.BudgetCode, item.NumberOfGift, 0, promotionInfo.SaleOrg,
                            CustomerScope.Branch, CustomerScope.Region, CustomerScope.SubRegion, CustomerScope.Area, CustomerScope.SubArea, request.CustomerCode, request.ShiptoCode);
                            if (IsEnoughBg)
                            {
                                products.ForEach(x => x.IsEnoughBudget = true);
                            }
                            var ProductForGift = new ProductForGift();
                            ProductForGift.IsDefault = item.IsDefaultProduct;
                            ProductForGift.Exchange = item.Exchange;
                            ProductForGift.productByCodeDetails.AddRange(products);
                            result.productForGiftDetails.Add(ProductForGift);
                        }
                    }
                }

                if (levelInfo.IsDonate)
                {
                    TypeOfReward = 2;
                    if (levelInfo.IsFixMoney)
                    {
                        result.AmountOfDonation = request.NumberOfPurchases * levelInfo.AmountOfDonation;
                    }
                    else
                    {
                        result.AmountOfDonation = request.NumberOfPurchases * (levelInfo.ValuePurchased * (decimal)levelInfo.PercentageOfAmount);
                    }

                    var IsEnoughBg = await IsEnoughBudget(levelInfo.BudgetForDonation, 0, result.AmountOfDonation, promotionInfo.SaleOrg,
                                CustomerScope.Branch, CustomerScope.Region, CustomerScope.SubRegion, CustomerScope.Area, CustomerScope.SubArea, request.CustomerCode, request.ShiptoCode);
                    if (IsEnoughBg)
                    {
                        result.IsEnoughBudgetAmount = true;
                    }
                }

                if (levelInfo.RuleOfGiving && levelInfo.IsDonate)
                {
                    TypeOfReward = 3;
                }
                result.TypeOfReward = TypeOfReward;
            }
            return result;
        }

        private async Task<bool> IsEnoughBudget(string budgetCode, int quantity, decimal Amount, string SaleOrg,
            string BranchCode, string RegionCode, string SubRegionCode, string AreaCode, string SubAreaCode, string CustomerCode, string ShiptoCode)
        {
            var budgetInfo = await (from b in _dbPromotionBudget.GetAllQueryable()
                                    join bd in _dpTpBudgetDefine.GetAllQueryable()
                                    on b.Code equals bd.BudgetCode
                                    where b.Code == budgetCode
                                    select new BudgetForCheckModel()
                                    {
                                        Code = b.Code,
                                        SaleOrg = b.SaleOrg,
                                        BudgetType = b.BudgetType,
                                        BudgetAllocationForm = b.BudgetAllocationForm,
                                        BudgetAllocationLevel = b.BudgetAllocationLevel,
                                        FlagOverBudget = b.FlagOverBudget,
                                        Status = b.Status,
                                        PromotionProductType = bd.PromotionProductType,
                                        PromotionProductCode = bd.PromotionProductCode,
                                        PackSize = bd.PackSize,
                                        BudgetQuantity = bd.BudgetQuantity,
                                        BudgetQuantityUsed = bd.BudgetQuantityUsed,
                                        ItemHierarchyLevel = bd.ItemHierarchyLevel,
                                        ItemHierarchyValue = bd.ItemHierarchyValue,
                                        TotalAmountAllotment = bd.TotalAmountAllotment
                                    }).FirstOrDefaultAsync();

            if (budgetInfo != null)
            {
                var lstBudgetAllotments = await _dbTpBudgetAllotment.GetAllQueryable(x => x.BudgetCode == budgetCode).ToListAsync();
                budgetInfo.BudgetAllotmentModels = new List<TpBudgetAllotment>();
                budgetInfo.BudgetAllotmentModels.AddRange(lstBudgetAllotments);

                var budgetUseds = await _dbBudgetUsed.GetAllQueryable(x => x.BudgetCode == budgetCode).ToListAsync();

                if (budgetInfo.BudgetType == CommonData.BudgetForTradePromotion.AccordingToTheSalesTeam)
                {
                    if (budgetInfo.BudgetType == CommonData.BudgetForTradePromotion.Commodity)
                    {
                        var totalQuantityUsed = budgetUseds.Sum(x => x.QuantityUsed);
                        decimal totalQuantityUsedByCustomerByScope = 0;
                        decimal totalQuantityBudget = 0;
                        if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.Branch)
                        {
                            totalQuantityUsedByCustomerByScope = budgetUseds.Where(x => x.SaleOrgCode == SaleOrg && x.BranchCode == BranchCode).Sum(x => x.QuantityUsed);
                            totalQuantityBudget = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == BranchCode).Sum(x => x.BudgetQuantityDetail);
                        }
                        else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.Region)
                        {
                            totalQuantityUsedByCustomerByScope = budgetUseds.Where(x => x.SaleOrgCode == SaleOrg && x.RegionCode == RegionCode).Sum(x => x.QuantityUsed);
                            totalQuantityBudget = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == RegionCode).Sum(x => x.BudgetQuantityDetail);
                        }
                        else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.SubRegion)
                        {
                            totalQuantityUsedByCustomerByScope = budgetUseds.Where(x => x.SaleOrgCode == SaleOrg && x.SubRegionCode == SubRegionCode).Sum(x => x.QuantityUsed);
                            totalQuantityBudget = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == SubRegionCode).Sum(x => x.BudgetQuantityDetail);
                        }
                        else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.Area)
                        {
                            totalQuantityUsedByCustomerByScope = budgetUseds.Where(x => x.SaleOrgCode == SaleOrg && x.AreaCode == AreaCode).Sum(x => x.QuantityUsed);
                            totalQuantityBudget = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == AreaCode).Sum(x => x.BudgetQuantityDetail);
                        }
                        else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.SubArea)
                        {
                            totalQuantityUsedByCustomerByScope = budgetUseds.Where(x => x.SaleOrgCode == SaleOrg && x.SubAreaCode == SubAreaCode).Sum(x => x.QuantityUsed);
                            totalQuantityBudget = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == SubAreaCode).Sum(x => x.BudgetQuantityDetail);
                        }

                        totalQuantityUsed = totalQuantityUsed + quantity;
                        totalQuantityUsedByCustomerByScope = totalQuantityUsedByCustomerByScope + quantity;

                        if (budgetInfo.BudgetQuantity <= totalQuantityUsed || totalQuantityBudget <= totalQuantityUsedByCustomerByScope)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        var totalMoneyUsed = budgetUseds.Sum(x => x.AmountUsed);
                        decimal totalMoneyUsedByCustomerByScope = 0;
                        decimal totalMoneyBudget = 0;
                        if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.Branch)
                        {
                            totalMoneyUsedByCustomerByScope = budgetUseds.Where(x => x.SaleOrgCode == SaleOrg && x.BranchCode == BranchCode).Sum(x => x.AmountUsed);
                            totalMoneyBudget = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == BranchCode).Sum(x => x.BudgetQuantityDetail);
                        }
                        else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.Region)
                        {
                            totalMoneyUsedByCustomerByScope = budgetUseds.Where(x => x.SaleOrgCode == SaleOrg && x.RegionCode == RegionCode).Sum(x => x.AmountUsed);
                            totalMoneyBudget = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == RegionCode).Sum(x => x.BudgetQuantityDetail);
                        }
                        else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.SubRegion)
                        {
                            totalMoneyUsedByCustomerByScope = budgetUseds.Where(x => x.SaleOrgCode == SaleOrg && x.SubRegionCode == SubRegionCode).Sum(x => x.AmountUsed);
                            totalMoneyBudget = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == SubRegionCode).Sum(x => x.BudgetQuantityDetail);
                        }
                        else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.Area)
                        {
                            totalMoneyUsedByCustomerByScope = budgetUseds.Where(x => x.SaleOrgCode == SaleOrg && x.AreaCode == AreaCode).Sum(x => x.AmountUsed);
                            totalMoneyBudget = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == AreaCode).Sum(x => x.BudgetQuantityDetail);
                        }
                        else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.SubArea)
                        {
                            totalMoneyUsedByCustomerByScope = budgetUseds.Where(x => x.SaleOrgCode == SaleOrg && x.SubAreaCode == SubAreaCode).Sum(x => x.AmountUsed);
                            totalMoneyBudget = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == SubAreaCode).Sum(x => x.BudgetQuantityDetail);
                        }

                        totalMoneyUsed = totalMoneyUsed + Amount;
                        totalMoneyUsedByCustomerByScope = totalMoneyUsedByCustomerByScope + Amount;

                        if (budgetInfo.TotalAmountAllotment <= totalMoneyUsed || totalMoneyBudget <= totalMoneyUsedByCustomerByScope)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (budgetInfo.BudgetType == CommonData.BudgetForTradePromotion.Commodity)
                    {
                        var totalQuantityUsed = budgetUseds.Sum(x => x.QuantityUsed);
                        var totalQuantityUsedByCustomer = budgetUseds.Where(x => x.CustomerCode == CustomerCode && x.ShiptoCode == ShiptoCode).Sum(x => x.QuantityUsed);
                        var totalMoneyBudget = budgetInfo.BudgetAllotmentModels.Sum(x => x.BudgetQuantityDetail);

                        totalQuantityUsed = totalQuantityUsed + quantity;
                        totalQuantityUsedByCustomer = totalQuantityUsedByCustomer + quantity;

                        if (budgetInfo.BudgetQuantity <= totalQuantityUsed || totalMoneyBudget <= totalQuantityUsedByCustomer)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        var totalMoneyUsed = budgetUseds.Sum(x => x.AmountUsed);
                        var totalMoneyUsedByCustomer = budgetUseds.Where(x => x.CustomerCode == CustomerCode && x.ShiptoCode == ShiptoCode).Sum(x => x.AmountUsed);
                        var totalMoneyBudget = budgetInfo.BudgetAllotmentModels.Sum(x => x.BudgetQuantityDetail);

                        totalMoneyUsed = totalMoneyUsed + Amount;
                        totalMoneyUsedByCustomer = totalMoneyUsedByCustomer + Amount;

                        if (budgetInfo.TotalAmountAllotment <= totalMoneyUsed || totalMoneyBudget <= totalMoneyUsedByCustomer)
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        #endregion

        public async Task<PromotionBudgetResponse> ExtenalApiCheckBudgetInfoPromotion(PromotionBudgetRequest request)
        {
            PromotionBudgetResponse result = new PromotionBudgetResponse();

            var promotionStructure = await _dbPromotionDefinitionStructure.GetAllQueryable(x =>
                                        x.PromotionCode == request.PromotionCode && x.LevelCode == request.PromotionLevel).FirstOrDefaultAsync();

            if (promotionStructure != null)
            {
                if (promotionStructure.IsApplyBudget)
                {
                    if (promotionStructure.IsGiftProduct)
                    {
                        var lstProductForGifts = await _dbPromotionDefinitionProductForGift.GetAllQueryable(x => x.PromotionCode == request.PromotionCode
                                                    && x.LevelCode == request.PromotionLevel).ToListAsync();

                        string budgetCode = string.Empty;
                        if (promotionStructure.ProductTypeForGift == CommonData.PromotionSetting.SKU)
                        {
                            budgetCode = lstProductForGifts.FirstOrDefault(x => x.ProductCode == request.ProductSkuCode).BudgetCode;
                        }
                        else if (promotionStructure.ProductTypeForGift == CommonData.PromotionSetting.ItemGroup)
                        {
                            var itemGroupCode = _dbInventoryItem.GetAllQueryable(x => x.InventoryItemId == request.ProductSkuCode).FirstOrDefault().GroupId;
                            budgetCode = lstProductForGifts.FirstOrDefault(x => x.ProductCode == itemGroupCode).BudgetCode;
                        }
                        else if (promotionStructure.ProductTypeForGift == CommonData.PromotionSetting.ItemHierarchyValue)
                        {
                            foreach (var product in lstProductForGifts)
                            {
                                var InventoryItem = GetInventoryItemByItemHierarchy(product.ItemHierarchyLevelForGift, product.ProductCode);
                                if (InventoryItem.InventoryItemId == request.ProductSkuCode)
                                {
                                    budgetCode = product.BudgetCode;
                                }
                            }
                        }

                        var budgetInfo = await GetBudgetInfo(budgetCode);
                        result.FlagOverBudgetForProduct = budgetInfo.FlagOverBudget;
                        var budgetUseds = await _dbBudgetUsed.GetAllQueryable(x => x.BudgetCode == budgetCode).ToListAsync();

                        if (budgetInfo != null)
                        {
                            var lstBudgetAllotments = await _dbTpBudgetAllotment.GetAllQueryable(x => x.BudgetCode == budgetCode).ToListAsync();
                            budgetInfo.BudgetAllotmentModels = new List<TpBudgetAllotment>();
                            budgetInfo.BudgetAllotmentModels.AddRange(lstBudgetAllotments);

                            if (budgetInfo.BudgetAllocationForm == CommonData.BudgetForTradePromotion.AccordingToTheSalesTeam)
                            {
                                if (budgetInfo.BudgetType == CommonData.BudgetForTradePromotion.Commodity)
                                {
                                    var budgetUseByCustomer = new List<TpBudgetUsed>();
                                    var budgetDefineByCustomer = new List<TpBudgetAllotment>();

                                    decimal totalBudgetDefine = budgetInfo.BudgetQuantity;
                                    decimal totalQuantityUsed = budgetUseds.Sum(x => x.QuantityUsed);

                                    decimal totalBudgetDefineByScope = 0;
                                    decimal totalBudgetUsedByScope = 0;

                                    decimal totalBudgetDefineByCustomer = 0;
                                    decimal totalBudgetUsedByCustomer = 0;

                                    if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.Branch)
                                    {
                                        budgetDefineByCustomer = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == request.BranchCode).ToList();
                                        budgetUseByCustomer = budgetUseds.Where(x => x.SaleOrgCode == request.SaleOrg && x.BranchCode == request.BranchCode).ToList();
                                        totalBudgetDefineByScope = budgetDefineByCustomer.Sum(x => x.BudgetQuantityDetail);
                                        totalBudgetUsedByScope = budgetUseByCustomer.Sum(x => x.QuantityUsed);
                                    }
                                    else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.Region)
                                    {
                                        budgetDefineByCustomer = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == request.RegionCode).ToList();
                                        budgetUseByCustomer = budgetUseds.Where(x => x.SaleOrgCode == request.SaleOrg && x.RegionCode == request.RegionCode).ToList();
                                        totalBudgetDefineByScope = budgetDefineByCustomer.Sum(x => x.BudgetQuantityDetail);
                                        totalBudgetUsedByScope = budgetUseByCustomer.Sum(x => x.QuantityUsed);
                                    }
                                    else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.SubRegion)
                                    {
                                        budgetDefineByCustomer = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == request.SubRegionCode).ToList();
                                        budgetUseByCustomer = budgetUseds.Where(x => x.SaleOrgCode == request.SaleOrg && x.SubRegionCode == request.SubRegionCode).ToList();
                                        totalBudgetDefineByScope = budgetDefineByCustomer.Sum(x => x.BudgetQuantityDetail);
                                        totalBudgetUsedByScope = budgetUseByCustomer.Sum(x => x.QuantityUsed);
                                    }
                                    else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.Area)
                                    {
                                        budgetDefineByCustomer = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == request.AreaCode).ToList();
                                        budgetUseByCustomer = budgetUseds.Where(x => x.SaleOrgCode == request.SaleOrg && x.AreaCode == request.AreaCode).ToList();
                                        totalBudgetDefineByScope = budgetDefineByCustomer.Sum(x => x.BudgetQuantityDetail);
                                        totalBudgetUsedByScope = budgetUseByCustomer.Sum(x => x.QuantityUsed);
                                    }
                                    else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.SubArea)
                                    {
                                        budgetDefineByCustomer = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == request.SubAreaCode).ToList();
                                        budgetUseByCustomer = budgetUseds.Where(x => x.SaleOrgCode == request.SaleOrg && x.SubAreaCode == request.SubAreaCode).ToList();
                                        totalBudgetDefineByScope = budgetDefineByCustomer.Sum(x => x.BudgetQuantityDetail);
                                        totalBudgetUsedByScope = budgetUseByCustomer.Sum(x => x.QuantityUsed);
                                    }

                                    result.SumProductQuantityUsed = totalBudgetUsedByScope;
                                    result.SumProductQuantityUsedByCustomer = budgetUseByCustomer.Where(x => x.CustomerCode == request.CustomerCode
                                                                    && x.ShiptoCode == request.CustomerShiptoCode).Sum(x => x.QuantityUsed);

                                    // Check over Budget by Scope
                                    if (totalBudgetDefineByScope < (result.SumProductQuantityUsed + request.ProductNumber))
                                    {
                                        result.OverProductQuanty = true;
                                    }
                                    else
                                    {
                                        result.OverProductQuanty = false;
                                    }

                                    // Check over Budget By Customer
                                    var FlagBudgetQuantityLimitDetail = budgetDefineByCustomer.FirstOrDefault().FlagBudgetQuantityLimitDetail;
                                    var BudgetQuantityLimitDetail = budgetDefineByCustomer.FirstOrDefault().BudgetQuantityLimitDetail;
                                    if (FlagBudgetQuantityLimitDetail && BudgetQuantityLimitDetail > 0)
                                    {
                                        totalBudgetDefineByCustomer = BudgetQuantityLimitDetail;
                                        if (BudgetQuantityLimitDetail < (result.SumProductQuantityUsedByCustomer + request.ProductNumber))
                                        {
                                            result.OverProductQuantyByCustomer = true;
                                        }
                                        else
                                        {
                                            result.OverProductQuantyByCustomer = false;
                                        }
                                    }

                                    if (((totalBudgetDefine >= (totalQuantityUsed + request.ProductNumber)) && (totalBudgetDefineByScope >= (totalBudgetUsedByScope + request.ProductNumber)) && !FlagBudgetQuantityLimitDetail)
                                        || ((totalBudgetDefine >= (totalQuantityUsed + request.ProductNumber)) && (totalBudgetDefineByScope >= (totalBudgetUsedByScope + request.ProductNumber)) && (totalBudgetDefineByCustomer) >= (totalBudgetUsedByCustomer + request.ProductNumber)))
                                    {
                                        result.IsEnoughBudgetForProduct = true;
                                    }
                                    else
                                    {
                                        result.IsEnoughBudgetForProduct = false;
                                    }
                                    result.BudgetForProductInfo = budgetInfo;
                                }
                            }
                            else
                            {
                                if (budgetInfo.BudgetType == CommonData.BudgetForTradePromotion.Commodity)
                                {
                                    decimal totalBudgetDefine = budgetInfo.TotalAmountAllotment;
                                    decimal totalQuantityUsed = budgetUseds.Sum(x => x.QuantityUsed);

                                    decimal totalBudgetDefineByCustomer = budgetInfo.BudgetAllotmentModels.FirstOrDefault().BudgetQuantityDetail;
                                    decimal totalQuantityUsedByCustomer = budgetUseds.Where(x => x.CustomerCode == request.CustomerCode
                                                                            && x.ShiptoCode == request.CustomerShiptoCode).Sum(x => x.QuantityUsed);

                                    if ((totalBudgetDefine >= (totalQuantityUsed + request.AmountOfDonation))
                                        && (totalBudgetDefineByCustomer) >= (totalQuantityUsedByCustomer + request.AmountOfDonation))
                                    {
                                        result.IsEnoughBudgetForProduct = true;
                                    }
                                    else
                                    {
                                        result.IsEnoughBudgetForProduct = false;
                                    }

                                    result.SumProductQuantityUsed = totalQuantityUsedByCustomer;
                                    result.SumProductQuantityUsedByCustomer = totalQuantityUsedByCustomer;
                                    result.OverProductQuanty = false;
                                    result.OverProductQuantyByCustomer = false;
                                    result.BudgetForProductInfo = budgetInfo;
                                }
                            }
                        }
                    }

                    if (promotionStructure.IsDonate)
                    {
                        var budgetInfo = await GetBudgetInfo(promotionStructure.BudgetForDonation);
                        result.FlagOverBudgetForAmount = budgetInfo.FlagOverBudget;
                        var budgetUseds = await _dbBudgetUsed.GetAllQueryable(x => x.BudgetCode == promotionStructure.BudgetForDonation).ToListAsync();

                        if (budgetInfo != null)
                        {
                            var lstBudgetAllotments = await _dbTpBudgetAllotment.GetAllQueryable(x => x.BudgetCode == promotionStructure.BudgetForDonation).ToListAsync();
                            budgetInfo.BudgetAllotmentModels = new List<TpBudgetAllotment>();
                            budgetInfo.BudgetAllotmentModels.AddRange(lstBudgetAllotments);

                            if (budgetInfo.BudgetAllocationForm == CommonData.BudgetForTradePromotion.AccordingToTheSalesTeam)
                            {
                                if (budgetInfo.BudgetType == CommonData.BudgetForTradePromotion.Money)
                                {
                                    var budgetUseByCustomer = new List<TpBudgetUsed>();
                                    var budgetDefineByCustomer = new List<TpBudgetAllotment>();

                                    decimal totalBudgetDefine = budgetInfo.TotalAmountAllotment;
                                    decimal totalMoneyUsed = budgetUseds.Sum(x => x.AmountUsed);

                                    decimal totalMoneyBudgetDefineByScope = 0;
                                    decimal totalMoneyBudgetUsedByScope = 0;

                                    decimal totalBudgetDefineByCustomer = 0;
                                    decimal totalBudgetUsedByCustomer = 0;

                                    if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.Branch)
                                    {
                                        budgetDefineByCustomer = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == request.BranchCode).ToList();
                                        budgetUseByCustomer = budgetUseds.Where(x => x.SaleOrgCode == request.SaleOrg && x.BranchCode == request.BranchCode).ToList();
                                        totalMoneyBudgetDefineByScope = budgetDefineByCustomer.Sum(x => x.BudgetQuantityDetail);
                                        totalMoneyBudgetUsedByScope = budgetUseByCustomer.Sum(x => x.AmountUsed);

                                    }
                                    else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.Region)
                                    {
                                        budgetDefineByCustomer = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == request.RegionCode).ToList();
                                        budgetUseByCustomer = budgetUseds.Where(x => x.SaleOrgCode == request.SaleOrg && x.RegionCode == request.RegionCode).ToList();
                                        totalMoneyBudgetDefineByScope = budgetDefineByCustomer.Sum(x => x.BudgetQuantityDetail);
                                        totalMoneyBudgetUsedByScope = budgetUseByCustomer.Sum(x => x.AmountUsed);

                                    }
                                    else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.SubRegion)
                                    {
                                        budgetDefineByCustomer = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == request.SubRegionCode).ToList();
                                        budgetUseByCustomer = budgetUseds.Where(x => x.SaleOrgCode == request.SaleOrg && x.SubRegionCode == request.SubRegionCode).ToList();
                                        totalMoneyBudgetDefineByScope = budgetDefineByCustomer.Sum(x => x.BudgetQuantityDetail);
                                        totalMoneyBudgetUsedByScope = budgetUseByCustomer.Sum(x => x.AmountUsed);
                                    }
                                    else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.Area)
                                    {
                                        budgetDefineByCustomer = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == request.AreaCode).ToList();
                                        budgetUseByCustomer = budgetUseds.Where(x => x.SaleOrgCode == request.SaleOrg && x.AreaCode == request.AreaCode).ToList();
                                        totalMoneyBudgetDefineByScope = budgetDefineByCustomer.Sum(x => x.BudgetQuantityDetail);
                                        totalMoneyBudgetUsedByScope = budgetUseByCustomer.Sum(x => x.AmountUsed);
                                    }
                                    else if (budgetInfo.BudgetAllocationLevel == CommonData.TerritoryLevelSetting.SubArea)
                                    {
                                        budgetDefineByCustomer = budgetInfo.BudgetAllotmentModels.Where(x => x.SalesTerritoryValueCode == request.SubAreaCode).ToList();
                                        budgetUseByCustomer = budgetUseds.Where(x => x.SaleOrgCode == request.SaleOrg && x.SubAreaCode == request.SubAreaCode).ToList();
                                        totalMoneyBudgetDefineByScope = budgetDefineByCustomer.Sum(x => x.BudgetQuantityDetail);
                                        totalMoneyBudgetUsedByScope = budgetUseByCustomer.Sum(x => x.AmountUsed);
                                    }

                                    result.SumAmountUsed = totalMoneyBudgetUsedByScope;
                                    result.SumAmountUsedByCustomer = budgetUseByCustomer.Where(x => x.CustomerCode == request.CustomerCode
                                                                    && x.ShiptoCode == request.CustomerShiptoCode).Sum(x => x.AmountUsed);

                                    // Check over Budget by Scope
                                    if (totalMoneyBudgetDefineByScope < (result.SumAmountUsed + request.AmountOfDonation))
                                    {
                                        result.OverAmount = true;
                                    }
                                    else
                                    {
                                        result.OverAmount = false;
                                    }

                                    // Check over Budget By Customer
                                    var FlagBudgetQuantityLimitDetail = budgetDefineByCustomer.FirstOrDefault().FlagBudgetQuantityLimitDetail;
                                    var BudgetQuantityLimitDetail = budgetDefineByCustomer.FirstOrDefault().BudgetQuantityLimitDetail;
                                    if (FlagBudgetQuantityLimitDetail && BudgetQuantityLimitDetail > 0)
                                    {
                                        if (BudgetQuantityLimitDetail < (result.SumAmountUsedByCustomer + request.AmountOfDonation))
                                        {
                                            result.OverAmountByCustomer = true;
                                        }
                                        else
                                        {
                                            result.OverAmountByCustomer = false;
                                        }
                                    }

                                    if (((totalBudgetDefine >= (totalMoneyUsed + request.AmountOfDonation)) && (totalMoneyBudgetDefineByScope >= (totalMoneyBudgetUsedByScope + request.AmountOfDonation)) && !FlagBudgetQuantityLimitDetail)
                                        || ((totalBudgetDefine >= (totalMoneyUsed + request.AmountOfDonation)) && (totalMoneyBudgetDefineByScope >= (totalMoneyBudgetUsedByScope + request.AmountOfDonation)) && (totalBudgetDefineByCustomer) >= (totalBudgetUsedByCustomer + request.AmountOfDonation)))
                                    {
                                        result.IsEnoughBudgetForAmount = true;
                                    }
                                    else
                                    {
                                        result.IsEnoughBudgetForAmount = false;
                                    }
                                    result.BudgetForAmountInfo = budgetInfo;
                                }
                            }
                            else
                            {
                                if (budgetInfo.BudgetType == CommonData.BudgetForTradePromotion.Money)
                                {
                                    decimal totalBudgetDefine = budgetInfo.TotalAmountAllotment;
                                    decimal totalMoneyUsed = budgetUseds.Sum(x => x.AmountUsed);

                                    decimal totalBudgetDefineByCustomer = budgetInfo.BudgetAllotmentModels.FirstOrDefault().BudgetQuantityDetail;
                                    decimal totalBudgetUsedByCustomer = budgetUseds.Where(x => x.CustomerCode == request.CustomerCode
                                                                        && x.ShiptoCode == request.CustomerShiptoCode).Sum(x => x.AmountUsed);

                                    if ((totalBudgetDefine >= (totalMoneyUsed + request.AmountOfDonation))
                                        && (totalBudgetDefineByCustomer) >= (totalBudgetUsedByCustomer + request.AmountOfDonation))
                                    {
                                        result.IsEnoughBudgetForAmount = true;
                                    }
                                    else
                                    {
                                        result.IsEnoughBudgetForAmount = false;
                                    }

                                    result.SumAmountUsed = totalBudgetUsedByCustomer;
                                    result.SumAmountUsedByCustomer = totalBudgetUsedByCustomer;
                                    result.OverAmount = false;
                                    result.OverAmountByCustomer = false;
                                    result.BudgetForAmountInfo = budgetInfo;
                                }
                            }
                        }
                    }
                    result.IsHaveBudget = true;
                }
                else
                {
                    result.IsHaveBudget = false;
                }
            }
            return result;
        }

        private InventoryItem GetInventoryItemByItemHierarchy(string ItemHierarchyLevel, string ItemHierarchyValue)
        {
            var itemAttributeId = _dbItemAttribute.GetAllQueryable(x => x.ItemAttributeMaster == ItemHierarchyLevel
                                    && x.ItemAttributeCode == ItemHierarchyValue).FirstOrDefault();
            if (itemAttributeId != null)
            {
                var inventoryItem = _dbInventoryItem.GetAllQueryable(x => x.Attribute1 == itemAttributeId.Id
                                                                        || x.Attribute2 == itemAttributeId.Id
                                                                        || x.Attribute3 == itemAttributeId.Id
                                                                        || x.Attribute4 == itemAttributeId.Id
                                                                        || x.Attribute5 == itemAttributeId.Id
                                                                        || x.Attribute6 == itemAttributeId.Id
                                                                        || x.Attribute7 == itemAttributeId.Id
                                                                        || x.Attribute8 == itemAttributeId.Id
                                                                        || x.Attribute9 == itemAttributeId.Id
                                                                        || x.Attribute10 == itemAttributeId.Id
                                                                        ).FirstOrDefault();

                return inventoryItem;
            }

            return new InventoryItem();
        }

        private async Task<BudgetForCheckModel> GetBudgetInfo(string budgetCode)
        {
            return await (from b in _dbPromotionBudget.GetAllQueryable()
                          join bd in _dpTpBudgetDefine.GetAllQueryable()
                          on b.Code equals bd.BudgetCode
                          where b.Code == budgetCode
                          select new BudgetForCheckModel()
                          {
                              Code = b.Code,
                              SaleOrg = b.SaleOrg,
                              BudgetType = b.BudgetType,
                              BudgetAllocationForm = b.BudgetAllocationForm,
                              BudgetAllocationLevel = b.BudgetAllocationLevel,
                              FlagOverBudget = b.FlagOverBudget,
                              Status = b.Status,
                              PromotionProductType = bd.PromotionProductType,
                              PromotionProductCode = bd.PromotionProductCode,
                              PackSize = bd.PackSize,
                              BudgetQuantity = bd.BudgetQuantity,
                              BudgetQuantityUsed = bd.BudgetQuantityUsed,
                              ItemHierarchyLevel = bd.ItemHierarchyLevel,
                              ItemHierarchyValue = bd.ItemHierarchyValue,
                              TotalAmountAllotment = bd.TotalAmountAllotment
                          }).FirstOrDefaultAsync();
        }

        public async Task<ListPromotionAndDiscountResponseModel> ExternalApiGetListPromotionAndDiscount(ListPromotionAndDiscountRequestModel request)
        {
            var result = new ListPromotionAndDiscountResponseModel();
            try
            {
                var lstPromotion = await GetListPromotionByCustomer(request);
                if (lstPromotion != null && lstPromotion.Count > 0)
                {
                    result.ListPromotionGeneralInfos = lstPromotion;
                }

                var discount = await GetDiscountByCustomer(request);
                if (discount != null && !string.IsNullOrEmpty(discount.Code))
                {
                    result.DiscountGeneralInfo = discount;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DiscountExternalModel> GetDiscountByCustomer(ListPromotionAndDiscountRequestModel request)
        {
            var now = DateTime.Now;
            DiscountExternalModel result = null;
            try
            {
                var dbCustomerInfo = _dbCustomerInformation.GetAllQueryable(x => x.CustomerCode == request.CustomerCode).AsNoTracking();
                var dbCustomerShipto = _dbCustomerShipto.GetAllQueryable(x => x.ShiptoCode == request.ShiptoCode);

                // List By Scope Territory
                var lstByScopeTerritory = await _dbDiscountScopeTerritory.GetAllQueryable(x => x.SaleOrg == request.SaleOrgCode
                                                && ((x.ScopeSaleTerritoryLevel == CommonData.TerritoryLevelSetting.Branch && x.SalesTerritoryValue == request.Branch)
                                                || (x.ScopeSaleTerritoryLevel == CommonData.TerritoryLevelSetting.Region && x.SalesTerritoryValue == request.Region)
                                                || (x.ScopeSaleTerritoryLevel == CommonData.TerritoryLevelSetting.SubRegion && x.SalesTerritoryValue == request.SubRegion)
                                                || (x.ScopeSaleTerritoryLevel == CommonData.TerritoryLevelSetting.Area && x.SalesTerritoryValue == request.Area)
                                                || (x.ScopeSaleTerritoryLevel == CommonData.TerritoryLevelSetting.SubArea && x.SalesTerritoryValue == request.SubArea)))
                                                .Select(x => x.DiscountCode).ToListAsync();

                // List By Scope Dsa
                var lstByScopeDsa = await _dbDiscountScopeDsa.GetAllQueryable(x => x.SaleOrg == request.SaleOrgCode
                                    && x.ScopeDsaValue == request.DsaCode).Select(x => x.DiscountCode).ToListAsync();

                // List discount For Object Applicable Customer Attribute.
                var CustomerObjects = await (from ci in dbCustomerInfo
                                             join cs in dbCustomerShipto
                                             on ci.Id equals cs.CustomerInfomationId
                                             join cda in _dbCustomerDsm.GetAllQueryable()
                                             on cs.Id equals cda.CustomerShiptoId
                                             join ca in _dbCustomerAttribute.GetAllQueryable(x => x.EffectiveDate <= now && (!x.ValidUntil.HasValue || x.ValidUntil.Value >= now))
                                             on cda.CustomerAttributeId equals ca.Id
                                             join cs2 in _dbCustomerSetting.GetAllQueryable()
                                             on ca.CustomerSettingId equals cs2.Id
                                             select new CustomerObjectApplicable()
                                             {
                                                 CustomerCode = ci.CustomerCode,
                                                 ShiptoCode = cs.ShiptoCode,
                                                 CustomerAttributeLevel = cs2.AttributeId,
                                                 CustomerAttributeValue = ca.Code
                                             }).AsNoTracking().ToListAsync();

                // Applicable Object Customer Attribute
                var lstAllTpObjectCustomerAttributes = await _dbDiscountAttributeValue.GetAllQueryable().AsNoTracking().ToListAsync();
                var lstTpCustomerAttribute = new List<string>();
                foreach (var item in CustomerObjects)
                {
                    var tempDataScope = lstAllTpObjectCustomerAttributes.Where(x =>
                    x.CustomerAttributerLevel == item.CustomerAttributeLevel
                    && x.CustomerAttributerValue == item.CustomerAttributeValue).Select(z => z.DiscountCode);

                    lstTpCustomerAttribute.AddRange(tempDataScope);
                }

                // Applicable Object Customer Shipto
                var lstTpCustomerShipto = await _dbDiscountShipto.GetAllQueryable(x => x.CustomerCode == request.CustomerCode
                && x.CustomerShiptoCode == request.ShiptoCode).Select(y => y.DiscountCode).AsNoTracking().ToListAsync();

                // List discount
                var lstDiscountScopeAll = await (from p in _dbDiscount.GetAllQueryable(x => x.Status == CommonData.PromotionSetting.Confirmed
                                           && x.EffectiveDate <= DateTime.Now && (x.ValidUntil == null || (x.ValidUntil.HasValue && x.ValidUntil.Value >= DateTime.Now)))
                                                 select new DiscountExternalModel()
                                                 {
                                                     Id = p.Id,
                                                     Code = p.Code,
                                                     ShortName = p.ShortName,
                                                     FullName = p.FullName,
                                                     Scheme = p.Scheme,
                                                     Status = p.Status,
                                                     EffectiveDateFrom = p.EffectiveDate,
                                                     ValidUntil = p.ValidUntil,
                                                     SaleOrg = p.SaleOrg,
                                                     ScopeType = p.ScopeType,
                                                     ApplicableObjectType = p.ObjectType,
                                                     SicCode = p.SicCode,
                                                     DiscountType = p.DiscountType
                                                 }
                                      ).AsNoTracking().ToListAsync();

                // TH1. List Discount Scope All - Applicable All
                var lstDiscount1 = lstDiscountScopeAll.Where(x => x.ScopeType == CommonData.PromotionSetting.ScopeNationwide
                                    && x.ApplicableObjectType == CommonData.PromotionSetting.ObjectAllCustomer).ToList();

                // TH2. List Discount Scope All - Customer Attribute
                var lstDiscount2 = lstDiscountScopeAll.Where(x => x.ScopeType == CommonData.PromotionSetting.ScopeNationwide
                                    && lstTpCustomerAttribute.Contains(x.Code)).ToList();

                // TH3. List Discount Scope All - Customer Shipto
                var lstDiscount3 = lstDiscountScopeAll.Where(x => x.ScopeType == CommonData.PromotionSetting.ScopeNationwide
                                    && lstTpCustomerShipto.Contains(x.Code)).ToList();

                // TH4. List Discount Scope Territory - Applicable Object all
                var lstDiscount4 = lstDiscountScopeAll.Where(x => lstByScopeTerritory.Contains(x.Code)
                                    && x.ApplicableObjectType == CommonData.PromotionSetting.ObjectAllCustomer).ToList();

                // TH5. List Discount Scope Territory - Customer Attribute
                var lstDiscount5 = lstDiscountScopeAll.Where(x => lstByScopeTerritory.Contains(x.Code)
                                    && lstTpCustomerAttribute.Contains(x.Code)).ToList();

                // TH6. List Discount Scope Dsa - Applicable Object all
                var lstDiscount6 = lstDiscountScopeAll.Where(x => lstByScopeDsa.Contains(x.Code)
                                    && x.ApplicableObjectType == CommonData.PromotionSetting.ObjectAllCustomer).ToList();

                // TH7. List Discount Scope Territory - Customer Attribute
                var lstDiscount7 = lstDiscountScopeAll.Where(x => lstByScopeDsa.Contains(x.Code)
                                    && lstTpCustomerAttribute.Contains(x.Code)).ToList();
                if (lstDiscount3 != null && lstDiscount3.Count > 0)
                {
                    result = lstDiscount3.FirstOrDefault();
                }

                if (lstDiscount7 != null && lstDiscount7.Count > 0 && result == null)
                {
                    result = lstDiscount7.FirstOrDefault();
                }

                if (lstDiscount6 != null && lstDiscount6.Count > 0 && result == null)
                {
                    result = lstDiscount6.FirstOrDefault();
                }

                if (lstDiscount5 != null && lstDiscount5.Count > 0 && result == null)
                {
                    result = lstDiscount5.FirstOrDefault();
                }

                if (lstDiscount4 != null && lstDiscount4.Count > 0 && result == null)
                {
                    result = lstDiscount4.FirstOrDefault();
                }

                if (lstDiscount2 != null && lstDiscount2.Count > 0 && result == null)
                {
                    result = lstDiscount2.FirstOrDefault();
                }

                if (lstDiscount1 != null && lstDiscount1.Count > 0 && result == null)
                {
                    result = lstDiscount1.FirstOrDefault();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
