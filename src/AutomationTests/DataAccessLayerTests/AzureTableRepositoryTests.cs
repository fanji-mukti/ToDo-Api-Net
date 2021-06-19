namespace AutomationTests.DataAccessLayerTests
{
    using System;
    using System.Threading.Tasks;
    using AutomationTests.TestDataBuilders;
    using AutomationTests.TestHelpers;
    using Core.Models;
    using Core.Repositories.Entities;
    using Xunit;

    [Collection(nameof(StorageEmulatorCollectionFixture))]
    public sealed class AzureTableRepositoryTests : IDisposable
    {
        private const string RequestedAccountId = "account-1";
        private const string RequestedToDoItemId = "todo-itemid-1";

        private readonly AzureTableRepositorySteps steps;

        private bool disposed;

        public AzureTableRepositoryTests(AzureStorageEmulator storageEmulator)
        {
            this.steps = new AzureTableRepositorySteps(storageEmulator);
        }

        [Fact]
        public async Task GetAsync_EntityFound_ReturnToDoItem()
        {
            var entity = new ToDoItemEntityBuilder()
                .WithRowKey(RequestedToDoItemId)
                .WithPartitionKey(RequestedAccountId)
                .Build();

            var expected = new ToDoItemBuilder()
                .From(entity)
                .Build();

            await this.steps.GivenIHaveTheFollowingEntities(entity).ConfigureAwait(false);
            await this.steps.WhenIGetAsync(RequestedAccountId, RequestedToDoItemId).ConfigureAwait(false);

            this.steps.ThenTheResultShouldBe(expected);
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.steps.Dispose();
            this.disposed = true;
        }
    }
}