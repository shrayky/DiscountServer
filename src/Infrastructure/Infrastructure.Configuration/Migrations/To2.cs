using Shared.Configuration;

namespace Configuration.Migrations
{
    public static class To2
    {
        public static AppSettings DoMigration(AppSettings current)
        {
            AppSettings appSettings = new();

            appSettings = current;

            appSettings.Information = new();
            appSettings.ServerConfig.ApiSecureIpPort = current.ServerConfig.ApiIpPort;
            appSettings.ServerConfig.ApiIpPort = 2552;

            return appSettings;
        }
    }
}
