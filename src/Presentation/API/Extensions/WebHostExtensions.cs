using Application.Services.ClientService.Interfaces;
using Application.Services.ClientService;
using Application.Services.FrontolDiscountUnit.Application.Services.FduService;
using Domain.FrontolDiscountUnit.Client.Application.Services.FduService.Interfaces;
using Shared.Configuration.interfaces;
using System.Security.Cryptography.X509Certificates;
using Configuration;
using CouchDb;
using Exchange;

namespace API.Extensions
{
    public static class WebHostExtensions
    {
        public static WebApplicationBuilder ApplyAppConfigurationExtension(this WebApplicationBuilder builder)
        {
            using var scope = builder.Services.BuildServiceProvider().CreateScope();
            var configService = scope.ServiceProvider.GetRequiredService<IConfigurationService>();
            var settings = configService.GetSettings();
            var serverConfig = settings.ServerConfig;
            var certPath = Path.Combine(AppContext.BaseDirectory, "selfsigned.pfx");
            var certPassword = "12345678";

            builder.WebHost.UseUrls(
                $"https://+:{serverConfig.ApiSecureIpPort}",
                $"http://+:{serverConfig.ApiIpPort}");

            X509Certificate2 cert = CertificateManager.GetCertificate(certPath, certPassword);

            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ConfigureHttpsDefaults(httpsOptions =>
                {
                    httpsOptions.ServerCertificate = cert;
                });
                serverOptions.ConfigureEndpointDefaults(options =>
                {
                    if (Environment.OSVersion.Version.Major < 10)
                    {
                        options.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
                    }
                });
            });

            if (settings.Database.UserName != string.Empty)
            {
                builder.Services.AddCouchDbInfrastructure();
                builder.Services.AddExchangeInfrastructure();

                builder.Services.AddScoped<IClientService, ClientService>();
                builder.Services.AddScoped<ICardService, CardService>();
                builder.Services.AddScoped<IFduService, FduService>();
            }

            return builder;
        }
    }

}
