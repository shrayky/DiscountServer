using CouchDb.Context;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Configuration.interfaces;

namespace CouchDb.Repositories
{
    public class CouchDbRepositoryFactory : IRepositoryFactory
    {
        private readonly CouchDbContext _context;
        IConfigurationService _configurationService;
        private readonly ILoggerFactory _loggerFactory;


        public CouchDbRepositoryFactory(CouchDbContext context, IConfigurationService configurationService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _loggerFactory = loggerFactory;
            _configurationService = configurationService;
        }

        public IClientRepository CreateClientRepository()
        {
            return new CouchDbClientRepository(_context, _configurationService, _loggerFactory.CreateLogger<CouchDbClientRepository>());
        }

        public ICardRepository CreateCardRepository()
        {
            return new CouchDbCardRepository(_context, _configurationService, _loggerFactory.CreateLogger<CouchDbCardRepository>());
        }
    }
}
