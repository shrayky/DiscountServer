using Application.Services.ExchangeService.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Configuration.interfaces;
using Shared.Configuration;
using Domain.Exchange;

namespace Application.Services.ExchangeService
{
    public class ExchangeWithBulkService : IExchangeService
    {
        private readonly IExchangeRepositoryFactory _processorFactory;
        private readonly ILogger<ExchangeService> _logger;
        private readonly Exchange _exchangeConfig;

        public ExchangeWithBulkService(
            IExchangeRepositoryFactory processorFactory,
            ILogger<ExchangeService> logger,
            IConfigurationService configService)
        {
            _processorFactory = processorFactory;
            _logger = logger;
            _exchangeConfig = configService.GetSettings().Exchange;
        }

        public async Task ProcessExchangeFileAsync()
        {
            if (!File.Exists(_exchangeConfig.FullFileName()))
                return;

            _logger.LogWarning("[{datetime}] - Start data loading", DateTime.Now);

            string[] lines;
            using (var reader = new StreamReader(_exchangeConfig.FullFileName()))
            {
                lines = await File.ReadAllLinesAsync(_exchangeConfig.FullFileName());
            }

            var currentSection = "";
            var currentBatch = new List<string>();
            IExchangeLineProcessor? currentProcessor = null;

            foreach (var line in lines)
            {
                if (line == String.Empty)
                    continue;

                if (line.StartsWith("$$$"))
                {
                    if (currentProcessor != null && currentBatch.Count != 0)
                    {
                        await currentProcessor.ProcessLinesAsync(currentBatch);
                        currentBatch.Clear();
                    }

                    currentSection = line;
                    currentProcessor = _processorFactory.GetProcessor(currentSection);
                    continue;
                }

                if (currentProcessor == null)
                    continue;

                currentBatch.Add(line);

            }

            if (currentProcessor != null && currentBatch.Count != 0)
            {
                await currentProcessor.ProcessLinesAsync(currentBatch);
                currentBatch.Clear();
            }

            _logger.LogWarning("[{datetime}] - Finish data loading", DateTime.Now);

           File.Delete(_exchangeConfig.FullFileName());
        }
    }
}
