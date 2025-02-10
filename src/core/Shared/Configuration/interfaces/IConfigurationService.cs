namespace Shared.Configuration.interfaces
{
    public interface IConfigurationService
    {
        AppSettings GetSettings();
        void SaveConfiguration(AppSettings settings);
        void InvalidateCache();
        bool IsCacheAvailable();
    }
}
