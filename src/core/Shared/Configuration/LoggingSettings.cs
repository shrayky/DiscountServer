using Shared.Logging.interfasces;
using Shared.Logging.Shared.Logging;

namespace Shared.Configuration
{
    public class LoggingSettings : ILoggingConfiguration
    {
        public bool Enable { get; set; } = true;
        public string LogLevel { get; set; } = LogLevels.Warning;
        public int RetentionDays { get; set; } = 30;
    }
}
