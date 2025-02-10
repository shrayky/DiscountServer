using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shared.Configuration.interfaces;

namespace Shared.Logging
{
    public static class LoggingExtensions
    {
    public static IServiceCollection AddCustomLogging(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var configService = scope.ServiceProvider.GetRequiredService<IConfigurationService>();
            var settings = configService.GetSettings();
            var logsConfiguration = settings.LoggingConfig;

            if (!logsConfiguration.Enable)
                return services;

            string directory = string.Empty;

            if (OperatingSystem.IsWindows())
                directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            if (OperatingSystem.IsLinux())
                directory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            string logFileName = Path.Combine(directory,
                                              "Automation",
                                              settings.Information.AppName,
                                              "logs",
                                              $"{settings.Information.AppName.ToLower()}.log");

            if (string.IsNullOrEmpty(logFileName))
                return services;

            string? folder = Path.GetDirectoryName(logFileName);

            if (folder != null && !Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var logger = SerilogConfiguration.ToFile(logFileName, logsConfiguration.LogLevel, logsConfiguration.RetentionDays);

            return services.AddLogging(logBuilder =>
            {
                logBuilder.AddSerilog(logger);
            });
        }
    }
}
