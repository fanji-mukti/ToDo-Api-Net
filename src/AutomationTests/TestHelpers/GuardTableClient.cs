namespace AutomationTests.TestHelpers
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;

    public sealed class GuardTableClient : IDisposable
    {
        private readonly ConcurrentBag<CloudTable> createdTables = new ConcurrentBag<CloudTable>();

        private bool disposed;

        public GuardTableClient(CloudTableClient tableClient)
        {
            this.TableClient = tableClient;
        }

        public CloudTableClient TableClient { get; set; }

        public async Task<CloudTable> CreateTableIfNotExistsAsync(string tableName)
        {
            var table = this.TableClient.GetTableReference(tableName);
            if (await table.CreateIfNotExistsAsync().ConfigureAwait(false))
            {
                this.createdTables.Add(table);
            }

            return table;
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            var deletionTasks = this.createdTables
                .Select(x => x.DeleteAsync())
                .ToArray();

            Task.WaitAll(deletionTasks);

            this.disposed = true;
        }
    }
}