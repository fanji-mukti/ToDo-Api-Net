namespace AutomationTests.DataAccessLayerTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutomationTests.TestDataBuilders;
    using AutomationTests.TestHelpers;
    using Core.Models;
    using Xunit;

    [Collection(nameof(StorageEmulatorCollectionFixture))]
    public sealed class AzureTableRepositoryTests
    {
        private readonly AzureTableRepositorySteps steps;

        public AzureTableRepositoryTests(AzureStorageEmulator storageEmulator)
        {
            this.steps = new AzureTableRepositorySteps(storageEmulator);
        }

        [Fact]
        public async Task GetAsyncWithAccountIdAndId_EntityFound_ReturnToDoItem()
        {
            var requestedAccountId = "account-1";
            var requestedToDoItemId = "todo-itemid-1";

            var entity = new ToDoItemEntityBuilder()
                .WithRowKey(requestedToDoItemId)
                .WithPartitionKey(requestedAccountId)
                .Build();

            var expected = new ToDoItemBuilder()
                .From(entity)
                .Build();

            await this.steps.GivenIHaveTheFollowingEntities(entity).ConfigureAwait(false);
            await this.steps.WhenIGetAsync(requestedAccountId, requestedToDoItemId).ConfigureAwait(false);

            this.steps.ThenTheResultShouldBe(expected);
        }

        [Fact]
        public async Task GetAsyncWithAccountIdAndId_EntityNotFound_ReturnNull()
        {
            await this.steps.WhenIGetAsync("notExists", "notExists").ConfigureAwait(false);
            this.steps
                .ThenNoExceptionShouldBeThrown()
                .ThenTheResultShouldBe((ToDoItem)null);
        }

        [Fact]
        public async Task GetAsyncWithAccountId_EntityFound_ReturnCollectionOfToDoItems()
        {
            var accountId = "test account 2";

            var entities = new[]
            {
                new ToDoItemEntityBuilder()
                    .WithRowKey("row 1")
                    .WithPartitionKey(accountId)
                    .Build(),
                new ToDoItemEntityBuilder()
                    .WithRowKey("row 2")
                    .WithPartitionKey(accountId)
                    .WithIsComplete(false)
                    .Build(),
            };

            var expected = entities
                .Select(entity => new ToDoItemBuilder().From(entity).Build())
                .ToArray();

            await this.steps.GivenIHaveTheFollowingEntities(entities).ConfigureAwait(false);
            await this.steps.WhenIGetAsync(accountId).ConfigureAwait(false);
            this.steps.ThenTheResultShouldBe(expected);
        }
        
        [Fact]
        public async Task GetAsyncWithAccountId_EntityNotFound_ReturnEmptyCollection()
        {
            await this.steps.WhenIGetAsync("notExists").ConfigureAwait(false);
            this.steps.ThenTheResultShouldBe(Array.Empty<ToDoItem>());
        }
    }
}