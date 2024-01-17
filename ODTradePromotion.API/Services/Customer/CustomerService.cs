using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Models.Customer;
using ODTradePromotion.API.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly IBaseRepository<CustomerAttribute> _dbCustomerAttribute;
        private readonly IBaseRepository<CustomerSetting> _dbCustomerSetting;
        private readonly IBaseRepository<CustomerInformation> _dbCusInfo;
        private readonly IBaseRepository<CustomerShipto> _dbCusShipto;
        private readonly IMapper _mapper;

        public CustomerService(ILogger<CustomerService> logger,
            IBaseRepository<CustomerAttribute> dbCustomerAttribute,
            IBaseRepository<CustomerSetting> dbCustomerSetting,
            IBaseRepository<CustomerInformation> dbCusInfo,
            IBaseRepository<CustomerShipto> dbCusShipto,
            IMapper mapper)
        {
            _logger = logger;
            _dbCustomerAttribute = dbCustomerAttribute;
            _dbCustomerSetting = dbCustomerSetting;
            _dbCusInfo = dbCusInfo;
            _dbCusShipto = dbCusShipto;
            _mapper = mapper;
        }

        public IQueryable<CustomerAttributeModel> GetListCustomerAttributeByCustomerSetting(string customerSettingCode)
        {
            var now = DateTime.Now;
            var CustomerSettingId = _dbCustomerSetting.FirstOrDefault(x => x.AttributeId.ToLower().Equals(customerSettingCode.ToLower()));

            var result = (from x in _dbCustomerAttribute.GetAllQueryable(x => x.CustomerSettingId.Equals(CustomerSettingId.Id)
                   && x.EffectiveDate <= now && (!x.ValidUntil.HasValue || x.ValidUntil.Value >= now))
                          select new CustomerAttributeModel()
                          {
                              Id = x.Id,
                              AttributeMaster = customerSettingCode,
                              Code = x.Code,
                              Description = x.Description,
                              ShortName = x.ShortName,
                              EffectiveDate = x.EffectiveDate,
                              ValidUntil = x.ValidUntil,
                              IsCustomerAttribute = x.IsCustomerAttribute
                          }).AsQueryable();
            return result;
        }

        public IQueryable<CustomerShiptoModel> GetListCustomerShipto()
        {
            return (from st in _dbCusShipto.GetAllQueryable().AsNoTracking()
                    join ci in _dbCusInfo.GetAllQueryable().AsNoTracking()
                    on st.CustomerInfomationId equals ci.Id
                    select new CustomerShiptoModel()
                    {
                        Id = st.Id,
                        ShiptoCode = st.ShiptoCode,
                        ShiptoName = st.ShiptoName,
                        Address = st.Address,
                        CustomerCode = ci.CustomerCode,
                        CustomerName = ci.FullName,
                        FullName = ci.FullName,
                        PhoneNumber = ci.PhoneNumber,
                        Email = ci.Email,
                        CustomerInfomationId = ci.Id
                    }).AsQueryable();
        }
    }
}
