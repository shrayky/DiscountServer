namespace Shared.Logging.interfasces
{
    public interface ILoggingConfiguration
    {
        public bool Enable { get; set; }
        public string LogLevel { get; set; }
        public int RetentionDays { get; set; }
    }
}
