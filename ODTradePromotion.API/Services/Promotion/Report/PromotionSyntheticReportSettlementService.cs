using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Infrastructure.Tp;
using ODTradePromotion.API.Models.Promotion;
using ODTradePromotion.API.Models.Report;
using ODTradePromotion.API.Services.Base;
using Sys.Common.Constants;
using System.Linq;

namespace ODTradePromotion.API.Services.Promotion.Report
{
    public class PromotionSyntheticReportSettlementService : IPromotionSyntheticReportSettlementService
    {
        #region Property
        private readonly ILogger<PromotionSyntheticReportSettlementService> _logger;
        private readonly IBaseRepository<TpSettlement> _dbSettlement;
        private readonly IBaseRepository<TpSettlementDetail> _dbSettlementDetail;
        private readonly IBaseRepository<TpSettlementObject> _dbSettlementObject;
        private readonly IBaseRepository<Distributor> _dbDistributor;
        private readonly IBaseRepository<SystemSetting> _dbSystemSetting;
        private readonly IBaseRepository<TpPromotion> _dbPromotion;
        private readonly IBaseRepository<Infrastructure.Tp.TpDiscount> _dbDiscount;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public PromotionSyntheticReportSettlementService(ILogger<PromotionSyntheticReportSettlementService> logger,
            IBaseRepository<TpSettlement> dbSettlement,
            IBaseRepository<TpSettlementDetail> dbSettlementDetail,
            IBaseRepository<TpSettlementObject> dbSettlementObject,
            IBaseRepository<Distributor> dbDistributor,
            IBaseRepository<SystemSetting> dbSystemSetting,
            IBaseRepository<TpPromotion> dbPromotion,
            IBaseRepository<Infrastructure.Tp.TpDiscount> dbDiscount,
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
            _mapper = mapper;
        }
        #endregion

        #region Method
        public IQueryable<PromotionDiscountForPopupReportModel> GetListPromotionSettlementForPopup()
        {
            string ctkmDescription = _dbSystemSetting.FirstOrDefault(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.TpType.ToLower())
            && x.SettingKey.ToLower().Equals(CommonData.PromotionSetting.PromotionProgram.ToLower())).Description;
            string ctckDescription = _dbSystemSetting.FirstOrDefault(x => x.SettingType.ToLower().Equals(CommonData.SystemSetting.TpType.ToLower())
            && x.SettingKey.ToLower().Equals(CommonData.PromotionSetting.DiscountProgram.ToLower())).Description;

            var resultPromotions = from ob in _dbSettlementObject.GetAllQueryable(x => x.DeleteFlag == 0
                                    && x.ProgramType.Equals(CommonData.PromotionSetting.PromotionProgram)).AsNoTracking()

                                   join s in _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0
                                   && x.ProgramType.Equals(CommonData.PromotionSetting.PromotionProgram) && x.Status.Equals(CommonData.PromotionSetting.Confirmed)).AsNoTracking()
                                   on ob.SettlementCode equals s.Code

                                   join p in _dbPromotion.GetAllQueryable(x => x.DeleteFlag == 0
                                   && x.Status.ToLower().Equals(CommonData.PromotionSetting.Confirmed.ToLower())).AsNoTracking()
                                   on ob.PromotionDiscountCode equals p.Code into pData
                                   from p in pData.DefaultIfEmpty()
                                   select new PromotionDiscountForPopupReportModel()
                                   {
                                       Id = (p != null) ? p.Id : System.Guid.Empty,
                                       Code = ob.PromotionDiscountCode,
                                       ShortName = (p != null) ? p.ShortName : string.Empty,
                                       FullName = (p != null) ? p.FullName : string.Empty,
                                       ProgramType = CommonData.PromotionSetting.PromotionProgram,
                                       ProgramTypeDescription = ctkmDescription
                                   };

