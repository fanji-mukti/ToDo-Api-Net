namespace AutomationTests.DataAccessLayerTests
{
    using System;
    using System.Threading.Tasks;
    using AutomationTests.TestHelpers;
    using Core.Models;
    using Core.Repositories.Entities;
    using Xunit;

    [Collection(nameof(StorageEmulatorCollectionFixture))]
    public sealed class AzureTableRepositoryTests : IDisposable
    {
        private readonly AzureTableRepositorySteps steps;

        private bool disposed;

        public AzureTableRepositoryTests(AzureStorageEmulator storageEmulator)
        {
            this.steps = new AzureTableRepositorySteps(storageEmulator);
        }

        [Fact]
        public async Task GetAsync_EntityFound_ReturnToDoItem()
        {
            var entity = new ToDoItemEntity("account_1", "id-1")
            {
                Name = "asd",
                Description = "asd",
                IsComplete = true,
            };

            await this.steps.GivenIHaveTheFollowingEntities(entity).ConfigureAwait(false);
            await this.steps.WhenIGetAsync("account_1", "id-1").ConfigureAwait(false);

            this.steps.ThenTheResultShouldBe(new ToDoItem());
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