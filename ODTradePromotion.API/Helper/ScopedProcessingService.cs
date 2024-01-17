using Microsoft.Extensions.Configuration;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Infrastructure.Tp;
using ODTradePromotion.API.Services.Base;
using ODTradePromotion.API.Services.RegisterPromotion;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Helper
{
    internal interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
    internal class ScopedProcessingService : IScopedProcessingService
    {
        private readonly ICheckBudgetService _checkBudgetService;
        private readonly IBaseRepository<RegistrationQueue> _baseRegistrationQueueRepository;
        private readonly IConfiguration _config;

        public ScopedProcessingService(ICheckBudgetService checkBudgetService, IBaseRepository<RegistrationQueue> baseRegistrationQueueRepository, IConfiguration config)
        {
            _checkBudgetService = checkBudgetService;
            _baseRegistrationQueueRepository = baseRegistrationQueueRepository;
            _config = config;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            var timer = Convert.ToInt32(_config.GetValue<string>("TimerPromotionBudgetBackgroundService"));
            while (!stoppingToken.IsCancellationRequested)
            {
                var quequeNumber = _baseRegistrationQueueRepository.GetAll().ToList();
                if (!quequeNumber.Any())
                {
                    await Task.Delay(timer*2, stoppingToken);
                }
                await _checkBudgetService.CheckBudgetAndResponeForRegisterPromotion();
                await Task.Delay(timer,stoppingToken);
            }
        }
    }
}
