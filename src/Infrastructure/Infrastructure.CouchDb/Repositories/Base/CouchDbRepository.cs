using CouchDb.Context;
using CouchDb.Documents;
using CouchDB.Driver;
using Domain.Common;
using Microsoft.Extensions.Logging;
using Shared.Configuration.interfaces;

namespace CouchDb.Repositories.Base
{
    public abstract class CouchDbRepository<T> where T : class, IHasId
    {
        protected readonly CouchDbContext Context;
        protected readonly ILogger Logger;
        protected readonly ICouchDatabase<CouchDoc<T>> Database;
        private readonly IConfigurationService _configurationService;

        protected CouchDbRepository(CouchDbContext context, IConfigurationService configurationService, ILogger logger, Func<CouchDbContext, ICouchDatabase<CouchDoc<T>>> selectedDatabase)
        {
            Context = context;
            Logger = logger;
            Database = selectedDatabase(context);
            _configurationService = configurationService;
        }

        public virtual async Task<T?> GetByIdAsync(string id)
        {
            var doc = await Database.FindAsync(id);
            return doc?.ToDomain();
        }

        public virtual async Task<bool> CreateAsync(T entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
            }

            return await SaveDocumentAsync(entity);
        }

        public virtual async Task<bool> CreateBulkAsync(IEnumerable<T> entities)
        {
            var configuration = _configurationService.GetSettings();
            int BATCH_SIZE = configuration.Database.BulkBatchSize;
            int MAX_PARALLEL_TASKS = configuration.Database.BulkMaxParallelTasks;

            //var duplicateIds = entities.GroupBy(e => e.Id).Where(g => g.Count() > 1).Select(g => new { Id = g.Key, Count = g.Count() });

            //// Логируем если есть дубликаты
            //if (duplicateIds.Any())
            //{
            //    Logger.LogWarning("Found duplicate IDs: {Duplicates}",
            //        string.Join(", ", duplicateIds.Select(d => $"{d.Id}({d.Count})\r\n")));
            //}

            var ids = entities.Select(e => e.Id).ToList();
            var existingDocs = await Database.FindManyAsync(ids);

            var documentBatches = entities
               .Join(
                   existingDocs,
                   entity => entity.Id,
                   doc => doc.Id,
                   (entity, existingDoc) =>
                   {
                       var doc = CouchDoc<T>.FromDomain(entity, entity.Id);
                       doc.Rev = existingDoc.Rev;
                       return doc;
                   })
               .Union(entities
                   .Where(entity => !existingDocs.Any(doc => doc.Id == entity.Id))
                   .Select(entity => CouchDoc<T>.FromDomain(entity, entity.Id)))
               .GroupBy(e => e.Id)
               .Select(g => g.Last())
               .Chunk(BATCH_SIZE);

            var dbName = typeof(T).Name.ToLower();
            Logger.LogInformation("Starting bulk insert into {Database}: {Count} documents", dbName, entities.Count());
            using var semaphore = new SemaphoreSlim(MAX_PARALLEL_TASKS);

            var tasks = documentBatches.Select(async batch =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await Database.AddOrUpdateRangeAsync(batch);
                    await Task.Delay(100);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);

            // однопоточная запись
            //foreach (var batch in documentBatches)
            //{
            //    int batchCount = 0;
            //    try
            //    {
            //        await Database.AddOrUpdateRangeAsync(batch);
            //        await Task.Delay(100);
            //    }
            //    catch (Exception ex)
            //    {
            //        Logger.LogError(ex, "Error during bulk insert of {Count} documents in {batchCount} batch: error: {err}", entities.Count(), batchCount, ex.Message);
            //    }
            //    batchCount++;
            //}

            return true;
        }

        public virtual async Task<bool> UpdateAsync(string id, T entity)
        {
            entity.Id = id;
            return await SaveDocumentAsync(entity);
        }

        private async Task<bool> SaveDocumentAsync(T entity)
        {
            var existingDoc = await Database.FindAsync(entity.Id);
            var doc = CouchDoc<T>.FromDomain(entity, entity.Id);

            if (existingDoc != null)
            {
                doc.Rev = existingDoc.Rev;
            }

            await Database.AddOrUpdateAsync(doc);
            return true;
        }
    }
}
