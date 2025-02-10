using CouchDb.Documents;
using CouchDB.Driver;
using CouchDB.Driver.Options;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CouchDb.Context
{
    public class CouchDbContext : CouchContext
    {
        private readonly ILogger<CouchDbContext> _logger;

        public CouchDbContext(CouchOptions<CouchDbContext> settings, ILogger<CouchDbContext> logger) : base(settings)
        {
            _logger = logger;
        }

        public CouchDatabase<CouchDoc<Client>>? Clients { get; set; }
        public CouchDatabase<CouchDoc<Card>>? Cards { get; set; }

        protected override void OnDatabaseCreating(CouchDatabaseBuilder databaseBuilder)
        {
            databaseBuilder.Document<CouchDoc<Client>>().ToDatabase("ds_clients");
            databaseBuilder.Document<CouchDoc<Card>>().ToDatabase("ds_cards");

            databaseBuilder.Document<CouchDoc<Client>>().HasIndex("phone_index", builder => builder.IndexBy(c => c.Data.Phone));
        }

    }
}
