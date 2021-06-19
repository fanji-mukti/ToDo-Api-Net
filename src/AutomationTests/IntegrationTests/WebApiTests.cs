namespace AutomationTests.IntegrationTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Net;
    using AutomationTests.TestHelpers;
    using AutomationTests.TestDataBuilders;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    [Collection(nameof(StorageEmulatorCollectionFixture))]
    public sealed class WebApiTests : IClassFixture<WebApplicationFactory<WebApi.Startup>>
    {
        private const string GetByAccountUriFormat = "api/v1.0/accounts/{0}/ToDoItems";
        private const string GetByAccountAndIdUriFormat = "api/v1.0/accounts/{0}/ToDoItems/{1}";

        private readonly WebApiSteps steps;

        public WebApiTests(AzureStorageEmulator azureStorageEmulator, WebApplicationFactory<WebApi.Startup> factory)
        {
            this.steps = new WebApiSteps(azureStorageEmulator, factory);
        }

        [Fact]
        public async Task GetByAccount_ReturnOk()
        {
            var accountId = "webapitest-getByaccount";

            var entities = new[]
            {
                new ToDoItemEntityBuilder()
                    .WithRowKey("webapitest-getByAccount-1")
                    .WithPartitionKey(accountId)
                    .Build(),
                new ToDoItemEntityBuilder()
                    .WithRowKey("webapitest-getByAccount-2")
                    .WithPartitionKey(accountId)
                    .Build(),
            };

            var expected = entities
                .Select(entity => new ToDoItemResponseBuilder().From(entity).Build());

            var requestUri = String.Format(GetByAccountUriFormat, accountId);

            await this.steps.GivenIHaveTheFollowingEntities(entities).ConfigureAwait(false);
            await this.steps.WhenIGetAsync(requestUri).ConfigureAwait(false);

            await this.steps
                .ThenTheResponseStatusCodeShouldBe(HttpStatusCode.OK)
                .ThenTheResponseBodyShouldBe(expected)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task GetByAccountAndId_ReturnOk()
        {
            var accountId = "webapitest-getByAccountAndId";

            var entity = new ToDoItemEntityBuilder()
                .WithRowKey($"{accountId}-1")
                .WithPartitionKey(accountId)
                .Build();

            var expected = new ToDoItemResponseBuilder()
                .From(entity)
                .Build();

            var requestUri = String.Format(GetByAccountAndIdUriFormat, accountId, entity.RowKey);

            await this.steps.GivenIHaveTheFollowingEntities(entity).ConfigureAwait(false);
            await this.steps.WhenIGetAsync(requestUri).ConfigureAwait(false);

            await this.steps
                .ThenTheResponseStatusCodeShouldBe(HttpStatusCode.OK)
                .ThenTheResponseBodyShouldBe(expected);
        }
    }
}