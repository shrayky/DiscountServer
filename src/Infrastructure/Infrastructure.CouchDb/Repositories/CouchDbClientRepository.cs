using CouchDb.Context;
using CouchDb.Documents;
using CouchDb.Repositories.Base;
using CouchDB.Driver.Extensions;
using CouchDB.Driver.Query.Extensions;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Configuration.interfaces;

namespace CouchDb.Repositories
{
    public class CouchDbClientRepository : CouchDbRepository<Client>, IClientRepository
    {
        public CouchDbClientRepository(CouchDbContext context, IConfigurationService configurationService, ILogger<CouchDbClientRepository> logger) : base(context, configurationService, logger, ctx => ctx.Clients!)
        {
        }

        public async Task<Client?> GetAsync(string id) => await GetByIdAsync(id);

        public async Task<Client?> ByIdAsync(string id) => await GetByIdAsync(id);

        public async Task<Client?> ByPhoneAsync(string phone)
        {
            var result = await Database
                    .UseIndex("phone_index")
                    .Where(c => c.Data.Phone == phone)
                    .FirstOrDefaultAsync();

            return result?.ToDomain()!;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var doc = await Database.FindAsync(id);

            if (doc == null)
                return false;

            return await DeleteAsync(doc.Data);
        }

        public async Task<bool> DeleteAsync(Client client)
        {
            var doc = CouchDoc<Client>.FromDomain(client, client.Id);

            await Database.RemoveAsync(doc);

            return true;
        }

        public async Task<bool> DeleteAllAsync()
        {
            var docs = await Database.ToListAsync();
            await Database.DeleteRangeAsync(docs);
            return true;
        }

        public async Task<bool> UpdateAsync(Client client)
        {
            return await base.UpdateAsync(client.Id, client);
        }
    }
}
