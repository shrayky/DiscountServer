using Application.Services.ExchangeService.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Configuration.interfaces;

namespace Exchange.BackgroudService
{
    public class ExchangeBackgroundService : BackgroundService
    {
        private readonly IExchangeService _exchangeService;
        private readonly ILogger<ExchangeBackgroundService> _logger;
        private readonly int _delayMinutes;

        public ExchangeBackgroundService(
            IExchangeService exchangeService,
            ILogger<ExchangeBackgroundService> logger,
            IConfigurationService configService)
        {
            _exchangeService = exchangeService;
            _logger = logger;
            _delayMinutes = 5;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _exchangeService.ProcessExchangeFileAsync();
                    _logger.LogInformation("Exchange file processed successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in exchange background service");
                }

                await Task.Delay(TimeSpan.FromMinutes(_delayMinutes), stoppingToken);
            }
        }
    }
}
