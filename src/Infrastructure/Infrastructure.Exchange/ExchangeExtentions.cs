using Application.Services.ExchangeService;
using Application.Services.ExchangeService.Interfaces;
using Domain.Exchange;
using Exchange.BackgroudService;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange
{
    public static class ExchangeExtentions
    {
        public static IServiceCollection AddExchangeInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IExchangeRepositoryFactory, ExchangeProcessorFactory>();
            services.AddSingleton<IExchangeService, ExchangeWithBulkService>();

            services.AddHostedService<ExchangeBackgroundService>();

            return services;
        }
    }
}
