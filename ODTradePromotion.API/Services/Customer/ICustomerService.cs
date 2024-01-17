using ODTradePromotion.API.Models.Customer;
using System;
using System.Linq;

namespace ODTradePromotion.API.Services.Customer
{
    public interface ICustomerService
    {
        public IQueryable<CustomerAttributeModel> GetListCustomerAttributeByCustomerSetting(string customerSettingCode);
        public IQueryable<CustomerShiptoModel> GetListCustomerShipto();
    }
}
