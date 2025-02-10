using Serilog;

namespace Shared.Logging
{
    public static class SerilogConfiguration
    {
        private const RollingInterval DefaultRollingInterval = RollingInterval.Day;
        private const int DefaultLogDepth = 30;
        private const string DefaultLogLevel = "information";

        public static ILogger ToFile(
            string logFileName,
            string? logLevel = null,
            int? logDepth = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(logFileName, "logFileName");
            ArgumentException.ThrowIfNullOrWhiteSpace(logFileName, "logFileName");

            logDepth = logDepth ?? DefaultLogDepth;
            logDepth = logDepth == 0 ? DefaultLogDepth : logDepth;

            var configuration = new LoggerConfiguration()
                .WriteTo.File(
                    path: logFileName,
                    rollOnFileSizeLimit: true,
                    rollingInterval: DefaultRollingInterval,
                    retainedFileCountLimit: logDepth ?? DefaultLogDepth,
                    shared: true);

            configuration = SetLogLevel(configuration, logLevel ?? DefaultLogLevel);

            return configuration.CreateLogger();
        }

        private static LoggerConfiguration SetLogLevel(
            LoggerConfiguration configuration,
            string logLevel)
        {
            return logLevel.ToLower() switch
            {
                "verbose" => configuration.MinimumLevel.Verbose(),
                "debug" => configuration.MinimumLevel.Debug(),
                "information" => configuration.MinimumLevel.Information(),
                "warning" => configuration.MinimumLevel.Warning(),
                "error" => configuration.MinimumLevel.Error(),
                "fatal" => configuration.MinimumLevel.Fatal(),
                _ => configuration.MinimumLevel.Information()
            };
        }
    }
}
