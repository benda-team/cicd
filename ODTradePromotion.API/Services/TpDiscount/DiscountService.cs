using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ODTradePromotion.API.Constants;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Infrastructure.Tp;
using ODTradePromotion.API.Models.Base;
using ODTradePromotion.API.Models.Customer;
using ODTradePromotion.API.Models.Discount;
using ODTradePromotion.API.Models.External;
using ODTradePromotion.API.Models.Promotion;
using ODTradePromotion.API.Models.SalesOrg;
using ODTradePromotion.API.Services.Base;
using ODTradePromotion.API.Services.User;
using ODTradePromotion.API.Services.UserWithDistributor;
using Sys.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.TpDiscount
{
    public class DiscountService : IDiscountService
    {
        private readonly ILogger<DiscountService> _logger;
        private readonly IBaseRepository<Infrastructure.Tp.TpDiscount> _dbDiscount;
        private readonly IBaseRepository<TpDiscountScopeTerritory> _dbDiscountScopeTerritory;
        private readonly IBaseRepository<TpDiscountScopeDsa> _dbDiscountScopeDsa;
        private readonly IBaseRepository<TpDiscountObjectCustomerAttributeLevel> _dbDiscountAttributeLevel;
        private readonly IBaseRepository<TpDiscountObjectCustomerAttributeValue> _dbDiscountAttributeValue;
        private readonly IBaseRepository<TpDiscountObjectCustomerShipto> _dbDiscountShipto;
        private readonly IBaseRepository<Infrastructure.Tp.TpDiscountStructureDetail> _dbDiscountStructureDetail;
        private readonly IBaseRepository<CustomerSetting> _dbCustomerSetting;
        private readonly IBaseRepository<CustomerAttribute> _dbCustomerAttribute;
        private readonly IBaseRepository<CustomerShipto> _dbCustomerShipto;
        private readonly IBaseRepository<SystemSetting> _dbSettingService;
        private readonly IBaseRepository<ScTerritoryMapping> _dbScTerritoryMapping;
        private readonly IBaseRepository<Infrastructure.CustomerDmsAttribute> _dbCustomerDsm;
        private readonly IBaseRepository<ScTerritoryValue> _dbTerritoryValue;
        private readonly IBaseRepository<CustomerInformation> _dbCustomerInformation;
        private readonly IBaseRepository<ScSalesOrganizationStructure> _dbSaleOrg;
        private readonly IBaseRepository<RzRouteZoneInfomation> _dbRzInfo;
        private readonly IBaseRepository<RzRouteZoneShipto> _dbRzShipto;
        private readonly IBaseRepository<DsaDistributorSellingArea> _dbDsa;
        private readonly IUserService _userService;
        private readonly IUserWithDistributorService _userWithDisService;

        private readonly IMapper _mapper;

        public DiscountService(
            ILogger<DiscountService> logger,
            IBaseRepository<Infrastructure.Tp.TpDiscount> serviceDiscount,
            IBaseRepository<TpDiscountScopeTerritory> dbScopeTerritory,
            IBaseRepository<TpDiscountScopeDsa> dbScopeDsa,
            IBaseRepository<TpDiscountObjectCustomerAttributeLevel> dbDiscountAttributeLevel,
            IBaseRepository<TpDiscountObjectCustomerAttributeValue> dbDiscountAttributeValue,
            IBaseRepository<TpDiscountObjectCustomerShipto> dbDiscountShipto,
            IBaseRepository<TpScopeDiscount> serviceScopeDiscount,
            IBaseRepository<Infrastructure.Tp.TpDiscountStructureDetail> serviceDiscountStructureDetail,
            IBaseRepository<CustomerSetting> serviceCustomerSetting,
            IBaseRepository<CustomerAttribute> serviceCustomerAttribute,
            IBaseRepository<CustomerShipto> serviceCustomerShipto,
            IBaseRepository<SystemSetting> systemSettingService,
            IBaseRepository<ScTerritoryMapping> dbScTerritoryMapping,
            IBaseRepository<Infrastructure.CustomerDmsAttribute> dbCustomerDsm,
            IBaseRepository<ScTerritoryValue> serviceTerritoryValue,
            IBaseRepository<CustomerInformation> serviceCustomerInformation,
            IBaseRepository<ScSalesOrganizationStructure> dbSaleOrg,
            IBaseRepository<RzRouteZoneInfomation> dbRzInfo,
            IBaseRepository<RzRouteZoneShipto> dbRzShipto,
            IBaseRepository<DsaDistributorSellingArea> serviceDsa,
            IMapper mapper,
            IUserService userService,
            IUserWithDistributorService userWithDisService)
        {
            _logger = logger;
            _dbDiscount = serviceDiscount;
            _dbDiscountScopeTerritory = dbScopeTerritory;
            _dbDiscountScopeDsa = dbScopeDsa;
            _dbDiscountAttributeLevel = dbDiscountAttributeLevel;
            _dbDiscountAttributeValue = dbDiscountAttributeValue;
            _dbDiscountShipto = dbDiscountShipto;
            _dbDiscountStructureDetail = serviceDiscountStructureDetail;
            _dbSettingService = systemSettingService;
            _dbScTerritoryMapping = dbScTerritoryMapping;
            _dbCustomerDsm = dbCustomerDsm;
            _dbTerritoryValue = serviceTerritoryValue;
            _dbCustomerInformation = serviceCustomerInformation;
            _dbCustomerShipto = serviceCustomerShipto;
            _dbCustomerSetting = serviceCustomerSetting;
            _dbCustomerAttribute = serviceCustomerAttribute;
            _dbSaleOrg = dbSaleOrg;
            _dbRzInfo = dbRzInfo;
            _dbRzShipto = dbRzShipto;
            _dbDsa = serviceDsa;
            _mapper = mapper;
            _userService = userService;
            _userWithDisService = userWithDisService;
        }

        public IQueryable<TpDiscountModel> GetListTpDiscount()
        {

            return _dbDiscount.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                .Select(f => new TpDiscountModel()
                {
                    Id = f.Id,
                    Code = f.Code,
                    FullName = f.FullName,
                    ShortName = f.ShortName,
                    Scheme = f.Scheme,
                    EffectiveDate = f.EffectiveDate,
                    ValidUntil = f.ValidUntil,
                    SaleOrg = f.SaleOrg,
                    DiscountFrequency = f.DiscountFrequency,
                    FilePath = f.FilePath,
                    FileName = f.FileName,
                    Reason = f.Reason,
                    Status = f.Status,
                    ScopeType = f.ScopeType,
                    ObjectType = f.ObjectType,
                    FileExt = f.FileExt,
                    FolderType = f.FolderType,
                }).AsQueryable();
        }

        public IQueryable<TpDiscountModel> GetListTpDiscountForSettlement()
        {
            var systemSettings = _dbSettingService.GetAllQueryable(x => x.IsActive).AsNoTracking().AsQueryable();
            var result = (from p in _dbDiscount.GetAllQueryable(x => x.DeleteFlag == 0 && x.Status.ToLower().Equals(CommonData.PromotionSetting.Confirmed.ToLower()))
                          join status in systemSettings.Where(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.PromotionStatus.ToLower()))
                          .AsNoTracking() on p.Status equals status.SettingKey into emptyStatus
                          from status in emptyStatus.DefaultIfEmpty()
                          select new TpDiscountModel()
                          {
                              Id = p.Id,
                              Code = p.Code,
                              ShortName = p.ShortName,
                              FullName = p.FullName,
                              Status = p.Status,
                              StatusName = (status != null) ? status.Description : string.Empty,
                              Scheme = p.Scheme,
                              EffectiveDate = p.EffectiveDate,
                              ValidUntil = p.ValidUntil,
                              SaleOrg = p.SaleOrg,
                              DiscountFrequency = p.DiscountFrequency,
                          }).AsQueryable();

            return result;
        }

        public TpDiscountModel GetTpDiscountDetailById(Guid Id)
        {
            DateTime now = DateTime.Now;
            var discount = _mapper.Map<TpDiscountModel>(_dbDiscount.GetAllQueryable(x => x.Id == Id && x.DeleteFlag == 0).FirstOrDefault());
            if (discount != null)
            {
                // get List Scope
                if (discount.ScopeType.Equals(CommonData.PromotionSetting.ScopeSalesTerritoryLevel))
                {
                    var listScopeSales = (from scope in _dbDiscountScopeTerritory
                                          .GetAllQueryable(x => x.DiscountCode.ToLower().Equals(discount.Code.ToLower())).AsNoTracking()
                                          join territory in _dbTerritoryValue
                                          .GetAllQueryable(x => x.TerritoryLevelCode.ToLower().Equals(discount.ScopeSaleTerritoryLevel.ToLower())).AsNoTracking()
                                          on scope.SalesTerritoryValue equals territory.Code into emptyScope
                                          from territory in emptyScope.DefaultIfEmpty()
                                          select new TpSalesTerritoryValueModel()
                                          {
                                              Code = scope.SalesTerritoryValue,
                                              TerritoryLevelCode = discount.ScopeSaleTerritoryLevel,
                                              Description = (territory != null) ? territory.Description : string.Empty
                                          }).AsNoTracking().ToList();
                    discount.ListScopeSalesTerritory = new List<TpSalesTerritoryValueModel>();
                    discount.ListScopeSalesTerritory = listScopeSales;
                }
                else if (discount.ScopeType.Equals(CommonData.PromotionSetting.ScopeDSA))
                {
                    var listScopeSales = (from scope in _dbDiscountScopeDsa
                                          .GetAllQueryable(x => x.DiscountCode.ToLower().Equals(discount.Code.ToLower())).AsNoTracking()
                                          join dsa in _dbDsa.GetAllQueryable(x => x.EffectiveDate <= now && (!x.UntilDate.HasValue || x.UntilDate.Value >= now)).AsNoTracking()
                                          on scope.ScopeDsaValue equals dsa.Code into emptyScope
                                          from dsa in emptyScope.DefaultIfEmpty()
                                          select new TpSalesOrgDsaModel()
                                          {
                                              Code = scope.ScopeDsaValue,
                                              Description = (dsa != null) ? dsa.Description : string.Empty
                                          }).AsNoTracking().ToList();
                    discount.ListScopeDSA = new List<TpSalesOrgDsaModel>();
                    discount.ListScopeDSA = listScopeSales;
                }

                if (discount.ObjectType.Equals(CommonData.PromotionSetting.ObjectCustomerAttributes))
                {
                    var listCustomerSetting = (from cussetting in _dbDiscountAttributeLevel
                                               .GetAllQueryable(x => x.DiscountCode.ToLower().Equals(discount.Code.ToLower())).AsNoTracking()
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
                    discount.ListCustomerSetting = new List<CustomerSettingModel>();
                    discount.ListCustomerSetting = listCustomerSetting;

                    List<CustomerAttributeModel> listCustomerAttributeModel = new List<CustomerAttributeModel>();
                    var lstCustomerAttributes = _dbCustomerAttribute.GetAllQueryable(x => x.EffectiveDate <= now && (!x.ValidUntil.HasValue || x.ValidUntil.Value >= now)).AsNoTracking().AsQueryable();
                    var lstCustomerSettingApply = listCustomerSetting.Where(x => x.IsChecked).ToList();
                    foreach (var item in lstCustomerSettingApply)
                    {
                        var tempListCustomerAttributes = lstCustomerAttributes.Where(x => x.CustomerSettingId == item.CustomerSettingId).AsNoTracking().AsQueryable();

                        var listCustomerValue = (from customervalue in _dbDiscountAttributeValue
                                               .GetAllQueryable(x => x.DiscountCode.ToLower().Equals(discount.Code.ToLower())
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
                    discount.ListCustomerAttribute = new List<CustomerAttributeModel>();
                    discount.ListCustomerAttribute = listCustomerAttributeModel;
                }
                else if (discount.ObjectType.Equals(CommonData.PromotionSetting.ObjectCustomerShipto))
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

                    var lstDataCustomerShipto = (from data in _dbDiscountShipto
                                          .GetAllQueryable(x => x.DiscountCode.ToLower().Equals(discount.Code.ToLower())).AsNoTracking()
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
                    discount.ListCustomerShipto = new List<CustomerShiptoModel>();
                    discount.ListCustomerShipto = lstDataCustomerShipto;
                }

                // Structure define
                var ListStructureDifines = _mapper.Map<List<TpDiscountStructureDetailModel>>(_dbDiscountStructureDetail.GetAllQueryable(x => x.DiscountCode == discount.Code && x.DeleteFlag == 0).ToList());
                discount.ListDiscountStructureDetails = ListStructureDifines;
            }
            return discount;
        }

        public TpDiscountModel GetGeneralTpDiscountByCode(string code)
        {
            var returnValue = _mapper.Map<TpDiscountModel>(_dbDiscount.GetAllQueryable(x => x.Code == code && x.DeleteFlag == 0).FirstOrDefault());
            return returnValue;
        }

        public TpDiscountModel GetTpDiscountDetailByCode(string code)
        {
            DateTime now = DateTime.Now;
            var discount = _mapper.Map<TpDiscountModel>(_dbDiscount.GetAllQueryable(x => x.Code == code && x.DeleteFlag == 0).FirstOrDefault());
            if (discount != null)
            {
                // get List Scope
                if (discount.ScopeType.Equals(CommonData.PromotionSetting.ScopeSalesTerritoryLevel))
                {
                    var listScopeSales = (from scope in _dbDiscountScopeTerritory
                                          .GetAllQueryable(x => x.DiscountCode.ToLower().Equals(code.ToLower())).AsNoTracking()
                                          join territory in _dbTerritoryValue
                                          .GetAllQueryable(x => x.TerritoryLevelCode.ToLower().Equals(discount.ScopeSaleTerritoryLevel.ToLower())).AsNoTracking()
                                          on scope.SalesTerritoryValue equals territory.Code into emptyScope
                                          from territory in emptyScope.DefaultIfEmpty()
                                          select new TpSalesTerritoryValueModel()
                                          {
                                              Code = scope.SalesTerritoryValue,
                                              TerritoryLevelCode = discount.ScopeSaleTerritoryLevel,
                                              Description = (territory != null) ? territory.Description : string.Empty,
                                              OwnerType = scope.OwnerType,
                                              OwnerCode = scope.OwnerCode
                                          }).AsNoTracking().ToList();
                    discount.ListScopeSalesTerritory = new List<TpSalesTerritoryValueModel>();
                    discount.ListScopeSalesTerritory = listScopeSales;
                }
                else if (discount.ScopeType.Equals(CommonData.PromotionSetting.ScopeDSA))
                {
                    var listScopeSales = (from scope in _dbDiscountScopeDsa
                                          .GetAllQueryable(x => x.DiscountCode.ToLower().Equals(code.ToLower())).AsNoTracking()
                                          join dsa in _dbDsa.GetAllQueryable(x => x.EffectiveDate <= now && (!x.UntilDate.HasValue || x.UntilDate.Value >= now)).AsNoTracking()
                                          on scope.ScopeDsaValue equals dsa.Code into emptyScope
                                          from dsa in emptyScope.DefaultIfEmpty()
                                          select new TpSalesOrgDsaModel()
                                          {
                                              Code = scope.ScopeDsaValue,
                                              Description = (dsa != null) ? dsa.Description : string.Empty,
                                              OwnerType = scope.OwnerType,
                                              OwnerCode = scope.OwnerCode
                                          }).AsNoTracking().ToList();
                    discount.ListScopeDSA = new List<TpSalesOrgDsaModel>();
                    discount.ListScopeDSA = listScopeSales;
                }

                if (discount.ObjectType.Equals(CommonData.PromotionSetting.ObjectCustomerAttributes))
                {
                    var listCustomerSetting = (from cussetting in _dbDiscountAttributeLevel
                                               .GetAllQueryable(x => x.DiscountCode.ToLower().Equals(code.ToLower())).AsNoTracking()
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
                                                   CustomerSettingId = csetting.Id,
                                                   OwnerType = cussetting.OwnerType,
                                                   OwnerCode = cussetting.OwnerCode
                                               }).AsNoTracking().ToList();
                    discount.ListCustomerSetting = new List<CustomerSettingModel>();
                    discount.ListCustomerSetting = listCustomerSetting;

                    List<CustomerAttributeModel> listCustomerAttributeModel = new List<CustomerAttributeModel>();
                    var lstCustomerAttributes = _dbCustomerAttribute.GetAllQueryable(x => x.EffectiveDate <= now && (!x.ValidUntil.HasValue || x.ValidUntil.Value >= now)).AsNoTracking().AsQueryable();
                    var lstCustomerSettingApply = listCustomerSetting.Where(x => x.IsChecked).ToList();
                    foreach (var item in lstCustomerSettingApply)
                    {
                        var tempListCustomerAttributes = lstCustomerAttributes.Where(x => x.CustomerSettingId == item.CustomerSettingId).AsNoTracking().AsQueryable();

                        var listCustomerValue = (from customervalue in _dbDiscountAttributeValue
                                               .GetAllQueryable(x => x.DiscountCode.ToLower().Equals(code.ToLower())
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
                                                     OwnerType = customervalue.OwnerType,
                                                     OwnerCode = customervalue.OwnerCode
                                                 }).AsNoTracking().ToList();

                        listCustomerAttributeModel.AddRange(listCustomerValue);
                    }
                    discount.ListCustomerAttribute = new List<CustomerAttributeModel>();
                    discount.ListCustomerAttribute = listCustomerAttributeModel;
                }
                else if (discount.ObjectType.Equals(CommonData.PromotionSetting.ObjectCustomerShipto))
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
                                                  Address = customershipto.Address,
                                                  OwnerType = customershipto.OwnerType,
                                                  OwnerCode = customershipto.OwnerCode
                                              }).AsNoTracking().AsQueryable();

                    var lstDataCustomerShipto = (from data in _dbDiscountShipto
                                          .GetAllQueryable(x => x.DiscountCode.ToLower().Equals(code.ToLower())).AsNoTracking()
                                                 join customershipto in listCustomerShipto on
                                                 new { customer_code = data.CustomerCode, customer_shipto_code = data.CustomerShiptoCode } equals
                                                 new { customer_code = customershipto.CustomerCode, customer_shipto_code = customershipto.ShiptoCode }
                                                 into temptyCustomerShipto
                                                 from customershipto in temptyCustomerShipto.DefaultIfEmpty()
                                                 select new CustomerShiptoModel()
                                                 {
                                                     CustomerCode = data.CustomerCode,
                                                     ShiptoCode = data.CustomerShiptoCode,
                                                     Address = (customershipto != null) ? customershipto.Address : string.Empty,
                                                     OwnerType = data.OwnerType,
                                                     OwnerCode = data.OwnerCode
                                                 }).AsNoTracking().ToList();
                    discount.ListCustomerShipto = new List<CustomerShiptoModel>();
                    discount.ListCustomerShipto = lstDataCustomerShipto;
                }

                // Structure define
                var ListStructureDifines = _mapper.Map<List<TpDiscountStructureDetailModel>>(_dbDiscountStructureDetail.GetAllQueryable(x => x.DiscountCode == code && x.DeleteFlag == 0).ToList());
                discount.ListDiscountStructureDetails = ListStructureDifines;
            }
            return discount;
        }

        public async Task<TpDiscountModel> GetGeneralDiscountByCode(string code)
        {
            var data = await _dbDiscount.GetAllQueryable(m => m.Code.ToLower().Equals(code.ToLower()) && m.DeleteFlag == 0).FirstOrDefaultAsync();
            return _mapper.Map<TpDiscountModel>(data);
        }

        public async Task<BaseResultModel> CreateTpDiscount(TpDiscountModel input, string userlogin)
        {
            try
            {
                string _ownerType = OwnerTypeConst.SYSTEM;
                string _ownerCode = null;
                // Check user
                var userInfo = _userService.GetUserInfoByUserName(userlogin);
                if (userInfo.IsDistributorUser) {
                    _ownerType = OwnerTypeConst.DISTRIBUTOR;

                    // Get distributorCode
                    var listDisCode = await _userWithDisService.GetUserWithDistributorsAsync(userlogin);
                    string disCode = listDisCode != null && listDisCode.Count > 0 ? listDisCode.First().DistributorCode : null;
                    if (disCode == null) 
                    {
                        return new BaseResultModel 
                        {
                            IsSuccess = false,
                            Message = "Cannot found distributor code",
                            Code = 404
                        };
                    }
                    _ownerCode = disCode;
                }

                input.OwnerType = _ownerType;
                input.OwnerCode = _ownerCode;
                // Create Discount
                var discount = _mapper.Map<Infrastructure.Tp.TpDiscount>(input);
                discount.Id = Guid.NewGuid();
                discount.CreatedDate = DateTime.Now;
                discount.CreatedBy = userlogin;
                _dbDiscount.Insert(discount);

                // Scope
                if (discount.ScopeType.ToLower().Equals(CommonData.PromotionSetting.ScopeSalesTerritoryLevel.ToLower()))
                {
                    List<TpDiscountScopeTerritory> listScope = new List<TpDiscountScopeTerritory>();
                    if (input.ListScopeSalesTerritory != null && input.ListScopeSalesTerritory.Count > 0)
                    {
                        foreach (var item in input.ListScopeSalesTerritory)
                        {
                            listScope.Add(new TpDiscountScopeTerritory()
                            {
                                Id = Guid.NewGuid(),
                                DiscountCode = discount.Code,
                                SaleOrg = discount.SaleOrg,
                                ScopeSaleTerritoryLevel = discount.ScopeSaleTerritoryLevel,
                                SalesTerritoryValue = item.Code,
                                OwnerType = _ownerType,
                                OwnerCode = _ownerCode
                            });
                        }
                        _dbDiscountScopeTerritory.InsertRange(listScope);
                    }
                }
                else if (discount.ScopeType.ToLower().Equals(CommonData.PromotionSetting.ScopeDSA.ToLower()))
                {
                    List<TpDiscountScopeDsa> listScope = new List<TpDiscountScopeDsa>();
                    if (input.ListScopeDSA != null && input.ListScopeDSA.Count > 0)
                    {
                        foreach (var item in input.ListScopeDSA)
                        {
                            listScope.Add(new TpDiscountScopeDsa()
                            {
                                Id = Guid.NewGuid(),
                                DiscountCode = discount.Code,
                                SaleOrg = discount.SaleOrg,
                                ScopeDsaValue = item.Code,
                                OwnerType = _ownerType,
                                OwnerCode = _ownerCode
                            });
                        }
                        _dbDiscountScopeDsa.InsertRange(listScope);
                    }
                }

                // Applicable Object
                if (discount.ObjectType.ToLower().Equals(CommonData.PromotionSetting.ObjectCustomerAttributes.ToLower()))
                {
                    // Insert Customer Setting
                    List<TpDiscountObjectCustomerAttributeLevel> listCustomerSetting = new List<TpDiscountObjectCustomerAttributeLevel>();
                    if (input.ListCustomerSetting != null && input.ListCustomerSetting.Count > 0)
                    {
                        foreach (var item in input.ListCustomerSetting)
                        {
                            listCustomerSetting.Add(new TpDiscountObjectCustomerAttributeLevel()
                            {
                                Id = Guid.NewGuid(),
                                DiscountCode = discount.Code,
                                CustomerAttributerLevel = item.AttributeID,
                                IsApply = item.IsChecked,
                                OwnerType = _ownerType,
                                OwnerCode = _ownerCode
                            });
                        }
                        _dbDiscountAttributeLevel.InsertRange(listCustomerSetting);
                    }

                    // Insert Customer Attribute
                    List<TpDiscountObjectCustomerAttributeValue> listCustomerAttribute = new List<TpDiscountObjectCustomerAttributeValue>();
                    if (input.ListCustomerAttribute != null && input.ListCustomerAttribute.Count > 0)
                    {
                        foreach (var item in input.ListCustomerAttribute)
                        {
                            listCustomerAttribute.Add(new TpDiscountObjectCustomerAttributeValue()
                            {
                                Id = Guid.NewGuid(),
                                DiscountCode = discount.Code,
                                CustomerAttributerLevel = item.AttributeMaster,
                                CustomerAttributerValue = item.Code,
                                OwnerType = _ownerType,
                                OwnerCode = _ownerCode
                            });
                        }
                        _dbDiscountAttributeValue.InsertRange(listCustomerAttribute);
                    }
                }
                else if (discount.ObjectType.ToLower().Equals(CommonData.PromotionSetting.ObjectCustomerShipto.ToLower()))
                {
                    // Insert Customer Shipto
                    List<TpDiscountObjectCustomerShipto> listCustomerShipto = new List<TpDiscountObjectCustomerShipto>();
                    if (input.ListCustomerShipto != null && input.ListCustomerShipto.Count > 0)
                    {
                        foreach (var item in input.ListCustomerShipto)
                        {
                            listCustomerShipto.Add(new TpDiscountObjectCustomerShipto()
                            {
                                Id = Guid.NewGuid(),
                                DiscountCode = discount.Code,
                                CustomerCode = item.CustomerCode,
                                CustomerShiptoCode = item.ShiptoCode,
                                OwnerType = _ownerType,
                                OwnerCode = _ownerCode
                            });
                        }
                        _dbDiscountShipto.InsertRange(listCustomerShipto);
                    }
                }

                // structure
                var discountStructureDetail = _mapper.Map<List<TpDiscountStructureDetail>>(input.ListDiscountStructureDetails);
                discountStructureDetail.ForEach(x => {
                    x.DiscountCode = discount.Code;
                    x.DiscountType = discount.DiscountType;
                    x.OwnerCode = _ownerCode;
                    x.OwnerType = _ownerType;
                    x.CreatedBy = userlogin;
                    x.CreatedDate = DateTime.Now;
                });


                _dbDiscountStructureDetail.InsertRange(discountStructureDetail);

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

        public async Task<BaseResultModel> UpdateTpDiscount(TpDiscountModel input, string userlogin)
        {
            try
            {
                var dataDiscount = GetTpDiscountDetailByCode(input.Code);
                if (dataDiscount == null || string.IsNullOrEmpty(dataDiscount.Code))
                {
                    return new BaseResultModel
                    {
                        IsSuccess = false,
                        Message = "TpDiscountCodeIsNotExist"
                    };
                }

                // Check user
                var userInfo = _userService.GetUserInfoByUserName(userlogin);
                if (userInfo.IsDistributorUser)
                {
                    // Get distributorCode
                    var listDisCode = await _userWithDisService.GetUserWithDistributorsAsync(userlogin);
                    string disCode = listDisCode != null && listDisCode.Count > 0 ? listDisCode.First().DistributorCode : null;
                    if (disCode == null)
                    {
                        return new BaseResultModel
                        {
                            IsSuccess = false,
                            Message = "Cannot found distributor code",
                            Code = 404
                        };
                    }

                    if (dataDiscount.OwnerCode != disCode)
                    {
                        return new BaseResultModel
                        {
                            IsSuccess = false,
                            Message = "Cannot edit other distributor's data",
                            Code = 400
                        };
                    }
                }
                

                // Mapping Discount
                var discountId = dataDiscount.Id;
                var discount = _mapper.Map<Infrastructure.Tp.TpDiscount>(input);
                discount.Id = discountId;
                discount.UpdatedDate = DateTime.Now;
                discount.UpdatedBy = userlogin;
                discount.OwnerCode = dataDiscount.OwnerCode;
                discount.OwnerType = dataDiscount.OwnerType;
                discount.CreatedBy = dataDiscount.CreatedBy;
                discount.CreatedDate = dataDiscount.CreatedDate;
                _dbDiscount.Update(discount);

                // Scope
                var scopeSalesTerritorys = _dbDiscountScopeTerritory.GetAllQueryable(x => x.DiscountCode.ToLower().Equals(discount.Code.ToLower()));
                _dbDiscountScopeTerritory.DeleteRange(scopeSalesTerritorys);

                var scopeDsas = _dbDiscountScopeDsa.GetAllQueryable(x => x.DiscountCode.ToLower().Equals(discount.Code.ToLower()));
                _dbDiscountScopeDsa.DeleteRange(scopeDsas);

                // Scope
                if (discount.ScopeType.ToLower().Equals(CommonData.PromotionSetting.ScopeSalesTerritoryLevel.ToLower()))
                {
                    List<TpDiscountScopeTerritory> listScope = new List<TpDiscountScopeTerritory>();
                    if (input.ListScopeSalesTerritory != null && input.ListScopeSalesTerritory.Count > 0)
                    {
                        foreach (var item in input.ListScopeSalesTerritory)
                        {
                            listScope.Add(new TpDiscountScopeTerritory()
                            {
                                Id = Guid.NewGuid(),
                                DiscountCode = discount.Code,
                                SaleOrg = discount.SaleOrg,
                                ScopeSaleTerritoryLevel = discount.ScopeSaleTerritoryLevel,
                                SalesTerritoryValue = item.Code,
                                OwnerCode = dataDiscount.OwnerCode,
                                OwnerType = dataDiscount.OwnerType
                            });
                        }
                        _dbDiscountScopeTerritory.InsertRange(listScope);
                    }
                }
                else if (discount.ScopeType.ToLower().Equals(CommonData.PromotionSetting.ScopeDSA.ToLower()))
                {
                    List<TpDiscountScopeDsa> listScope = new List<TpDiscountScopeDsa>();
                    if (input.ListScopeDSA != null && input.ListScopeDSA.Count > 0)
                    {
                        foreach (var item in input.ListScopeDSA)
                        {
                            listScope.Add(new TpDiscountScopeDsa()
                            {
                                Id = Guid.NewGuid(),
                                DiscountCode = discount.Code,
                                SaleOrg = discount.SaleOrg,
                                ScopeDsaValue = item.Code,
                                OwnerCode = dataDiscount.OwnerCode,
                                OwnerType = dataDiscount.OwnerType
                            });
                        }
                        _dbDiscountScopeDsa.InsertRange(listScope);
                    }
                }

                // Applicable Object
                var objectCusSettings = _dbDiscountAttributeLevel.GetAllQueryable(x => x.DiscountCode.ToLower().Equals(discount.Code.ToLower()));
                _dbDiscountAttributeLevel.DeleteRange(objectCusSettings);

                var objectCusAttributes = _dbDiscountAttributeValue.GetAllQueryable(x => x.DiscountCode.ToLower().Equals(discount.Code.ToLower()));
                _dbDiscountAttributeValue.DeleteRange(objectCusAttributes);

                var objectCustomerShipto = _dbDiscountShipto.GetAllQueryable(x => x.DiscountCode.ToLower().Equals(discount.Code.ToLower()));
                _dbDiscountShipto.DeleteRange(objectCustomerShipto);

                if (discount.ObjectType.ToLower().Equals(CommonData.PromotionSetting.ObjectCustomerAttributes.ToLower()))
                {
                    // Insert Customer Setting
                    List<TpDiscountObjectCustomerAttributeLevel> listCustomerSetting = new List<TpDiscountObjectCustomerAttributeLevel>();
                    if (input.ListCustomerSetting != null && input.ListCustomerSetting.Count > 0)
                    {
                        foreach (var item in input.ListCustomerSetting)
                        {
                            listCustomerSetting.Add(new TpDiscountObjectCustomerAttributeLevel()
                            {
                                Id = Guid.NewGuid(),
                                DiscountCode = discount.Code,
                                CustomerAttributerLevel = item.AttributeID,
                                IsApply = item.IsChecked,
                                OwnerCode = dataDiscount.OwnerCode,
                                OwnerType = dataDiscount.OwnerType
                            });
                        }
                        _dbDiscountAttributeLevel.InsertRange(listCustomerSetting);
                    }

                    // Insert Customer Attribute
                    List<TpDiscountObjectCustomerAttributeValue> listCustomerAttribute = new List<TpDiscountObjectCustomerAttributeValue>();
                    if (input.ListCustomerAttribute != null && input.ListCustomerAttribute.Count > 0)
                    {
                        foreach (var item in input.ListCustomerAttribute)
                        {
                            listCustomerAttribute.Add(new TpDiscountObjectCustomerAttributeValue()
                            {
                                Id = Guid.NewGuid(),
                                DiscountCode = discount.Code,
                                CustomerAttributerLevel = item.AttributeMaster,
                                CustomerAttributerValue = item.Code,
                                OwnerCode = dataDiscount.OwnerCode,
                                OwnerType = dataDiscount.OwnerType
                            });
                        }
                        _dbDiscountAttributeValue.InsertRange(listCustomerAttribute);
                    }
                }
                else if (discount.ObjectType.ToLower().Equals(CommonData.PromotionSetting.ObjectCustomerShipto.ToLower()))
                {
                    // Insert Customer Shipto
                    List<TpDiscountObjectCustomerShipto> listCustomerShipto = new List<TpDiscountObjectCustomerShipto>();
                    if (input.ListCustomerShipto != null && input.ListCustomerShipto.Count > 0)
                    {
                        foreach (var item in input.ListCustomerShipto)
                        {
                            listCustomerShipto.Add(new TpDiscountObjectCustomerShipto()
                            {
                                Id = Guid.NewGuid(),
                                DiscountCode = discount.Code,
                                CustomerCode = item.CustomerCode,
                                CustomerShiptoCode = item.ShiptoCode,
                                OwnerCode = dataDiscount.OwnerCode,
                                OwnerType = dataDiscount.OwnerType
                            });
                        }
                        _dbDiscountShipto.InsertRange(listCustomerShipto);
                    }
                }

                // structure
                var structureDeletes = _dbDiscountStructureDetail.GetAllQueryable(x => x.DiscountCode.ToLower().Equals(discount.Code.ToLower()));
                _dbDiscountStructureDetail.DeleteRange(structureDeletes);

                var discountStructureDetail = _mapper.Map<List<TpDiscountStructureDetail>>(input.ListDiscountStructureDetails);
                discountStructureDetail.ForEach(x => {
                    x.DiscountCode = discount.Code;
                    x.DiscountType = discount.DiscountType;
                    x.OwnerCode = dataDiscount.OwnerCode;
                    x.OwnerType = dataDiscount.OwnerType;
                    x.UpdatedBy = userlogin;
                    x.UpdatedDate = DateTime.Now;
                    x.CreatedBy = dataDiscount.CreatedBy;
                    x.CreatedDate = dataDiscount.CreatedDate;
                });


                _dbDiscountStructureDetail.InsertRange(discountStructureDetail);

                return new BaseResultModel
                {
                    IsSuccess = true,
                    Code = 201,
                    Message = "UpdateSuccess"
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

        public bool DeleteTpDiscountByCode(string code, string userlogin)
        {
            var discount = _dbDiscount.FirstOrDefault(f => f.Code == code);
            if (discount != null)
            {
                discount.DeleteFlag = 1;
                discount.UpdatedBy = userlogin;
                discount.UpdatedDate = DateTime.Now;
                _dbDiscount.Update(discount);

                return true;
            }
            return false;
        }

        public async Task<List<TpDiscountModel>> CheckExistScopeObjectDiscount(TpDiscountModel input)
        {
            List<TpDiscountModel> tpDiscounts = new List<TpDiscountModel>();
            var tempListDiscounts = await _dbDiscount.GetAllQueryable(x => x.DeleteFlag == 0 && x.Code != input.Code && x.SaleOrg == input.SaleOrg
        && x.SicCode == input.SicCode
        && (x.Status == CommonData.PromotionSetting.WaitConfirm || x.Status == CommonData.PromotionSetting.Confirmed)).ToListAsync();

            // Check Effective Date and ValidUntil
            var lstDiscounts = new List<Infrastructure.Tp.TpDiscount>();
            if (input.ValidUntil == null)
            {
                lstDiscounts = tempListDiscounts.Where(x => x.ValidUntil == null || (x.ValidUntil.HasValue && x.ValidUntil.Value > input.EffectiveDate)).ToList();
            }
            else
            {
                lstDiscounts = tempListDiscounts.Where(x => (x.ValidUntil == null && x.EffectiveDate > input.ValidUntil.Value) ||
                (x.EffectiveDate <= input.EffectiveDate && x.ValidUntil.HasValue && x.ValidUntil.Value >= input.EffectiveDate) ||
                (x.EffectiveDate <= input.ValidUntil.Value && x.ValidUntil.HasValue && x.ValidUntil.Value >= input.ValidUntil.Value) ||
                (x.EffectiveDate >= input.EffectiveDate && x.ValidUntil.HasValue && x.ValidUntil.Value <= input.ValidUntil.Value)
                ).ToList();
            }

            if (lstDiscounts != null && lstDiscounts.Count > 0)
            {
                if (input.ScopeType == CommonData.PromotionSetting.ScopeNationwide && input.ObjectType == CommonData.PromotionSetting.ObjectAllCustomer)
                {
                    var discountExist = lstDiscounts.FirstOrDefault(x => x.ScopeType == CommonData.PromotionSetting.ScopeNationwide && x.ObjectType == CommonData.PromotionSetting.ObjectAllCustomer);
                    if (discountExist != null && !string.IsNullOrEmpty(discountExist.Code))
                    {
                        tpDiscounts.Add(new TpDiscountModel() { Code = discountExist.Code });
                    }
                }
                else if (input.ScopeType == CommonData.PromotionSetting.ScopeNationwide && input.ObjectType == CommonData.PromotionSetting.ObjectCustomerAttributes)
                {
                    var tempDis1 = lstDiscounts.Where(x => x.ScopeType == CommonData.PromotionSetting.ScopeNationwide && x.ObjectType == CommonData.PromotionSetting.ObjectCustomerAttributes).ToList();
                    var lstCustomerAttributes = input.ListCustomerAttribute.Select(x => new CustomerAttributeMasterClass() { Level = x.AttributeMaster, Code = x.Code }).ToList();
                    var lstDisCusAttributes = _dbDiscountAttributeValue.GetAllQueryable().ToList().Where(x => lstCustomerAttributes.Any(cus => cus.Level == x.CustomerAttributerLevel && cus.Code == x.CustomerAttributerValue)).ToList();
                    var lstDisCusAttributeExists = lstDisCusAttributes.Where(x => tempDis1.Select(d => d.Code).Contains(x.DiscountCode)).OrderBy(x => x.CustomerAttributerLevel).ToList();

                    var maxlevelCustomerInput = lstCustomerAttributes.OrderBy(x => x.Level).LastOrDefault().Level;
                    var lstDiscountExist = lstDisCusAttributeExists.Select(x => x.DiscountCode).Distinct();
                    var lstCustomerSettings = input.ListCustomerSetting.Where(x => x.IsChecked).ToList();
                    foreach (var dis in lstDiscountExist)
                    {
                        var maxLevel = lstDisCusAttributeExists.Where(x => x.DiscountCode == dis).OrderBy(x => x.CustomerAttributerLevel).LastOrDefault().CustomerAttributerLevel;
                        if (maxLevel == maxlevelCustomerInput)
                        {
                            bool exist = true;
                            foreach (var cusSetting in lstCustomerSettings)
                            {
                                if (!lstDisCusAttributeExists.Any(x => x.DiscountCode == dis && x.CustomerAttributerLevel == cusSetting.AttributeID))
                                {
                                    exist = false;
                                }
                            }

                            if (exist)
                            {
                                var discount = new TpDiscountModel();
                                discount.Code = dis;
                                discount.ListCustomerAttribute = lstDisCusAttributeExists.Where(x => x.DiscountCode == dis).Select(x =>
                                                                  new CustomerAttributeModel() { AttributeMaster = x.CustomerAttributerLevel, Code = x.CustomerAttributerValue }).ToList();
                                tpDiscounts.Add(discount);
                            }

                        }
                    }
                }
                else if (input.ScopeType == CommonData.PromotionSetting.ScopeNationwide && input.ObjectType == CommonData.PromotionSetting.ObjectCustomerShipto)
                {
                    var tempDis2 = lstDiscounts.Where(x => x.ScopeType == CommonData.PromotionSetting.ScopeNationwide && x.ObjectType == CommonData.PromotionSetting.ObjectCustomerShipto).ToList();
                    var lstCustomerShiptos = input.ListCustomerShipto.Select(x => new CustomerShiptoMasterClass() { CustomerCode = x.CustomerCode, ShiptoCode = x.ShiptoCode }).ToList();
                    var lstDisCusAttributes = _dbDiscountShipto.GetAllQueryable().ToList().Where(x =>
                                                lstCustomerShiptos.Any(cus => cus.CustomerCode == x.CustomerCode && cus.ShiptoCode == x.CustomerShiptoCode)).ToList();
                    var lstDisCusShiptoExists = lstDisCusAttributes.Where(x => tempDis2.Select(d => d.Code).Contains(x.DiscountCode)).ToList();
                    if (lstDisCusShiptoExists != null && lstDisCusShiptoExists.Count > 0)
                    {
                        var discount = new TpDiscountModel();
                        discount.Code = lstDisCusShiptoExists.FirstOrDefault().DiscountCode;
                        discount.ListCustomerShipto = lstDisCusShiptoExists.Select(x => new CustomerShiptoModel() { CustomerCode = x.CustomerCode, ShiptoCode = x.CustomerShiptoCode }).ToList();
                        tpDiscounts.Add(discount);
                    }
                }
                else if (input.ScopeType == CommonData.PromotionSetting.ScopeSalesTerritoryLevel && input.ObjectType == CommonData.PromotionSetting.ObjectAllCustomer)
                {
                    var tempDis3 = lstDiscounts.Where(x => x.ScopeType == CommonData.PromotionSetting.ScopeSalesTerritoryLevel && x.ObjectType == CommonData.PromotionSetting.ObjectAllCustomer).ToList();
                    var lstInputScopeTerritorys = input.ListScopeSalesTerritory.Select(x => new ScopeTerritoryMasterClass() { TerritoryLevel = x.TerritoryLevelCode, TerritoryValue = x.Code }).ToList();
                    var lstDataScopeTerritorys = _dbDiscountScopeTerritory.GetAllQueryable().ToList().Where(x =>
                                                lstInputScopeTerritorys.Any(cus => cus.TerritoryLevel == x.ScopeSaleTerritoryLevel && cus.TerritoryValue == x.SalesTerritoryValue)).ToList();
                    var lstDisScopeTerritoryExists = lstDataScopeTerritorys.Where(x => tempDis3.Select(d => d.Code).Contains(x.DiscountCode)).ToList();
                    if (lstDisScopeTerritoryExists != null && lstDisScopeTerritoryExists.Count > 0)
                    {
                        var discount = new TpDiscountModel();
                        discount.Code = lstDisScopeTerritoryExists.FirstOrDefault().DiscountCode;
                        discount.ListScopeSalesTerritory = lstDisScopeTerritoryExists.Select(x => new TpSalesTerritoryValueModel() { TerritoryLevelCode = x.ScopeSaleTerritoryLevel, Code = x.SalesTerritoryValue }).ToList();
                        tpDiscounts.Add(discount);
                    }
                }
                else if (input.ScopeType == CommonData.PromotionSetting.ScopeSalesTerritoryLevel && input.ObjectType == CommonData.PromotionSetting.ObjectCustomerAttributes)
                {
                    var tempDis4 = lstDiscounts.Where(x => x.ScopeType == CommonData.PromotionSetting.ScopeSalesTerritoryLevel && x.ObjectType == CommonData.PromotionSetting.ObjectCustomerAttributes).ToList();

                    //Scope
                    var lstInputScopeTerritorys = input.ListScopeSalesTerritory.Select(x => new ScopeTerritoryMasterClass() { TerritoryLevel = x.TerritoryLevelCode, TerritoryValue = x.Code }).ToList();
                    var lstDataScopeTerritorys = _dbDiscountScopeTerritory.GetAllQueryable().ToList().Where(x =>
                                                lstInputScopeTerritorys.Any(cus => cus.TerritoryLevel == x.ScopeSaleTerritoryLevel && cus.TerritoryValue == x.SalesTerritoryValue)).ToList();
                    var lstDisScopeTerritoryExists = lstDataScopeTerritorys.Where(x => tempDis4.Select(d => d.Code).Contains(x.DiscountCode)).ToList();
                    if (lstDisScopeTerritoryExists != null && lstDisScopeTerritoryExists.Count > 0)
                    {
                        var discount = new TpDiscountModel();
                        discount.Code = lstDisScopeTerritoryExists.FirstOrDefault().DiscountCode;
                        discount.ListScopeSalesTerritory = lstDisScopeTerritoryExists.Select(x => new TpSalesTerritoryValueModel() { TerritoryLevelCode = x.ScopeSaleTerritoryLevel, Code = x.SalesTerritoryValue }).ToList();
                        tpDiscounts.Add(discount);
                    }

                    // Object
                    var lstCustomerAttributes = input.ListCustomerAttribute.Select(x => new CustomerAttributeMasterClass() { Level = x.AttributeMaster, Code = x.Code }).ToList();
                    var lstDisCusAttributes = _dbDiscountAttributeValue.GetAllQueryable().ToList().Where(x =>
                                                lstCustomerAttributes.Any(cus => cus.Level == x.CustomerAttributerLevel && cus.Code == x.CustomerAttributerValue)).ToList();
                    var lstDisCusAttributeExists = lstDisCusAttributes.Where(x => tempDis4.Select(d => d.Code).Contains(x.DiscountCode)).ToList();

                    var maxlevelCustomerInput = lstCustomerAttributes.OrderBy(x => x.Level).LastOrDefault().Level;
                    var lstDiscountExist = lstDisCusAttributeExists.Select(x => x.DiscountCode).Distinct();
                    var lstCustomerSettings = input.ListCustomerSetting.Where(x => x.IsChecked).ToList();
                    foreach (var dis in lstDiscountExist)
                    {
                        var maxLevel = lstDisCusAttributeExists.Where(x => x.DiscountCode == dis).OrderBy(x => x.CustomerAttributerLevel).LastOrDefault().CustomerAttributerLevel;
                        if (maxLevel == maxlevelCustomerInput)
                        {
                            bool exist = true;
                            foreach (var cusSetting in lstCustomerSettings)
                            {
                                if (!lstDisCusAttributeExists.Any(x => x.DiscountCode == dis && x.CustomerAttributerLevel == cusSetting.AttributeID))
                                {
                                    exist = false;
                                }
                            }

                            if (exist)
                            {
                                var discount = new TpDiscountModel();
                                discount.Code = dis;
                                discount.ListCustomerAttribute = lstDisCusAttributeExists.Where(x => x.DiscountCode == dis).Select(x =>
                                                                  new CustomerAttributeModel() { AttributeMaster = x.CustomerAttributerLevel, Code = x.CustomerAttributerValue }).ToList();
                                tpDiscounts.Add(discount);
                            }

                        }
                    }
                    tpDiscounts = GetListDiscountExist(tpDiscounts);
                }
                else if (input.ScopeType == CommonData.PromotionSetting.ScopeSalesTerritoryLevel && input.ObjectType == CommonData.PromotionSetting.ObjectCustomerShipto)
                {
                    var tempDis5 = lstDiscounts.Where(x => x.ScopeType == CommonData.PromotionSetting.ScopeSalesTerritoryLevel && x.ObjectType == CommonData.PromotionSetting.ObjectCustomerShipto).ToList();

                    //Scope
                    var lstInputScopeTerritorys = input.ListScopeSalesTerritory.Select(x => new ScopeTerritoryMasterClass() { TerritoryLevel = x.TerritoryLevelCode, TerritoryValue = x.Code }).ToList();
                    var lstDataScopeTerritorys = _dbDiscountScopeTerritory.GetAllQueryable(x =>
                                                lstInputScopeTerritorys.Any(cus => cus.TerritoryLevel == x.ScopeSaleTerritoryLevel && cus.TerritoryValue == x.SalesTerritoryValue)).ToList();
                    var lstDisScopeTerritoryExists = lstDataScopeTerritorys.Where(x => tempDis5.Select(d => d.Code).Contains(x.DiscountCode)).ToList();
                    if (lstDisScopeTerritoryExists != null && lstDisScopeTerritoryExists.Count > 0)
                    {
                        var discount = new TpDiscountModel();
                        discount.Code = lstDisScopeTerritoryExists.FirstOrDefault().DiscountCode;
                        discount.ListScopeSalesTerritory = lstDisScopeTerritoryExists.Select(x => new TpSalesTerritoryValueModel() { TerritoryLevelCode = x.ScopeSaleTerritoryLevel, Code = x.SalesTerritoryValue }).ToList();
                        tpDiscounts.Add(discount);
                    }

                    var lstCustomerShiptos = input.ListCustomerShipto.Select(x => new CustomerShiptoMasterClass() { CustomerCode = x.CustomerCode, ShiptoCode = x.ShiptoCode }).ToList();
                    var lstDisCusAttributes = _dbDiscountShipto.GetAllQueryable().ToList().Where(x =>
                                                lstCustomerShiptos.Any(cus => cus.CustomerCode == x.CustomerCode && cus.ShiptoCode == x.CustomerShiptoCode)).ToList();
                    var lstDisCusShiptoExists = lstDisCusAttributes.Where(x => tempDis5.Select(d => d.Code).Contains(x.DiscountCode)).ToList();
                    if (lstDisCusShiptoExists != null && lstDisCusShiptoExists.Count > 0)
                    {
                        var discount = new TpDiscountModel();
                        discount.Code = lstDisCusShiptoExists.FirstOrDefault().DiscountCode;
                        discount.ListCustomerShipto = lstDisCusShiptoExists.Select(x => new CustomerShiptoModel() { CustomerCode = x.CustomerCode, ShiptoCode = x.CustomerShiptoCode }).ToList();
                        tpDiscounts.Add(discount);
                    }
                    tpDiscounts = GetListDiscountExist(tpDiscounts);
                }
                else if (input.ScopeType == CommonData.PromotionSetting.ScopeDSA && input.ObjectType == CommonData.PromotionSetting.ObjectAllCustomer)
                {
                    var tempDis6 = lstDiscounts.Where(x => x.ScopeType == CommonData.PromotionSetting.ScopeDSA && x.ObjectType == CommonData.PromotionSetting.ObjectAllCustomer).ToList();
                    var lstDataScopeDsas = _dbDiscountScopeDsa.GetAllQueryable().ToList().Where(x => input.ListScopeDSA.Select(d => d.Code).Contains(x.ScopeDsaValue)).ToList();
                    var lstDisScopeDsaExists = lstDataScopeDsas.Where(x => tempDis6.Select(d => d.Code).Contains(x.DiscountCode)).ToList();
                    if (lstDisScopeDsaExists != null && lstDisScopeDsaExists.Count > 0)
                    {
                        var discount = new TpDiscountModel();
                        discount.Code = lstDisScopeDsaExists.FirstOrDefault().DiscountCode;
                        discount.ListScopeDSA = lstDisScopeDsaExists.Select(x => new TpSalesOrgDsaModel() { Code = x.ScopeDsaValue }).ToList();
                        tpDiscounts.Add(discount);
                    }

                }
                else if (input.ScopeType == CommonData.PromotionSetting.ScopeDSA && input.ObjectType == CommonData.PromotionSetting.ObjectCustomerAttributes)
                {
                    var tempDis7 = lstDiscounts.Where(x => x.ScopeType == CommonData.PromotionSetting.ScopeDSA && x.ObjectType == CommonData.PromotionSetting.ObjectCustomerAttributes).ToList();
                    var lstDataScopeDsas = _dbDiscountScopeDsa.GetAllQueryable(x => input.ListScopeDSA.Select(d => d.Code).Contains(x.ScopeDsaValue)).ToList();
                    var lstDisScopeDsaExists = lstDataScopeDsas.Where(x => tempDis7.Select(d => d.Code).Contains(x.DiscountCode)).ToList();
                    if (lstDisScopeDsaExists != null && lstDisScopeDsaExists.Count > 0)
                    {
                        var discount = new TpDiscountModel();
                        discount.Code = lstDisScopeDsaExists.FirstOrDefault().DiscountCode;
                        discount.ListScopeDSA = lstDisScopeDsaExists.Select(x => new TpSalesOrgDsaModel() { Code = x.ScopeDsaValue }).ToList();
                        tpDiscounts.Add(discount);
                    }

                    // Object
                    var lstCustomerAttributes = input.ListCustomerAttribute.Select(x => new CustomerAttributeMasterClass() { Level = x.AttributeMaster, Code = x.Code }).ToList();
                    var lstDisCusAttributes = _dbDiscountAttributeValue.GetAllQueryable().ToList().Where(x =>
                                                lstCustomerAttributes.Any(cus => cus.Level == x.CustomerAttributerLevel && cus.Code == x.CustomerAttributerValue)).ToList();
                    var lstDisCusAttributeExists = lstDisCusAttributes.Where(x => tempDis7.Select(d => d.Code).Contains(x.DiscountCode)).ToList();
                    var maxlevelCustomerInput = lstCustomerAttributes.OrderBy(x => x.Level).LastOrDefault().Level;
                    var lstDiscountExist = lstDisCusAttributeExists.Select(x => x.DiscountCode).Distinct();
                    var lstCustomerSettings = input.ListCustomerSetting.Where(x => x.IsChecked).ToList();
                    foreach (var dis in lstDiscountExist)
                    {
                        var maxLevel = lstDisCusAttributeExists.Where(x => x.DiscountCode == dis).OrderBy(x => x.CustomerAttributerLevel).LastOrDefault().CustomerAttributerLevel;
                        if (maxLevel == maxlevelCustomerInput)
                        {
                            bool exist = true;
                            foreach (var cusSetting in lstCustomerSettings)
                            {
                                if (!lstDisCusAttributeExists.Any(x => x.DiscountCode == dis && x.CustomerAttributerLevel == cusSetting.AttributeID))
                                {
                                    exist = false;
                                }
                            }

                            if (exist)
                            {
                                var discount = new TpDiscountModel();
                                discount.Code = dis;
                                discount.ListCustomerAttribute = lstDisCusAttributeExists.Where(x => x.DiscountCode == dis).Select(x =>
                                                                  new CustomerAttributeModel() { AttributeMaster = x.CustomerAttributerLevel, Code = x.CustomerAttributerValue }).ToList();
                                tpDiscounts.Add(discount);
                            }

                        }
                    }
                    tpDiscounts = GetListDiscountExist(tpDiscounts);
                }
                else
                {
                    var tempDis8 = lstDiscounts.Where(x => x.ScopeType == CommonData.PromotionSetting.ScopeDSA && x.ObjectType == CommonData.PromotionSetting.ObjectCustomerShipto).ToList();
                    var lstDataScopeDsas = _dbDiscountScopeDsa.GetAllQueryable().ToList().Where(x => input.ListScopeDSA.Select(d => d.Code).Contains(x.ScopeDsaValue)).ToList();
                    var lstDisScopeDsaExists = lstDataScopeDsas.Where(x => tempDis8.Select(d => d.Code).Contains(x.DiscountCode)).ToList();
                    if (lstDisScopeDsaExists != null && lstDisScopeDsaExists.Count > 0)
                    {
                        var discount = new TpDiscountModel();
                        discount.Code = lstDisScopeDsaExists.FirstOrDefault().DiscountCode;
                        discount.ListScopeDSA = lstDisScopeDsaExists.Select(x => new TpSalesOrgDsaModel() { Code = x.ScopeDsaValue }).ToList();
                        tpDiscounts.Add(discount);
                    }

                    var lstCustomerShiptos = input.ListCustomerShipto.Select(x => new CustomerShiptoMasterClass() { CustomerCode = x.CustomerCode, ShiptoCode = x.ShiptoCode }).ToList();
                    var lstDisCusAttributes = _dbDiscountShipto.GetAllQueryable().ToList().Where(x =>
                                                lstCustomerShiptos.Any(cus => cus.CustomerCode == x.CustomerCode && cus.ShiptoCode == x.CustomerShiptoCode)).ToList();
                    var lstDisCusShiptoExists = lstDisCusAttributes.Where(x => tempDis8.Select(d => d.Code).Contains(x.DiscountCode)).ToList();
                    if (lstDisCusShiptoExists != null && lstDisCusShiptoExists.Count > 0)
                    {
                        var discount = new TpDiscountModel();
                        discount.Code = lstDisCusShiptoExists.FirstOrDefault().DiscountCode;
                        discount.ListCustomerShipto = lstDisCusShiptoExists.Select(x => new CustomerShiptoModel() { CustomerCode = x.CustomerCode, ShiptoCode = x.CustomerShiptoCode }).ToList();
                        tpDiscounts.Add(discount);
                    }
                    tpDiscounts = GetListDiscountExist(tpDiscounts);
                }
            }

            return tpDiscounts;
        }

        private List<TpDiscountModel> GetListDiscountExist(List<TpDiscountModel> tpDiscounts)
        {
            List<TpDiscountModel> result = new List<TpDiscountModel>();

            var temp = from d in tpDiscounts
                       group d by d.Code into g
                       select new { Code = g.Key, Count = g.Select(x => x.Code).Count() };

            if (temp != null && temp.Any(x => x.Count > 1))
            {
                return tpDiscounts.Where(x => x.Code == temp.FirstOrDefault().Code).ToList();
            }
            return result;
        }

        #region External API
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
                                                 && x.SaleOrg == request.SaleOrgCode && x.SicCode == request.SicCode
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

                if (result != null)
                {
                    // Structure define
                    var ListStructureDifines = _mapper.Map<List<TpDiscountStructureDetailModel>>(_dbDiscountStructureDetail.GetAllQueryable(x => x.DiscountCode == result.Code && x.DeleteFlag == 0).ToList());
                    result.ListDiscountStructureDetails = ListStructureDifines;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public async Task<DiscountResultDetailModel> DiscountResult(DiscountResultParameters request)
        {
            DiscountResultDetailModel result = new DiscountResultDetailModel();
            var discountGeneral = await _dbDiscount.GetAllQueryable(x => x.DeleteFlag == 0 && x.Code == request.DiscountCode).FirstOrDefaultAsync();
            if (discountGeneral != null && !string.IsNullOrEmpty(discountGeneral.Code))
            {
                var dataStructure = await _dbDiscountStructureDetail.GetAllQueryable(x => x.DiscountCode == request.DiscountCode
                && x.Id == request.DiscountLevelId).FirstOrDefaultAsync();
                if (dataStructure != null && !string.IsNullOrEmpty(dataStructure.DiscountCode))
                {
                    result.Code = discountGeneral.Code;
                    result.ShortName = discountGeneral.ShortName;
                    result.FullName = discountGeneral.FullName;
                    result.LevelName = dataStructure.NameDiscountLevel;
                    result.CheckBy = dataStructure.DiscountType;
                    result.LevelCheckValue = dataStructure.DiscountCheckValue;
                    result.LevelAmount = dataStructure.DiscountAmount;
                    result.LevelPercent = dataStructure.DiscountPercent;

                    if (dataStructure.DiscountType == CommonData.PromotionSetting.DiscountAmount)
                    {
                        if (request.PurchaseAmount >= result.LevelCheckValue)
                        {
                            result.DiscountAmount = dataStructure.DiscountAmount;
                        }
                        else
                        {
                            result.DiscountAmount = 0;
                        }
                    }
                    else
                    {
                        if (request.PurchaseAmount >= result.LevelCheckValue)
                        {
                            result.DiscountAmount = request.PurchaseAmount * dataStructure.DiscountPercent / 100;
                        }
                        else
                        {
                            result.DiscountAmount = 0;
                        }
                    }
                }
            }
            return result;
        }
        #endregion
    }
}
