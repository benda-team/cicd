using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.External;
using System.Collections.Generic;
using System.Linq;

namespace ODTradePromotion.API.Services.Common
{
    public interface IExternalService
    {
        public IQueryable<SalePeriodModel> GetListCalendarBySalePeriod();
        public IQueryable<SalePeriodModel> GetListCalendar(GetListCalendarEcoParameters ecoParameters);
        public SaleCalendarByTyeModel GetCalendarGenerateByCode(string code);
        public UserWithDistributorModel GetDistributorByUserCode(string usercode);
    }
}
