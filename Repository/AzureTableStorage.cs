﻿namespace goOfflineE.Repository
{
    using goOfflineE.Common.Constants;
    using Microsoft.AspNetCore.Http;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AzureTableStorage" />.
    /// </summary>
    public class AzureTableStorage : ITableStorage
    {
        /// <summary>
        /// Defines the client.
        /// </summary>
        private CloudTableClient client;

        /// <summary>
        /// Defines the _tables.
        /// </summary>
        private ConcurrentDictionary<string, CloudTable> _tables;

        /// <summary>
        /// Gets or sets the Client.
        /// </summary>
        public CloudTableClient Client { get => client; set => client = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTableStorage"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The httpContextAccessor<see cref="IHttpContextAccessor"/>.</param>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/>.</param>
        public AzureTableStorage(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
        {
            string tenantId = "";
            var request = httpContextAccessor.HttpContext.Request;
            if (request != null && request.Headers.ContainsKey("Tenant"))
            {
                tenantId = request.Headers["Tenant"].ToString();
            }

            CloudStorageAccount account = CloudStorageAccount.Parse(SettingConfigurations.AzureWebJobsStorage);
            Client = account.CreateCloudTableClient();
            _tables = new ConcurrentDictionary<string, CloudTable>();
            if (!String.IsNullOrEmpty(tenantId))
            {
                var tenants = this.GetAllAsync<Entites.Tenant>("Tenants").Result;
                var tenant = tenants.FirstOrDefault(t => t.RowKey == tenantId);
                account = CloudStorageAccount.Parse(tenant.AzureWebJobsStorage);
                Client = account.CreateCloudTableClient();
                _tables = new ConcurrentDictionary<string, CloudTable>();
            }
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="tableName">.</param>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <returns>.</returns>
        public async Task<T> GetAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity
        {
            var table = await EnsureTable(tableName).ConfigureAwait(false);

            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            TableResult result = await table.ExecuteAsync(retrieveOperation).ConfigureAwait(false);

            return result.Result as T;
        }

        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        public async Task<IEnumerable<T>> GetAllAsync<T>(string tableName) where T : class, ITableEntity, new()
        {
            var table = await EnsureTable(tableName).ConfigureAwait(false);
            TableContinuationToken token = null;
            var entities = new List<T>();
            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(new TableQuery<T>(), token).ConfigureAwait(false);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return entities;
        }

        /// <summary>
        /// Gets entities by query. 
        /// Supports TakeCount parameter.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="query">The query<see cref="TableQuery{T}"/>.</param>
        /// <returns>.</returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string tableName, TableQuery<T> query) where T : class, ITableEntity, new()
        {
            var table = await EnsureTable(tableName).ConfigureAwait(false);

            bool shouldConsiderTakeCount = query.TakeCount.HasValue;

            return shouldConsiderTakeCount ?
                await QueryAsyncWithTakeCount(table, query).ConfigureAwait(false) :
                await QueryAsync(table, query).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds the or update entity.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>.</returns>
        public async Task<object> AddOrUpdateAsync(string tableName, ITableEntity entity)
        {
            var table = await EnsureTable(tableName).ConfigureAwait(false);

            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(entity);
            TableResult result = await table.ExecuteAsync(insertOrReplaceOperation).ConfigureAwait(false);

            return result.Result;
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>.</returns>
        public async Task<object> DeleteAsync(string tableName, ITableEntity entity)
        {
            var table = await EnsureTable(tableName).ConfigureAwait(false);

            TableOperation deleteOperation = TableOperation.Delete(entity);

            TableResult result = await table.ExecuteAsync(deleteOperation).ConfigureAwait(false);

            return result.Result;
        }

        /// <summary>
        /// Add the entity.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>.</returns>
        public async Task<object> AddAsync(string tableName, ITableEntity entity)
        {
            var table = await EnsureTable(tableName).ConfigureAwait(false);

            TableOperation insertOperation = TableOperation.Insert(entity);

            TableResult result = await table.ExecuteAsync(insertOperation).ConfigureAwait(false);

            return result.Result;
        }

        /// <summary>
        /// Insert a batch of entities. Support adding more than 100 entities.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="entities">Collection of entities.</param>
        /// <param name="options">Batch operation options.</param>
        /// <returns>.</returns>
        public async Task<IEnumerable<T>> AddBatchAsync<T>(string tableName, IEnumerable<ITableEntity> entities, BatchOperationOptions options = null) where T : class, ITableEntity, new()
        {
            var table = await EnsureTable(tableName).ConfigureAwait(false);

            options = options ?? new BatchOperationOptions();

            var tasks = new List<Task<IList<TableResult>>>();

            const int addBatchOperationLimit = 100;
            var entitiesOffset = 0;
            while (entitiesOffset < entities?.Count())
            {
                var entitiesToAdd = entities.Skip(entitiesOffset).Take(addBatchOperationLimit).ToList();
                entitiesOffset += entitiesToAdd.Count;

                Action<TableBatchOperation, ITableEntity> batchInsertOperation = null;
                switch (options.BatchInsertMethod)
                {
                    case BatchInsertMethod.Insert:
                        batchInsertOperation = (bo, entity) => bo.Insert(entity);
                        break;
                    case BatchInsertMethod.InsertOrReplace:
                        batchInsertOperation = (bo, entity) => bo.InsertOrReplace(entity);
                        break;
                    case BatchInsertMethod.InsertOrMerge:
                        batchInsertOperation = (bo, entity) => bo.InsertOrMerge(entity);
                        break;
                }
                TableBatchOperation batchOperation = new TableBatchOperation();
                foreach (var entity in entitiesToAdd)
                {
                    batchInsertOperation(batchOperation, entity);
                }
                tasks.Add(table.ExecuteBatchAsync(batchOperation));
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            return results.SelectMany(tableResults => tableResults, (tr, r) => r.Result as T);
        }

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>.</returns>
        public async Task<object> UpdateAsync(string tableName, ITableEntity entity)
        {
            var table = await EnsureTable(tableName).ConfigureAwait(false);
            TableOperation replaceOperation = TableOperation.Replace(entity);

            TableResult result = await table.ExecuteAsync(replaceOperation).ConfigureAwait(false);

            return result.Result;
        }

        /// <summary>
        /// Ensures existence of the table.
        /// </summary>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{CloudTable}"/>.</returns>
        private async Task<CloudTable> EnsureTable(string tableName)
        {
            if (!_tables.ContainsKey(tableName))
            {
                var table = Client.GetTableReference(tableName);
                await table.CreateIfNotExistsAsync().ConfigureAwait(false);
                _tables[tableName] = table;
            }

            return _tables[tableName];
        }

        /// <summary>
        /// Gets entities by query.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="table">.</param>
        /// <param name="query">.</param>
        /// <returns>.</returns>
        private async Task<IEnumerable<T>> QueryAsync<T>(CloudTable table, TableQuery<T> query)
            where T : class, ITableEntity, new()
        {
            var entities = new List<T>();

            TableContinuationToken token = null;
            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(query, token).ConfigureAwait(false);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return entities;
        }

        /// <summary>
        /// Get entities by query with TakeCount parameter.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="table">.</param>
        /// <param name="query">.</param>
        /// <returns>.</returns>
        private async Task<IEnumerable<T>> QueryAsyncWithTakeCount<T>(CloudTable table, TableQuery<T> query)
            where T : class, ITableEntity, new()
        {
            var entities = new List<T>();

            const int maxEntitiesPerQueryLimit = 1000;
            var totalTakeCount = query.TakeCount;
            var remainingRecordsToTake = query.TakeCount;

            TableContinuationToken token = null;
            do
            {
                query.TakeCount = remainingRecordsToTake >= maxEntitiesPerQueryLimit ? maxEntitiesPerQueryLimit : remainingRecordsToTake;
                remainingRecordsToTake -= query.TakeCount;

                var queryResult = await table.ExecuteQuerySegmentedAsync(query, token).ConfigureAwait(false);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (entities.Count < totalTakeCount && token != null);

            return entities;
        }
    }
}
