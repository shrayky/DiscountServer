using Domain.Exchange;
using Domain.Repositories;
using Exchange.LineProcessors;

namespace Exchange
{
    public class ExchangeProcessorFactory : IExchangeRepositoryFactory
    {
        private readonly IExchangeRepositoryFactory _repositoryFactory;
        private readonly Dictionary<string, IExchangeLineProcessor> _processors;

        public ExchangeProcessorFactory(IRepositoryFactory repositoryFactory)
        {
            _processors = new Dictionary<string, IExchangeLineProcessor>
            {
                ["$$$ADDCLIENTDISCS"] = new ClientLineProcessor(repositoryFactory),
                ["$$$ADDCCARDDISCS"] = new CardLineProcessor(repositoryFactory)
            };
        }

        public IExchangeLineProcessor? GetProcessor(string section)
        {
            return _processors.TryGetValue(section, out var processor) ? processor : null;
        }
    }
}
