namespace AutomationTests.DataAccessLayerTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutomationTests.TestHelpers;
    using Core.Models;
    using Core.Repositories;
    using Core.Repositories.Entities;
    using Microsoft.Azure.Cosmos.Table;

    internal sealed class AzureTableRepositorySteps : BaseTestSteps<AzureTableRepositorySteps>
    {
        private const string TableName = "ToDoItem";

        private readonly AzureStorageEmulator storageEmulator;
        private readonly IRepository<ToDoItem> repository;
        private readonly CloudTable table;

        public AzureTableRepositorySteps(AzureStorageEmulator storageEmulator)
        {
            this.storageEmulator = storageEmulator;
            this.table = storageEmulator.TableClient.CreateTableIfNotExistsAsync(TableName).GetAwaiter().GetResult();
            this.repository = RepositoryFactory.CreateToDoItemRepository(AzureStorageEmulator.EmulatorConnectionString);
        }

        public async Task GivenIHaveTheFollowingEntities(params ToDoItemEntity[] entities)
        {
            var operation = new TableBatchOperation();

            foreach (var entity in entities)
            {
                operation.Insert(entity);
            }

            await this.table.ExecuteBatchAsync(operation).ConfigureAwait(false);
        }

        public Task WhenIGetAsync(string accountId, string id)
        {
            return this.RecordExceptionAsync(() => this.repository.GetAsync(accountId, id));
        }

        public Task WhenIGetAsync(string accountId)
        {
            return this.RecordExceptionAsync(() => this.repository.GetAsync(accountId));
        }

        public Task WhenIAddAsync(ToDoItem toDoItem)
        {
            return this.RecordExceptionAsync(() => this.repository.AddAsync(toDoItem));
        }

        protected override AzureTableRepositorySteps GetStepClass()
        {
            return this;
        }
    }
}