            var resultDiscounts = from ob in _dbSettlementObject.GetAllQueryable(x => x.DeleteFlag == 0
                                    && x.ProgramType.Equals(CommonData.PromotionSetting.DiscountProgram)).AsNoTracking()

                                  join s in _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0
                                  && x.ProgramType.Equals(CommonData.PromotionSetting.DiscountProgram) && x.Status.Equals(CommonData.PromotionSetting.Confirmed)).AsNoTracking()
                                  on ob.SettlementCode equals s.Code

                                  join p in _dbDiscount.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                                  on ob.PromotionDiscountCode equals p.Code into pData
                                  from p in pData.DefaultIfEmpty()
                                  select new PromotionDiscountForPopupReportModel()
                                  {
                                      Id = (p != null) ? p.Id : System.Guid.Empty,
                                      Code = ob.PromotionDiscountCode,
                                      ShortName = (p != null) ? p.ShortName : string.Empty,
                                      FullName = (p != null) ? p.FullName : string.Empty,
                                      ProgramType = CommonData.PromotionSetting.DiscountProgram,
                                      ProgramTypeDescription = ctckDescription
                                  };

            return resultPromotions.Union(resultDiscounts).Distinct();
        }

        public int CountSettlementQuantity(string promotionDiscountCode)
        {
            var resultData = from ob in _dbSettlementObject.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                             where ob.PromotionDiscountCode.ToLower().Equals(promotionDiscountCode.ToLower())
                             select ob.PromotionDiscountCode;
            return resultData.Count();
        }

        public IQueryable<PromotionSyntheticReportSettlementListModel> GetListPromotionSyntheticReportSettlement(string promotionDiscountCode)
        {
            var resultData = from ob in _dbSettlementObject.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                             where ob.PromotionDiscountCode.ToLower().Equals(promotionDiscountCode.ToLower())
                             join s in _dbSettlement.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                             on ob.SettlementCode equals s.Code
                             join sd in _dbSettlementDetail.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                             on s.Code equals sd.SettlementCode
                             group sd by new
                             {
                                 SettlementCode = s.Code,
                                 SettlementName = s.Name,
                                 s.SaleCalendarCode,
                                 s.TotalDistributor
                             } into gsett
                             select new PromotionSyntheticReportSettlementListModel()
                             {
                                 Code = gsett.Key.SettlementCode,
                                 Name = gsett.Key.SettlementName,
                                 SalesPeriod = gsett.Key.SaleCalendarCode,
                                 DistributorQuantity = gsett.Key.TotalDistributor,
                                 DistributorQuantityConfirm = gsett.Where(x => x.Status.Equals(CommonData.SettlementPromotion.Confirm)).Select(x => x.DistributorCode).Distinct().Count(),
                                 DistributorQuantityUnConfirm = gsett.Where(x => x.Status.Equals(CommonData.SettlementPromotion.Create)).Select(x => x.DistributorCode).Distinct().Count()
                             };

            return resultData;
        }

        public IQueryable<DistributorPopupReportSettlementListModel> GetListDistributorPopupReportSettlement(string settlementCode)
        {
            var resultData = (from sd in _dbSettlementDetail.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                              join d in _dbDistributor.GetAllQueryable(x => x.DeleteFlag == 0)
                              on sd.DistributorCode equals d.Code into emptyDistributor
                              from d in emptyDistributor.DefaultIfEmpty()
                              where sd.SettlementCode.ToLower().Equals(settlementCode.ToLower())
                              select new DistributorPopupReportSettlementListModel()
                              {
                                  SettlementCode = sd.SettlementCode,
                                  DistributorCode = sd.DistributorCode,
                                  DistributorName = d.Name,
                                  Confirm = sd.Status.Equals(CommonData.SettlementPromotion.Confirm),
                                  UnConfirm = sd.Status.Equals(CommonData.SettlementPromotion.Create)
                              }).Distinct();

            return resultData;
        }
        #endregion
    }
}
