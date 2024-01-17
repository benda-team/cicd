using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Infrastructure.Tp;
using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.Discount;
using ODTradePromotion.API.Models.Promotion;
using ODTradePromotion.API.Models.Settlement;
using ODTradePromotion.API.Services.Base;
using Sys.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ODTradePromotion.API.Services.Settlement
{
    public class SettlementService : ISettlementService
    {
        #region Property
        private readonly ILogger<SettlementService> _logger;
        private readonly IBaseRepository<TpSettlement> _dbSettlement;
        private readonly IBaseRepository<TpSettlementDetail> _dbSettlementDetail;
        private readonly IBaseRepository<TpSettlementObject> _dbSettlementObject;
        private readonly IBaseRepository<Distributor> _dbDistributor;
        private readonly IBaseRepository<SystemSetting> _dbSystemSetting;
        private readonly IBaseRepository<TpPromotion> _dbPromotion;
        private readonly IBaseRepository<Infrastructure.Tp.TpDiscount> _dbDiscount;
        private readonly IBaseRepository<InventoryItem> _dbSku;
        private readonly IBaseRepository<Uom> _dbUom;
        private readonly IBaseRepository<TpPromotionDefinitionStructure> _dbTpDefinitionStructure;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public SettlementService(ILogger<SettlementService> logger, IBaseRepository<TpSettlement> dbSettlement,
            IBaseRepository<TpSettlementDetail> dbSettlementDetail,
            IBaseRepository<TpSettlementObject> dbSettlementObject,
            IBaseRepository<Distributor> dbDistributor,
            IBaseRepository<SystemSetting> dbSystemSetting,
            IBaseRepository<TpPromotion> dbPromotion,
            IBaseRepository<Infrastructure.Tp.TpDiscount> dbDiscount,
            IBaseRepository<InventoryItem> dbSku,
            IBaseRepository<Uom> dbUom,
            IBaseRepository<TpPromotionDefinitionStructure> dbTpDefinitionStructure,
            IMapper mapper
            )
        {
            _logger = logger;
            _dbSettlement = dbSettlement;
            _dbSettlementDetail = dbSettlementDetail;
            _dbSettlementObject = dbSettlementObject;
            _dbDistributor = dbDistributor;
            _dbSystemSetting = dbSystemSetting;
            _dbPromotion = dbPromotion;
            _dbDiscount = dbDiscount;
            _dbSku = dbSku;
            _dbUom = dbUom;
            _dbTpDefinitionStructure = dbTpDefinitionStructure;
            _mapper = mapper;
        }
        #endregion

        #region Method

        public bool Delete(string code, string userlogin)
        {

            var dbTpSettlements = _dbSettlement.FirstOrDefault(x => x.DeleteFlag == 0 && x.Code.ToLower().Equals(code.ToLower()));
            if (dbTpSettlements != null)
            {
                // delete TpSettlements
                dbTpSettlements.DeleteFlag = 1;
                dbTpSettlements.UpdatedBy = userlogin;
                dbTpSettlements.UpdatedDate = DateTime.Now;
                _dbSettlement.Update(dbTpSettlements);

                //delete in TpSettlementPromotions
                var lstSettlementPromotion = _dbSettlementDetail.GetAllQueryable(x => x.DeleteFlag == 0
                && x.SettlementCode.ToLower().Equals(code.ToLower())).AsNoTracking().ToList();
                if (lstSettlementPromotion != null && lstSettlementPromotion.Count > 0)
                {
                    foreach (var item in lstSettlementPromotion)
                    {
                        item.DeleteFlag = 1;
                        item.UpdatedBy = userlogin;
                        item.UpdatedDate = DateTime.Now;
                    }
                    _dbSettlementDetail.UpdateRange(lstSettlementPromotion);
                }
                return true;
            }
            return false;
        }

        public TpSettlementModel GetSettlementByCode(string code)
        {
            // Get Settlement
            var returnValue = _mapper.Map<TpSettlementModel>(_dbSettlement
                .FirstOrDefault(x => x.DeleteFlag == 0 && x.Code.ToLower().Equals(code.ToLower())));
            return returnValue;
        }

        public TpSettlementModel GetSettlementDetailByCode(string code)
        {
            // Get Settlement
            var returnValue = _mapper.Map<TpSettlementModel>(_dbSettlement
                .FirstOrDefault(x => x.DeleteFlag == 0 && x.Code.ToLower().Equals(code.ToLower())));

            if (returnValue != null)
            {
                // Get Settlement Object
                var lstSettObjectWithPromotions = (from so in _dbSettlementObject
                                            .GetAllQueryable(x => x.DeleteFlag == 0
                                                && x.SettlementCode.ToLower().Equals(code.ToLower())
                                                && x.ProgramType.Equals(CommonData.PromotionSetting.PromotionProgram)).AsNoTracking()
                                                   join p in _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                                                   on so.PromotionDiscountCode.ToLower() equals p.Code.ToLower()
                                                   select new TpSettlementObjectModel()
                                                   {
                                                       Id = so.Id,
                                                       SettlementCode = so.SettlementCode,
                                                       ProgramType = so.ProgramType,
                                                       PromotionDiscountCode = so.PromotionDiscountCode,
                                                       PromotionDiscountName = p.ShortName,
                                                       PromotionGeneralModel = _mapper.Map<TpPromotionGeneralModel>(p)
                                                   }).ToList();

                var lstSettObjectWithDiscounts = (from so in _dbSettlementObject
                                            .GetAllQueryable(x => x.DeleteFlag == 0
                                                && x.SettlementCode.ToLower().Equals(code.ToLower())
                                                && x.ProgramType.Equals(CommonData.PromotionSetting.DiscountProgram)).AsNoTracking()
                                                  join d in _dbDiscount.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                                                  on so.PromotionDiscountCode.ToLower() equals d.Code.ToLower()
                                                  select new TpSettlementObjectModel()
                                                  {
                                                      Id = so.Id,
                                                      SettlementCode = so.SettlementCode,
                                                      ProgramType = so.ProgramType,
                                                      PromotionDiscountCode = so.PromotionDiscountCode,
                                                      PromotionDiscountName = d.ShortName,
                                                      DiscountModel = _mapper.Map<TpDiscountModel>(d)
                                                  }).ToList();
                var lstSettObjects = new List<TpSettlementObjectModel>();
                lstSettObjects.AddRange(lstSettObjectWithPromotions);
                lstSettObjects.AddRange(lstSettObjectWithDiscounts);
                returnValue.ListTpSettlementObjects = lstSettObjects;

                if (returnValue.SchemeType)
                {
                    var lstSettlementPromotion = (from s in _dbSettlementObject.GetAllQueryable(x => x.DeleteFlag == 0 && x.SettlementCode.ToLower().Equals(code.ToLower()))
                                                  join p in _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0)
                                                  on s.PromotionDiscountCode.ToLower() equals p.Code.ToLower()
                                                  select new TpPromotionGeneralModel()
                                                  {
                                                      Id = p.Id,
                                                      PromotionType = p.PromotionType,
                                                      Code = p.Code,
                                                      ShortName = p.ShortName,
                                                      FullName = p.FullName,
                                                      Status = p.Status,
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
                                                  }).ToList();
                    returnValue.ListPromotionModels = lstSettlementPromotion;
                }

                // Get Settlement Detail
                var lstSettlementDetail = (from x in _dbSettlementDetail.GetAllQueryable(x => x.DeleteFlag == 0 && x.SettlementCode.ToLower().Equals(code.ToLower()))
                                           join d in _dbDistributor.GetAllQueryable(x => x.DeleteFlag == 0)
                                           on x.DistributorCode.ToLower() equals d.Code.ToLower() into emptyDistributor
                                           from d in emptyDistributor.DefaultIfEmpty()
                                           join sku in _dbSku.GetAllQueryable(x => x.DelFlg == 0)
                                           on x.ProductCode.ToLower() equals sku.InventoryItemId.ToLower() into emptySku
                                           from sku in emptySku.DefaultIfEmpty()
                                           join uom in _dbUom.GetAllQueryable(x => x.DeleteFlag == 0)
                                           on x.Package.ToLower() equals uom.UomId.ToLower() into emptyUom
                                           from uom in emptyUom.DefaultIfEmpty()
                                           orderby x.DistributorCode, x.ProductCode, x.Package
                                           select new TpSettlementDetailModel()
                                           {
                                               SettlementCode = code,
                                               OrdNbr = x.OrdNbr,
                                               ProgramType = x.ProgramType,
                                               PromotionDiscountCode = x.PromotionDiscountCode,
                                               DistributorCode = x.DistributorCode,
                                               DistributorName = d.Name,
                                               ProductCode = x.ProductCode,
                                               ProductName = sku.Description,
                                               Package = x.Package,
                                               PackageName = uom.Description,
                                               Quantity = x.Quantity,
                                               Amount = x.Amount,
                                               Status = x.Status
                                           }).ToList();
                returnValue.TpSettlementDetailModels = lstSettlementDetail;
            }
            return returnValue;
        }

        public List<string> SuggestionCode(string keyWord)
        {
            var data = _dbSettlement.GetAll().Where(e => e.Code.Contains(keyWord) && e.DeleteFlag == 0).Select(e => e.Code).Take(10).ToList();
            return data;
        }

        public List<TpSettlementModel> GetListSettlement()
        {
            var lstSettObjectWithPromotions = (from so in _dbSettlementObject.GetAllQueryable(x => x.DeleteFlag == 0 && x.ProgramType.Equals(CommonData.PromotionSetting.PromotionProgram))
                                               join pr in _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0)
                                               on so.PromotionDiscountCode.ToLower() equals pr.Code.ToLower()
                                               select new TpSettlementObjectModel()
                                               {
                                                   SettlementCode = so.SettlementCode,
                                                   PromotionDiscountCode = so.PromotionDiscountCode,
                                                   PromotionDiscountName = pr.ShortName
                                               }).ToList();

            var result1 = (from s in _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0 && x.ProgramType.Equals(CommonData.PromotionSetting.PromotionProgram))
                           join status in _dbSystemSetting.GetAllQueryable(x => x.SettingType.Equals(CommonData.SystemSetting.SettlementPromotionStatus) && x.IsActive).AsNoTracking()
                           on s.Status equals status.SettingKey
                           select new TpSettlementModel()
                           {
                               Id = s.Id,
                               Code = s.Code,
                               Name = s.Name,
                               ProgramType = s.ProgramType,
                               SettlementTypeName = s.ProgramType.Equals(CommonData.PromotionSetting.PromotionProgram) ? "Promotion" : "Discount",
                               Status = s.Status,
                               StatusName = status.Description,
                               SettlementDate = s.SettlementDate
                           }).ToList();

            foreach (var item in result1)
            {
                item.PromotionDiscountName = String.Join(",", lstSettObjectWithPromotions.Where(x => x.SettlementCode.ToLower().Equals(item.Code.ToLower())).Select(y => y.PromotionDiscountName).ToList());
            }

            var result2 = (from s in _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0 && x.ProgramType.Equals(CommonData.PromotionSetting.DiscountProgram))
                           join status in _dbSystemSetting.GetAllQueryable(x => x.SettingType.Equals(CommonData.SystemSetting.SettlementPromotionStatus) && x.IsActive).AsNoTracking()
                           on s.Status equals status.SettingKey
                           join so in _dbSettlementObject.GetAllQueryable(x => x.DeleteFlag == 0 && x.ProgramType.Equals(CommonData.PromotionSetting.DiscountProgram))
                           on s.Code.ToLower() equals so.SettlementCode.ToLower()
                           join dc in _dbDiscount.GetAllQueryable(x => x.DeleteFlag == 0)
                           on so.PromotionDiscountCode.ToLower() equals dc.Code.ToLower()
                           select new TpSettlementModel()
                           {
                               Id = s.Id,
                               Code = s.Code,
                               Name = s.Name,
                               ProgramType = s.ProgramType,
                               SettlementTypeName = s.ProgramType.Equals(CommonData.PromotionSetting.PromotionProgram) ? "Promotion" : "Discount",
                               Status = s.Status,
                               StatusName = status.Description,
                               SettlementDate = s.SettlementDate,
                               PromotionDiscountName = dc.ShortName
                           }).ToList();
            result1.AddRange(result2);
            return result1;
        }
        public IQueryable<TpSettlementModel> GetListSettlementForPopup()
        {
            var result = (from s in _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                          join status in _dbSystemSetting.GetAllQueryable(x => x.SettingType.Equals(CommonData.SystemSetting.SettlementPromotionStatus) && x.IsActive).AsNoTracking()
                           on s.Status equals status.SettingKey
                          select new TpSettlementModel()
                          {
                              Id = s.Id,
                              Code = s.Code,
                              Name = s.Name,
                              Status = s.Status,
                              StatusName = status.Description,
                          }).AsQueryable().AsNoTracking();
            return result;
        }
        public IQueryable<TpSettlementSearchModel> GetListSettlementCode(string status)
        {

            var ressultSettlementListView = _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                .Where(x => x.Status.Equals(CommonData.SettlementPromotion.confirmed))
                .Select(x => new TpSettlementSearchModel { Code = x.Code, Name = x.Name })
                .OrderBy(x => x.Name)
                .AsQueryable();
            var ressultSettlementListConfirm = _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                .Where(x => x.Status.Equals(CommonData.SettlementPromotion.WaitConfirm) && status.Equals(CommonData.SettlementPromotion.Inprogress))
                .Select(x => new TpSettlementSearchModel { Code = x.Code, Name = x.Name })
                .OrderBy(x => x.Name)
                .AsQueryable();
            if (status.Equals(CommonData.SettlementPromotion.confirmed))
                return ressultSettlementListView;
            else if (status.Equals(CommonData.SettlementPromotion.WaitConfirm) || status.Equals(CommonData.SettlementPromotion.Inprogress))
                return ressultSettlementListConfirm;
            else return null;
        }

        public BaseResultModel CreateSettlement(TpSettlementModel input, string userLogin)
        {
            try
            {
                // Create promotion
                var settlement = _mapper.Map<TpSettlement>(input);
                settlement.Id = Guid.NewGuid();
                settlement.CreatedBy = userLogin;
                settlement.CreatedDate = DateTime.Now;
                settlement.DeleteFlag = 0;
                _dbSettlement.Insert(settlement);

                // Create Settlement Promotion
                if (input.ListTpSettlementObjects != null && input.ListTpSettlementObjects.Count > 0)
                {
                    var settlementObjects = _mapper.Map<List<TpSettlementObject>>(input.ListTpSettlementObjects);
                    foreach (var item in settlementObjects)
                    {
                        item.Id = Guid.NewGuid();
                        item.CreatedBy = userLogin;
                        item.CreatedDate = DateTime.Now;
                        item.DeleteFlag = 0;
                    }
                    _dbSettlementObject.InsertRange(settlementObjects);
                }

                // create list SettlementDetail
                var lstSettlementDetail = _mapper.Map<List<TpSettlementDetail>>(input.TpSettlementDetailModels);
                foreach (var item in lstSettlementDetail)
                {
                    item.Id = Guid.NewGuid();
                    item.SettlementCode = input.Code;
                    item.CreatedBy = userLogin;
                    item.CreatedDate = DateTime.Now;
                    item.DeleteFlag = 0;
                }
                _dbSettlementDetail.InsertRange(lstSettlementDetail);

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

        public BaseResultModel UpdateSettlement(TpSettlementModel input, string userLogin)
        {
            try
            {
                var settlementDb = _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0 && x.Code.ToLower().Equals(input.Code.ToLower())).FirstOrDefault();
                if (settlementDb != null)
                {
                    var settlementId = settlementDb.Id;
                    var settlement = _mapper.Map<TpSettlement>(input);
                    settlement.Id = settlementId;
                    settlement.UpdatedDate = DateTime.Now;
                    settlement.UpdatedBy = userLogin;
                    _dbSettlement.Update(settlement);

                    // Delete settlement Object
                    var lstSettlemetObjects = _dbSettlementObject.GetAllQueryable(x => x.DeleteFlag == 0 && x.SettlementCode.ToLower().Equals(input.Code.ToLower()));
                    _dbSettlementObject.DeleteRange(lstSettlemetObjects);

                    // Create Settlement Promotion
                    if (input.ListTpSettlementObjects != null && input.ListTpSettlementObjects.Count > 0)
                    {
                        var settlementObjects = _mapper.Map<List<TpSettlementObject>>(input.ListTpSettlementObjects);
                        foreach (var item in settlementObjects)
                        {
                            item.Id = Guid.NewGuid();
                            item.CreatedBy = userLogin;
                            item.CreatedDate = DateTime.Now;
                            item.DeleteFlag = 0;
                        }
                        _dbSettlementObject.InsertRange(settlementObjects);
                    }

                    // Delete settlement Details
                    var lstSettlement = _dbSettlementDetail.GetAllQueryable(x => x.DeleteFlag == 0 && x.SettlementCode.ToLower().Equals(input.Code.ToLower()));
                    _dbSettlementDetail.DeleteRange(lstSettlement);

                    // create list SettlementDetail
                    var lstSettlementDetail = _mapper.Map<List<TpSettlementDetail>>(input.TpSettlementDetailModels);
                    foreach (var item in lstSettlementDetail)
                    {
                        item.Id = Guid.NewGuid();
                        item.SettlementCode = input.Code;
                        item.CreatedBy = userLogin;
                        item.CreatedDate = DateTime.Now;
                        item.DeleteFlag = 0;
                    }
                    _dbSettlementDetail.InsertRange(lstSettlementDetail);
                }

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

        public BaseResultModel ConfirmSettlementByDistributor(string distributorCode, List<string> lstInput)
        {
            try
            {
                var lstSettlement = _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0 && lstInput.Contains(x.Code)).ToList();
                if (lstSettlement != null && lstSettlement.Any())
                {
                    // update settlement
                    lstSettlement.ForEach(x => x.Status = CommonData.PromotionSetting.Confirmed);
                    _dbSettlement.UpdateRange(lstSettlement);

                    // update settlement Details
                    var lstSettlementDetail = _dbSettlementDetail.GetAllQueryable(x => x.DeleteFlag == 0 &&
                    x.DistributorCode.ToLower().Equals(distributorCode.ToLower()) && lstInput.Contains(x.SettlementCode)).ToList();

                    if (lstSettlementDetail != null && lstSettlementDetail.Any())
                    {
                        lstSettlementDetail.ForEach(x => x.Status = CommonData.SettlementPromotion.Confirm);
                        _dbSettlementDetail.UpdateRange(lstSettlementDetail);
                    }
                    _dbSettlement.Save();
                }

                return new BaseResultModel
                {
                    IsSuccess = true,
                    Code = 201,
                    Message = "ConfirmSuccess"
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

        public List<TpSettlementConfirmModel> GetListSettlementConfirm()
        {
            var distributors = _dbDistributor.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking();
            var products = _dbSku.GetAllQueryable(x => x.DelFlg == 0).AsNoTracking();
            var promotions = _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking();
            var discounts = _dbDiscount.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking();
            var systemSettings = _dbSystemSetting.GetAllQueryable(x => x.IsActive && x.SettingType.Equals(CommonData.SystemSetting.DistributorConfirmSettlement)).AsQueryable();

            List<TpSettlementConfirmModel> result = new List<TpSettlementConfirmModel>();

            var result1 = (from sett in _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0 && x.Status != CommonData.SettlementPromotion.Inprogress).AsNoTracking()
                           join settobj in _dbSettlementObject.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                           on sett.Code.ToLower() equals settobj.SettlementCode.ToLower()
                           join settdet in _dbSettlementDetail.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                           on sett.Code.ToLower() equals settdet.SettlementCode.ToLower()
                           join sta in systemSettings on settdet.Status.ToLower() equals sta.SettingKey.ToLower()
                           join pro in promotions on settobj.PromotionDiscountCode.ToLower() equals pro.Code.ToLower()
                           join dis in distributors on settdet.DistributorCode.ToLower() equals dis.Code.ToLower()
                           join sku in products on settdet.ProductCode.ToLower() equals sku.InventoryItemId.ToLower() into skuData
                           from sku in skuData.DefaultIfEmpty()
                           where sett.ProgramType.Equals(CommonData.PromotionSetting.PromotionProgram)
                           group settdet by new TpSettlementConfirmModel()
                           {
                               SettlementCode = sett.Code,
                               SettlementName = sett.Name,
                               ProgramType = sett.ProgramType,
                               DistributorCode = settdet.DistributorCode,
                               DistributorName = dis.Name,
                               PromotionCode = settobj.PromotionDiscountCode,
                               PromotionName = pro.ShortName,
                               ProductCode = settdet.ProductCode,
                               ProductName = sku != null ? sku.ShortName : string.Empty,
                               Status = settdet.Status,
                               StatusDescription = sta.Description
                           } into gsett
                           select new TpSettlementConfirmModel()
                           {
                               SettlementCode = gsett.Key.SettlementCode,
                               SettlementName = gsett.Key.SettlementName,
                               ProgramType = gsett.Key.ProgramType,
                               DistributorCode = gsett.Key.DistributorCode,
                               DistributorName = gsett.Key.DistributorName,
                               PromotionCode = gsett.Key.PromotionCode,
                               PromotionName = gsett.Key.PromotionName,
                               ProductCode = gsett.Key.ProductCode,
                               ProductName = gsett.Key.ProductName,
                               Status = gsett.Key.Status,
                               StatusDescription = gsett.Key.StatusDescription,
                               Quantity = (gsett.Select(x => x.Quantity).Sum() == null || gsett.Select(x => x.Quantity).Sum() == 0) ? null : gsett.Select(x => x.Quantity).Sum(),
                               Amount = (gsett.Select(x => x.Amount).Sum() == null || gsett.Select(x => x.Amount).Sum() == 0) ? null : gsett.Select(x => x.Amount).Sum()
                           }).ToList();

            var result2 = (from sett in _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0 && x.Status != CommonData.SettlementPromotion.Inprogress).AsNoTracking()
                           join settobj in _dbSettlementObject.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                           on sett.Code.ToLower() equals settobj.SettlementCode.ToLower()
                           join settdet in _dbSettlementDetail.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                           on sett.Code.ToLower() equals settdet.SettlementCode.ToLower()
                           join sta in systemSettings on settdet.Status.ToLower() equals sta.SettingKey.ToLower()
                           join discount in discounts on settobj.PromotionDiscountCode.ToLower() equals discount.Code.ToLower()
                           join dis in distributors on settdet.DistributorCode.ToLower() equals dis.Code.ToLower()
                           where sett.ProgramType.Equals(CommonData.PromotionSetting.DiscountProgram)
                           group settdet by new TpSettlementConfirmModel()
                           {
                               SettlementCode = sett.Code,
                               SettlementName = sett.Name,
                               ProgramType = sett.ProgramType,
                               DistributorCode = settdet.DistributorCode,
                               DistributorName = dis.Name,
                               PromotionCode = settobj.PromotionDiscountCode,
                               PromotionName = discount.ShortName,
                               Status = settdet.Status,
                               StatusDescription = sta.Description
                           } into gsett
                           select new TpSettlementConfirmModel()
                           {
                               SettlementCode = gsett.Key.SettlementCode,
                               SettlementName = gsett.Key.SettlementName,
                               ProgramType = gsett.Key.ProgramType,
                               DistributorCode = gsett.Key.DistributorCode,
                               DistributorName = gsett.Key.DistributorName,
                               PromotionCode = gsett.Key.PromotionCode,
                               PromotionName = gsett.Key.PromotionName,
                               Status = gsett.Key.Status,
                               StatusDescription = gsett.Key.StatusDescription,
                               ProductCode = string.Empty,
                               ProductName = string.Empty,
                               Quantity = null,
                               Amount = (gsett.Select(x => x.Amount).Sum() == null || gsett.Select(x => x.Amount).Sum() == 0) ? null : gsett.Select(x => x.Amount).Sum()
                           }).ToList();

            result.AddRange(result1);
            result.AddRange(result2);
            return result;
        }

        public IQueryable<TpSettlementConfirmModel> GetListSettlementConfirmByDistributor(string distributorCode)
        {
            var distributors = _dbDistributor.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking();
            var systemSettings = _dbSystemSetting.GetAllQueryable(x => x.IsActive).AsQueryable();

            var result = (from sett in _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0 && x.Status == CommonData.SettlementPromotion.Confirm).AsNoTracking()
                          join settobj in _dbSettlementObject.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                          on sett.Code.ToLower() equals settobj.SettlementCode.ToLower()
                          join settdet in _dbSettlementDetail.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                          on sett.Code.ToLower() equals settdet.SettlementCode.ToLower()
                          join sta in systemSettings.Where(x => x.SettingType == CommonData.SystemSetting.DistributorConfirmSettlement).AsNoTracking()
                          on settdet.Status.ToLower() equals sta.SettingKey.ToLower()
                          join programType in systemSettings.Where(x => x.SettingType == CommonData.SystemSetting.TpType).AsNoTracking()
                          on settdet.ProgramType.ToLower() equals programType.SettingKey.ToLower()
                          join dis in distributors on settdet.DistributorCode.ToLower() equals dis.Code.ToLower()
                          where settdet.DistributorCode.ToLower().Equals(distributorCode.ToLower())
                          && settdet.Status.ToLower().Equals(CommonData.SettlementPromotion.Create.ToLower())
                          group settdet by new TpSettlementConfirmModel()
                          {
                              SettlementCode = sett.Code,
                              SettlementName = sett.Name,
                              ProgramType = sett.ProgramType,
                              ProgramName = programType.Description,
                              DistributorCode = settdet.DistributorCode,
                              DistributorName = dis.Name,
                              Status = settdet.Status,
                              StatusDescription = sta.Description
                          } into gsett
                          select new TpSettlementConfirmModel()
                          {
                              SettlementCode = gsett.Key.SettlementCode,
                              SettlementName = gsett.Key.SettlementName,
                              ProgramType = gsett.Key.ProgramType,
                              ProgramName = gsett.Key.ProgramName,
                              DistributorCode = gsett.Key.DistributorCode,
                              DistributorName = gsett.Key.DistributorName,
                              Status = gsett.Key.Status,
                              StatusDescription = gsett.Key.StatusDescription,
                              Quantity = (gsett.Select(x => x.Quantity).Sum() == null || gsett.Select(x => x.Quantity).Sum() == 0) ? null : gsett.Select(x => x.Quantity).Sum(),
                              Amount = (gsett.Select(x => x.Amount).Sum() == null || gsett.Select(x => x.Amount).Sum() == 0) ? null : gsett.Select(x => x.Amount).Sum()
                          }).AsQueryable();

            return result;
        }

        public List<TpDefinitionStructureForSettlementModel> GetsTpDefinitionStructuresForSettlement(string distributorCode)
        {
            var result = new List<TpDefinitionStructureForSettlementModel>();
            var dataSettlements = (from sett in _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0 && x.Status == CommonData.SettlementPromotion.Confirm).AsNoTracking()
                                   join settdet in _dbSettlementDetail.GetAllQueryable(x => x.DeleteFlag == 0 && x.ProgramType == CommonData.PromotionSetting.PromotionProgram).AsNoTracking()
                                   on sett.Code.ToLower() equals settdet.SettlementCode.ToLower()
                                   where settdet.DistributorCode.ToLower().Equals(distributorCode.ToLower())
                                   && settdet.Status.ToLower().Equals(CommonData.SettlementPromotion.Create.ToLower())
                                   select new
                                   {
                                       sett.Code,
                                       settdet.PromotionDiscountCode
                                   }).Distinct().ToList();

            if (dataSettlements != null && dataSettlements.Any())
            {
                var lstCodePromotions = dataSettlements.Select(x => x.PromotionDiscountCode).ToList();
                var lstDataTpDefinitionStructure = _dbTpDefinitionStructure.GetAllQueryable(x => x.DeleteFlag == 0 && lstCodePromotions.Contains(x.PromotionCode)).AsNoTracking().ToList();
                foreach (var item in dataSettlements)
                {
                    var definitionStructure = lstDataTpDefinitionStructure.FirstOrDefault(x => x.PromotionCode == item.PromotionDiscountCode);
                    var model = new TpDefinitionStructureForSettlementModel()
                    {
                        SettlementCode = item.Code,
                        IsDonate = definitionStructure != null ? definitionStructure.IsDonate : false,
                        IsFixMoney = definitionStructure != null ? definitionStructure.IsFixMoney : false,
                        IsGiftProduct = definitionStructure != null ? definitionStructure.IsGiftProduct : false
                    };
                    result.Add(model);
                }
            }
            return result;
        }
        public IQueryable<TpSettlementObjectWithCalendarModel> GetSettlementByPromotionCodeBySaleCalendar(string code, string calendar)
        {
            return (from p in _dbSettlementObject.GetAllQueryable(x => x.DeleteFlag == 0 && x.ProgramType.Equals(CommonData.PromotionSetting.PromotionProgram)).AsNoTracking()
                    join s in _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0 && x.ProgramType.Equals(CommonData.PromotionSetting.PromotionProgram)).AsNoTracking()
                    on p.SettlementCode.ToLower() equals s.Code.ToLower()
                    where p.PromotionDiscountCode.ToLower().Equals(code.ToLower()) && s.SaleCalendarCode.ToLower().Equals(calendar.ToLower())
                    select new TpSettlementObjectWithCalendarModel()
                    {
                        ProgramType = p.ProgramType,
                        SettlementCode = p.SettlementCode,
                        PromotionDiscountCode = p.PromotionDiscountCode,
                        SaleCalendarCode = s.SaleCalendarCode
                    }).AsQueryable();
        }

        public IQueryable<TpSettlementObjectWithCalendarModel> GetSettlementByDiscountCodeBySaleCalendar(string code, string calendar)
        {
            return (from p in _dbSettlementObject.GetAllQueryable(x => x.DeleteFlag == 0 && x.ProgramType.Equals(CommonData.PromotionSetting.DiscountProgram)).AsNoTracking()
                    join s in _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0 && x.ProgramType.Equals(CommonData.PromotionSetting.DiscountProgram)).AsNoTracking()
                    on p.SettlementCode.ToLower() equals s.Code.ToLower()
                    where p.PromotionDiscountCode.ToLower().Equals(code.ToLower()) && s.SaleCalendarCode.ToLower().Equals(calendar.ToLower())
                    select new TpSettlementObjectWithCalendarModel()
                    {
                        ProgramType = p.ProgramType,
                        SettlementCode = p.SettlementCode,
                        PromotionDiscountCode = p.PromotionDiscountCode,
                        SaleCalendarCode = s.SaleCalendarCode
                    }).AsQueryable();
        }

        public IQueryable<TpSettlementDetailModel> GetListDetailSettlementConfirmByDistributor(string code, string distributorCode)
        {
            var result = (from td in _dbSettlementDetail.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()

                          join u in _dbUom.GetAllQueryable().AsNoTracking() on td.Package.ToLower() equals u.UomId.ToLower()
                          into uData
                          from u in uData.DefaultIfEmpty()

                          join sku in _dbSku.GetAllQueryable(x => x.DelFlg == 0).AsNoTracking()
                          on td.ProductCode.ToLower() equals sku.InventoryItemId.ToLower()
                          into skuData
                          from sku in skuData.DefaultIfEmpty()

                          where td.SettlementCode.ToLower().Equals(code.ToLower()) && td.DistributorCode.ToLower().Equals(distributorCode.ToLower())
                          select new TpSettlementDetailModel()
                          {
                              SettlementCode = td.SettlementCode,
                              OrdNbr = string.IsNullOrEmpty(td.OrdNbr) ? string.Empty : td.OrdNbr,
                              ProgramType = td.ProgramType,
                              PromotionDiscountCode = td.PromotionDiscountCode,
                              PromotionDiscountName = string.IsNullOrEmpty(td.PromotionDiscountName) ? string.Empty : td.PromotionDiscountName,
                              DistributorCode = td.DistributorCode,
                              ProductCode = string.IsNullOrEmpty(td.ProductCode) ? string.Empty : td.ProductCode,
                              ProductName = (sku != null) ? sku.ShortName : string.Empty,
                              Package = string.IsNullOrEmpty(td.Package) ? string.Empty : td.Package,
                              PackageName = (u != null) ? u.Description : string.Empty,
                              Quantity = td.Quantity,
                              Amount = td.Amount,
                              OrdDate = td.OrdDate,
                              CustomerID = string.IsNullOrEmpty(td.CustomerID) ? string.Empty : td.CustomerID,
                              PromotionLevel = string.IsNullOrEmpty(td.PromotionLevel) ? string.Empty : td.PromotionLevel,
                              PromotionLevelName = string.IsNullOrEmpty(td.PromotionLevelName) ? string.Empty : td.PromotionLevelName,
                              ReferenceLink = string.IsNullOrEmpty(td.ReferenceLink) ? string.Empty : td.ReferenceLink,
                              SalesRepCode = string.IsNullOrEmpty(td.SalesRepCode) ? string.Empty : td.SalesRepCode,
                              ShiptoID = string.IsNullOrEmpty(td.ShiptoID) ? string.Empty : td.ShiptoID,
                              ShiptoName = string.IsNullOrEmpty(td.ShiptoName) ? string.Empty : td.ShiptoName
                          }).AsQueryable().AsNoTracking();

            return result;
        }
        #endregion
    }
}
