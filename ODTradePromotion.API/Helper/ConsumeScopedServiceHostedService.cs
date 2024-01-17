﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Helper
{
    public class ConsumeScopedServiceHostedService : BackgroundService
    {
        public IServiceProvider Services { get; }
        public ConsumeScopedServiceHostedService(IServiceProvider services)
        {
            Services = services;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWork(stoppingToken);
        }
        private async Task DoWork(CancellationToken stoppingToken)
        {
            using(var scope = Services.CreateScope())
            {
                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

                await scopedProcessingService.DoWork(stoppingToken);
            }
        }
    }
}
