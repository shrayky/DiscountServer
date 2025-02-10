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
    public class CouchDbCardRepository : CouchDbRepository<Card>, ICardRepository
    {
        public CouchDbCardRepository(CouchDbContext context, IConfigurationService configurationService, ILogger<CouchDbCardRepository> logger) : base(context, configurationService, logger, ctx => ctx.Cards!)
        {
        }

        public async Task<Card?> GetAsync(string id) => await GetByIdAsync(id);

        public async Task<Card?> ByIdAsync(string id) => await GetByIdAsync(id);

        public async Task<IEnumerable<Card>> GetByClientIdAsync(string clientId)
        {
            var result = await Database
                .UseIndex("client_index")
                .Where(c => c.Data.ClientId == clientId)
                .ToListAsync();

            if (result == null)
                return Enumerable.Empty<Card>();

            return result.Select(doc => doc.ToDomain());
        }

        public async Task<bool> UpdateAsync(Card card)
        {
            return await base.UpdateAsync(card.Id, card);
        }

        public async Task<bool> DeleteAsync(Card card)
        {
            var doc = CouchDoc<Card>.FromDomain(card, card.Id);

            await Database.RemoveAsync(doc);

            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var doc = await Database.FindAsync(id);

            if (doc == null)
                return false;

            return await DeleteAsync(doc.Data);
        }

        public async Task<bool> DeleteAllAsync()
        {
            var docs = await Database.ToListAsync();
            await Database.DeleteRangeAsync(docs);
            return true;
        }
    }
}
