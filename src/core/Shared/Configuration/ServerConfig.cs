namespace Shared.Configuration
{
    public class ServerConfig
    {
        public int ApiIpPort { get; set; } = 2552;
        public int ApiSecureIpPort { get; set; } = 2553;
        public string NodeName { get; set; } = Environment.MachineName;
    }
}
