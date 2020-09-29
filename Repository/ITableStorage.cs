namespace goOfflineE.Repository
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ITableStorage" />.
    /// </summary>
    public interface ITableStorage
    {
        /// <summary>
        /// The GetAllAsync.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        Task<IEnumerable<T>> GetAllAsync<T>(string tableName) where T : class, ITableEntity, new();

        /// <summary>
        /// The QueryAsync.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="query">The query<see cref="TableQuery{T}"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        Task<IEnumerable<T>> QueryAsync<T>(string tableName, TableQuery<T> query) where T : class, ITableEntity, new();

        /// <summary>
        /// The GetAsync.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="partitionKey">The partitionKey<see cref="string"/>.</param>
        /// <param name="rowKey">The rowKey<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        Task<T> GetAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity;

        /// <summary>
        /// The AddOrUpdateAsync.
        /// </summary>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="entity">The entity<see cref="ITableEntity"/>.</param>
        /// <returns>The <see cref="Task{object}"/>.</returns>
        Task<object> AddOrUpdateAsync(string tableName, ITableEntity entity);

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="entity">The entity<see cref="ITableEntity"/>.</param>
        /// <returns>The <see cref="Task{object}"/>.</returns>
        Task<object> DeleteAsync(string tableName, ITableEntity entity);

        /// <summary>
        /// The AddAsync.
        /// </summary>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="entity">The entity<see cref="ITableEntity"/>.</param>
        /// <returns>The <see cref="Task{object}"/>.</returns>
        Task<object> AddAsync(string tableName, ITableEntity entity);

        /// <summary>
        /// The AddBatchAsync.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="entities">The entities<see cref="IEnumerable{ITableEntity}"/>.</param>
        /// <param name="options">The options<see cref="BatchOperationOptions"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/>.</returns>
        Task<IEnumerable<T>> AddBatchAsync<T>(string tableName, IEnumerable<ITableEntity> entities, BatchOperationOptions options) where T : class, ITableEntity, new();

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="tableName">The tableName<see cref="string"/>.</param>
        /// <param name="entity">The entity<see cref="ITableEntity"/>.</param>
        /// <returns>The <see cref="Task{object}"/>.</returns>
        Task<object> UpdateAsync(string tableName, ITableEntity entity);
    }
}
