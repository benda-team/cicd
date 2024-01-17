using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Infrastructure.Tp;
using ODTradePromotion.API.Services.Base;
using Sys.Common.Models;
using System;
using System.Linq;

namespace ODTradePromotion.API.Services.RegisterPromotion
{
    public class OrdinalNumberRegistrationService : IOrdinalNumberRegistrationService
    {
        private readonly IBaseRepository<RegistrationQueue> _baseRegistrationQueueRepository;
        public OrdinalNumberRegistrationService(IBaseRepository<RegistrationQueue> baseRegistrationQueueRepository)
        {
            _baseRegistrationQueueRepository = baseRegistrationQueueRepository;
        }

        public Result<bool> RegisterQueueForPromotions(RegistrationQueue request)
        {
            var result = new Result<bool>
            {
                Success = true,
                Data = true
            };
            try
            {
                var data = _baseRegistrationQueueRepository.Insert(request);
                return result;
            }
            catch (Exception ex)
            {
                result.Data = false;
                result.Success = false;
                result.Messages.Add(ex.Message);
                return result;
            }
        }
    }
}
