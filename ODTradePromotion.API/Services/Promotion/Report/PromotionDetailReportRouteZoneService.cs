using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ODTradePromotion.API.Infrastructure.TpTemTable;
using ODTradePromotion.API.Models.Report;
using ODTradePromotion.API.Services.Base;
using Sys.Common.Constants;
using System.Linq;

namespace ODTradePromotion.API.Services.Promotion.Report
{
    public class PromotionDetailReportRouteZoneService : IPromotionDetailReportRouteZoneService
    {
        #region Property
        private readonly ILogger<PromotionDetailReportRouteZoneService> _logger;
        private readonly IBaseRepository<Temp_TpOrderHeaders> _dbOrderHeader;
        private readonly IBaseRepository<Temp_TpOrderDetails> _dbOrderDetail;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public PromotionDetailReportRouteZoneService(ILogger<PromotionDetailReportRouteZoneService> logger,
            IBaseRepository<Temp_TpOrderHeaders> dbOrderHeader,
            IBaseRepository<Temp_TpOrderDetails> dbOrderDetail,
            IMapper mapper
            )
        {
            _logger = logger;
            _dbOrderHeader = dbOrderHeader;
            _dbOrderDetail = dbOrderDetail;
            _mapper = mapper;
        }
        #endregion

        #region Method
        public IQueryable<PromotionDetailReportRouteZoneListModel> GetRouteZonesOrderForPopupPromotionReport(PromotionReportEcoParameters request)
        {
            var query = (from td in _dbOrderDetail.GetAllQueryable().AsNoTracking()
                         join th in _dbOrderHeader.GetAllQueryable(x => x.Status.ToLower().Equals(CommonData.Status.Active.ToLower())
                         && string.IsNullOrEmpty(x.RecallOrderCode)
                         && x.OrdDate >= request.EffectiveDateFrom && x.OrdDate <= request.ValidUntil).AsNoTracking()
                         on td.OrdNbr equals th.OrdNbr
                         where request.PromotionCode.ToLower().Equals(td.DiscountID.ToLower())
                         select new { td, th });

            if (!string.IsNullOrEmpty(request.ScopeType) && request.ListScope != null && request.ListScope.Any())
            {
                if (request.ScopeType.Equals(CommonData.PromotionSetting.ScopeDSA))
                {
                    query = query.Where(x => request.ListScope.Contains(x.th.DSA_Code));
                }
                else
                {
                    query = query.Where(x => x.th.SalesOrg_Code.Equals(request.SaleOrg) &&
                    (request.ListScope.Contains(x.th.Branch_Code) || request.ListScope.Contains(x.th.Region_Code) ||
                    request.ListScope.Contains(x.th.SubRegion_Code) || request.ListScope.Contains(x.th.Area_Code) ||
                    request.ListScope.Contains(x.th.SubArea_Code)));
                }

            }

            if (!string.IsNullOrEmpty(request.ApplicableObjectType) && request.ListApplicableObject != null && request.ListApplicableObject.Any())
            {
                if (request.ApplicableObjectType.Equals(CommonData.PromotionSetting.ObjectCustomerAttributes))
                {
                    query = query.Where(x => request.ListApplicableObject.Contains(x.th.CustomerAttribute0) || request.ListApplicableObject.Contains(x.th.CustomerAttribute1) ||
                    request.ListApplicableObject.Contains(x.th.CustomerAttribute2) || request.ListApplicableObject.Contains(x.th.CustomerAttribute3) ||
                    request.ListApplicableObject.Contains(x.th.CustomerAttribute4) || request.ListApplicableObject.Contains(x.th.CustomerAttribute5) ||
                    request.ListApplicableObject.Contains(x.th.CustomerAttribute6) || request.ListApplicableObject.Contains(x.th.CustomerAttribute7) ||
                    request.ListApplicableObject.Contains(x.th.CustomerAttribute8) || request.ListApplicableObject.Contains(x.th.CustomerAttribute9));
                }
            }

            return query.Select(x => new PromotionDetailReportRouteZoneListModel()
            {
                RouteZoneId = x.th.RouteZoneID,
                RouteZoneDescription = x.th.RouteZoneName
            }).Distinct();
        }


