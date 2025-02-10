using CouchDb.Context;
using CouchDb.Repositories;
using CouchDB.Driver.DependencyInjection;
using Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Shared.Configuration.interfaces;

namespace CouchDb
{
    public static class CouchDbExtensions
    {
        public static IServiceCollection AddCouchDbInfrastructure(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var configService = scope.ServiceProvider.GetRequiredService<IConfigurationService>();
            var settings = configService.GetSettings();
            var dbConfig = settings.Database;

            services.AddCouchContext<CouchDbContext>(options =>
            {
                options.UseEndpoint(dbConfig.NetAddress)
                        .EnsureDatabaseExists()
                        .UseBasicAuthentication(username: dbConfig.UserName, password: dbConfig.Password);
            });

            services.AddSingleton<IClientRepository, CouchDbClientRepository>();
            services.AddSingleton<ICardRepository, CouchDbCardRepository>();
            services.AddSingleton<IRepositoryFactory, CouchDbRepositoryFactory>();

            return services;
        }
    }
}
