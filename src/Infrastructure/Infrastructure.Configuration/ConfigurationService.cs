using Application.Interfaces;
using Configuration.Migrations;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Configuration;
using Shared.Configuration.interfaces;
using Shared.Json;
using System.Text.Json;

namespace Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly ICacheService _memcachedClient;
        private readonly string _configPath = string.Empty;
        private readonly string _cacheKey = "app_settings";
        private readonly int _cacheExpirationMinutes = 60;
        private readonly ILogger<ConfigurationService> _logger;

        private static readonly object _lock = new();

        public ConfigurationService(ICacheService memcachedClient, ILogger<ConfigurationService> logger)
        {
            _memcachedClient = memcachedClient;
            _logger = logger;

            if (OperatingSystem.IsWindows())
            {
                string programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                _configPath = Path.Combine(programData, "Automation", ApplicationConstants.AppName, "config.json");
            }
            if (OperatingSystem.IsLinux())
            {
                string programData = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                _configPath = Path.Combine(programData, "Automation", ApplicationConstants.AppName, "config.json");
            }

            InitializeConfiguration();
        }

        private void InitializeConfiguration()
        {
            if (!File.Exists(_configPath))
                CreateDefaultConfig();
        }

        public AppSettings GetSettings()
        {
            // Сначала пробуем получить из кэша
            var settings = _memcachedClient.Get<AppSettings>(_cacheKey);

            if (settings != null)
            {
                return settings;
            }

            // Если в кэше нет, загружаем из файла и кэшируем
            lock (_lock)
            {
                settings = LoadFromFile();
                CacheSettings(settings);
                return settings;
            }
        }

        private AppSettings LoadFromFile()
        {
            string jsonContent = File.ReadAllText(_configPath);

            var configuration = JsonSerializer.Deserialize<AppSettings>(jsonContent) ?? new();

            if (configuration.Information.AppVersion == 1)
            {
                configuration = To2.DoMigration(configuration);
                SaveConfiguration(configuration);
            }

            return configuration;
        }

        private void CacheSettings(AppSettings settings)
        {
            _memcachedClient.Set(_cacheKey,
                                              settings,
                                              TimeSpan.FromMinutes(_cacheExpirationMinutes));
        }

        public void SaveConfiguration(AppSettings settings)
        {
            _logger.LogWarning("Записываю данные в файл конфигурации");

            lock (_lock)
            {
                // Сохраняем в файл
                string? directoryPath = Path.GetDirectoryName(_configPath);

                if (directoryPath == null)
                    return;

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string jsonContent = JsonSerializer.Serialize(settings, JsonSerializerOptionsProvider.Default());

                File.WriteAllText(_configPath, jsonContent);

                // Обновляем кэш
                CacheSettings(settings);
            }
        }

        private void CreateDefaultConfig()
        {
            AppSettings settings = new();
            SaveConfiguration(settings);
        }

        public void InvalidateCache()
        {
            try
            {
                _logger.LogInformation("Invalidating configuration cache");
                _memcachedClient.Remove(_cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error invalidating configuration cache");
                throw;
            }
        }

        public bool IsCacheAvailable()
        {
            try
            {
                // Пробуем получить любое значение из кэша
                var isAvailable = _memcachedClient.Get<string>("health_check") != null;

                if (!isAvailable)
                {
                    // Пробуем записать тестовое значение
                    _memcachedClient.Set("health_check", "ok", TimeSpan.FromSeconds(1));
                    isAvailable = true;
                }

                _logger.LogDebug("Cache availability check: {IsAvailable}", isAvailable);
                return isAvailable;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache is not available");
                return false;
            }
        }
    }
}
