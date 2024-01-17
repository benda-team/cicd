using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.External;
using ODTradePromotion.API.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ODTradePromotion.API.Services.Common
{
    public class ExternalService : IExternalService
    {
        #region Property
        private readonly ILogger<ExternalService> _logger;
        private readonly IBaseRepository<SaleCalendarGenerate> _dbSaleCalendarGenerate;
        private readonly IBaseRepository<SaleCalendar> _dbSaleCalendar;
        private readonly IBaseRepository<SaUserWithDistributorShipto> _dbUserWithDistributor;
        private readonly IBaseRepository<Distributor> _dbDistributor;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public ExternalService(ILogger<ExternalService> logger,
            IBaseRepository<SaleCalendarGenerate> dbSaleCalendarGenerate,
            IBaseRepository<SaleCalendar> dbSaleCalendar,
            IBaseRepository<SaUserWithDistributorShipto> dbUserWithDistributor,
            IBaseRepository<Distributor> dbDistributor,
            IMapper mapper
            )
        {
            _logger = logger;
            _dbSaleCalendarGenerate = dbSaleCalendarGenerate;
            _dbSaleCalendar = dbSaleCalendar;
            _dbUserWithDistributor = dbUserWithDistributor;
            _dbDistributor = dbDistributor;
            _mapper = mapper;
        }

        #endregion

        #region Method
        public SaleCalendarByTyeModel GetCalendarGenerateByCode(string code)
        {
            var result = _mapper.Map<SaleCalendarByTyeModel>(_dbSaleCalendarGenerate.FirstOrDefault(x => x.Code.Equals(code)));
            return result;
        }



        public IQueryable<SalePeriodModel> GetListCalendarBySalePeriod()
        {
            var result = (from sc in _dbSaleCalendar.GetAllQueryable().AsNoTracking()
                          join scg in _dbSaleCalendarGenerate.GetAllQueryable().AsNoTracking()
                          on sc.Id equals scg.SaleCalendarId
                          where scg.Type == "MONTH" && scg.EndDate.Value <= DateTime.Now
                          select new SalePeriodModel
                          {
                              Id = scg.Id,
                              SaleCalendarId = scg.SaleCalendarId,
                              Type = scg.Type,
                              Code = scg.Code,
                              StartDate = scg.StartDate,
                              EndDate = scg.EndDate,
                              Ordinal = scg.Ordinal,
                              SaleYear = sc.SaleYear
                          }).AsQueryable();
            return result;
        }

        public IQueryable<SalePeriodModel> GetListCalendar(GetListCalendarEcoParameters ecoParameters)
        {
            var result = (from sc in _dbSaleCalendar.GetAllQueryable().AsNoTracking()
                          join scg in _dbSaleCalendarGenerate.GetAllQueryable().AsNoTracking()
                          on sc.Id equals scg.SaleCalendarId
                          //where scg.StartDate.Value <= DateTime.Now
                          select new { sc, scg });

            if (!string.IsNullOrEmpty(ecoParameters.Type))
            {
                result = result.Where(x => x.scg.Type.ToLower().Equals(ecoParameters.Type.ToLower()));
            }

            if (ecoParameters.StartDate.HasValue)
            {
                result = result.Where(x => x.scg.StartDate.HasValue && x.scg.StartDate.Value <= DateTime.Now && x.scg.StartDate.Value <= ecoParameters.StartDate.Value);
            }
            else
            {
                result = result.Where(x => x.scg.StartDate.HasValue && x.scg.StartDate.Value <= DateTime.Now);
            }
            
            if (ecoParameters.EndDate.HasValue)
            {
                result = result.Where(x => x.scg.EndDate.HasValue && x.scg.EndDate.Value >= ecoParameters.EndDate.Value && x.scg.EndDate.Value <= DateTime.Now);
            }

            return result.Select(x => new SalePeriodModel
            {
                Id = x.scg.Id,
                SaleCalendarId = x.scg.SaleCalendarId,
                Type = x.scg.Type,
                Code = x.scg.Code,
                StartDate = x.scg.StartDate,
                EndDate = x.scg.EndDate,
                Ordinal = x.scg.Ordinal,
                SaleYear = x.sc.SaleYear
            }).AsQueryable();
        }

        public UserWithDistributorModel GetDistributorByUserCode(string usercode)
        {
            return (from x in _dbUserWithDistributor.GetAllQueryable(x => x.UserCode.ToLower().Equals(usercode.ToLower()))
                    join d in _dbDistributor.GetAllQueryable(x => x.DeleteFlag == 0).AsNoTracking()
                    on x.DistributorCode.ToLower() equals d.Code.ToLower() into dData
                    from d in dData.DefaultIfEmpty()
                    select new UserWithDistributorModel()
                    {
                        Id = x.Id,
                        UserCode = x.UserCode,
                        DistributorCode = x.DistributorCode,
                        DistributorName = d != null ? d.Name : string.Empty
                    }).FirstOrDefault();
        }
        #endregion
    }
}
