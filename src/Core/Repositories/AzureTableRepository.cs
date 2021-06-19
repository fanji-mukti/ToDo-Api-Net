namespace Core.Repositories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using EnsureThat;
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// Provides data access layer to Azure Table Storage.
    /// </summary>
    /// <typeparam name="T">The item stored in the table.</typeparam>
    /// <typeparam name="TEntity">The data representation of the item in database.</typeparam>
    internal sealed class AzureTableRepository<T, TEntity> : IRepository<T>
        where T : class
        where TEntity : TableEntity, new()
    {
        private const string PartitionKeyName = "PartitionKey";

        private readonly CloudTable table;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTableRepository{T, TEntity}"/> class.
        /// </summary>
        /// <param name="cloudTable">The <see cref="CloudTable"/>.</param>
        /// <param name="mapper">The <see cref="IMapper"/> to map <see cref="T"/> to <see cref="TEntity"/>.</param>
        public AzureTableRepository(CloudTable cloudTable, IMapper mapper)
        {
            this.table = EnsureArg.IsNotNull(cloudTable, nameof(cloudTable));
            this.mapper = EnsureArg.IsNotNull(mapper, nameof(mapper));
        }

        /// <inheritdoc/>
        public Task AddAsync(T itemToAdd)
        {
            EnsureArg.IsNotNull(itemToAdd, nameof(itemToAdd));

            return InsertAsync();

            async Task InsertAsync()
            {
                var entity = this.mapper.Map<TEntity>(itemToAdd);
                var insertOperation = TableOperation.Insert(entity);

                await this.table.ExecuteAsync(insertOperation).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public Task<IEnumerable<T>> GetAsync(string partitionKey)
        {
            EnsureArg.IsNotNullOrWhiteSpace(partitionKey, nameof(partitionKey));

            return RetrieveAsync();

            async Task<IEnumerable<T>> RetrieveAsync()
            {
                var entities = new List<TEntity>();
                var query = new TableQuery<TEntity>()
                    .Where(
                        TableQuery.GenerateFilterCondition(
                            PartitionKeyName,
                            QueryComparisons.Equal,
                            partitionKey));

                TableContinuationToken continuationToken = null;

                do
                {
                    var segment = await this.table.ExecuteQuerySegmentedAsync(query, continuationToken).ConfigureAwait(false);

                    entities.AddRange(segment.Results);
                    continuationToken = segment.ContinuationToken;
                }
                while (continuationToken != null);

                return this.mapper.Map<List<T>>(entities);
            }
        }

        /// <inheritdoc/>
        public Task<T> GetAsync(string partitionKey, string id)
        {
            EnsureArg.IsNotNullOrWhiteSpace(partitionKey, nameof(partitionKey));
            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            return RetrieveAsync();

            async Task<T> RetrieveAsync()
            {
                var retrieveOperation = TableOperation.Retrieve<TEntity>(partitionKey, id);
                var entity = await this.table.ExecuteAsync(retrieveOperation).ConfigureAwait(false);

                return this.mapper.Map<T>(entity);
            }
        }

        /// <inheritdoc/>
        public Task UpdateAsync(T itemToUpdate)
        {
            EnsureArg.IsNotNull(itemToUpdate, nameof(itemToUpdate));

            return UpdateAsync();

            async Task UpdateAsync()
            {
                var entity = this.mapper.Map<TEntity>(itemToUpdate);
                var updateOperation = TableOperation.Replace(entity);

                await this.table.ExecuteAsync(updateOperation).ConfigureAwait(false);
            }
        }
    }
}