        public IQueryable<PromotionDetailReportRouteZoneListModel> GetRouteZonesOrderForPromotionReport(PromotionReportEcoParameters request)
        {
            var query = (from td in _dbOrderDetail.GetAllQueryable().AsNoTracking()
                         join th in _dbOrderHeader.GetAllQueryable(x => x.Status.ToLower().Equals(CommonData.Status.Active.ToLower())
                         && string.IsNullOrEmpty(x.RecallOrderCode)
                         && x.OrdDate >= request.EffectiveDateFrom && x.OrdDate <= request.ValidUntil).AsNoTracking()
                         on td.OrdNbr equals th.OrdNbr
                         where request.PromotionCode.ToLower().Equals(td.DiscountID.ToLower())
                         select new { td, th });

            if (request.ListRouteZone != null && request.ListRouteZone.Any())
            {
                query = query.Where(x => request.ListRouteZone.Contains(x.th.RouteZoneID));
            }

            if (!string.IsNullOrEmpty(request.ScopeType) && request.ListScope != null && request.ListScope.Any())
            {
                if (request.ScopeType.Equals(CommonData.PromotionSetting.ScopeDSA))
                {
                    query = query.Where(x => request.ListScope.Contains(x.th.DSA_Code));
                }
                else
                {
                    query = query.Where(x => x.th.SalesOrg_Code.Equals(request.SaleOrg) &&
                    (request.ListScope.Contains(x.th.Branch_Code) || request.ListScope.Contains(x.th.Region_Code) ||
                    request.ListScope.Contains(x.th.SubRegion_Code) || request.ListScope.Contains(x.th.Area_Code) ||
                    request.ListScope.Contains(x.th.SubArea_Code)));
                }

            }

            if (!string.IsNullOrEmpty(request.ApplicableObjectType) && request.ListApplicableObject != null && request.ListApplicableObject.Any())
            {
                if (request.ApplicableObjectType.Equals(CommonData.PromotionSetting.ObjectCustomerAttributes))
                {
                    query = query.Where(x => request.ListApplicableObject.Contains(x.th.CustomerAttribute0) || request.ListApplicableObject.Contains(x.th.CustomerAttribute1) ||
                    request.ListApplicableObject.Contains(x.th.CustomerAttribute2) || request.ListApplicableObject.Contains(x.th.CustomerAttribute3) ||
                    request.ListApplicableObject.Contains(x.th.CustomerAttribute4) || request.ListApplicableObject.Contains(x.th.CustomerAttribute5) ||
                    request.ListApplicableObject.Contains(x.th.CustomerAttribute6) || request.ListApplicableObject.Contains(x.th.CustomerAttribute7) ||
                    request.ListApplicableObject.Contains(x.th.CustomerAttribute8) || request.ListApplicableObject.Contains(x.th.CustomerAttribute9));
                }
            }

            return query.GroupBy(n =>
            new
            {
                n.th.RouteZoneID,
                n.th.RouteZoneName,
                n.td.PromotionLevel,
                n.td.PromotionLevelName,
                n.th.ReferenceLink,
                n.th.SalesRepCode,
            }).Select(x => new PromotionDetailReportRouteZoneListModel()
            {
                RouteZoneId = x.Key.RouteZoneID,
                RouteZoneDescription = x.Key.RouteZoneName,
                PromotionLevel = x.Key.PromotionLevel,
                PromotionLevelName = x.Key.PromotionLevelName,
                SalesRepCode = x.Key.SalesRepCode,
                ReferenceLink = x.Key.ReferenceLink,
            });
        }
        #endregion

    }
}

