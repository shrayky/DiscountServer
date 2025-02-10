namespace Shared.Configuration
{
    public class AppSettings
    {
        public Information Information { get; set; } = new();
        public ServerConfig ServerConfig { get; set; } = new();
        public LoggingSettings LoggingConfig { get; set; } = new();
        public Database Database { get; set; } = new();
        public Exchange Exchange { get; set; } = new();
    }
}
