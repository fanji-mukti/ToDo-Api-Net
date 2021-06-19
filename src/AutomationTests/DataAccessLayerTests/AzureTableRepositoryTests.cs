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

        [Fact]
        public async Task AddAsync_ValidParameter_ToDoItemAddedSuccessfully()
        {
            var toDoItem = new ToDoItemBuilder()
                .WithId("Add Test")
                .WithAccountId("Add Test account")
                .Build();

            await this.steps.WhenIAddAsync(toDoItem).ConfigureAwait(false);
            await this.steps.WhenIGetAsync(toDoItem.AccountId, toDoItem.Id).ConfigureAwait(false);
            this.steps.ThenTheResultShouldBe(toDoItem);
        }

        [Fact]
        public async Task UpdateAsync_ValidParameter_ToDoItemUpdatedSuccessfully()
        {
            var entity = new ToDoItemEntityBuilder()
                .WithRowKey("update test")
                .WithPartitionKey("update test account")
                .Build();

            var updateRequest = new ToDoItemBuilder()
                .From(entity)
                .WithDescription("updated description")
                .WithName("updated name")
                .WithIsComplete(false)
                .Build();

            await this.steps.GivenIHaveTheFollowingEntities(entity).ConfigureAwait(false);
            await this.steps.WhenIUpdateAsync(updateRequest).ConfigureAwait(false);
            await this.steps.WhenIGetAsync(entity.PartitionKey, entity.RowKey).ConfigureAwait(false);
            this.steps.ThenTheResultShouldBe(updateRequest);
        }
    }
}