using Application.Services.ExchangeService.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Configuration.interfaces;
using Shared.Configuration;
using Domain.Exchange;

namespace Application.Services.ExchangeService
{
    public class ExchangeService : IExchangeService
    {
        private readonly IExchangeRepositoryFactory _processorFactory;
        private readonly ILogger<ExchangeService> _logger;
        private readonly Exchange _exchangeConfig;

        public ExchangeService(
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
            var fileName = _exchangeConfig.FullFileName();

            if (!File.Exists(fileName))
                return;

            _logger.LogWarning("Start data loading in {datetime}", DateTime.Now);

            string[] lines;
            using (var reader = new StreamReader(_exchangeConfig.FullFileName()))
            {
                lines = await File.ReadAllLinesAsync(_exchangeConfig.FullFileName());
            }

            var currentSection = "";
            IExchangeLineProcessor? currentProcessor = null;

            foreach (var line in lines)
            {
                if (line == String.Empty)
                    continue;

                if (line.StartsWith("$$$"))
                {
                    currentSection = line;
                    currentProcessor = _processorFactory.GetProcessor(currentSection);
                    continue;
                }

                if (currentProcessor == null)
                    continue;

                await currentProcessor.ProcessLineAsync(line);

            }

            _logger.LogWarning("Finish data loading in {datetime}", DateTime.Now);

            File.Delete(fileName);
        }

    }
}
