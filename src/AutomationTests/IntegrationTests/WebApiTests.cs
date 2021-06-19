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
        private const string ToDoItemsUriFormat = "api/v1.0/accounts/{0}/ToDoItems";
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

            var requestUri = String.Format(ToDoItemsUriFormat, accountId);

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

        [Fact]
        public async Task PostAsync_ReturnCreated()
        {
            var accountId = "webapitest-post";

            var request = new ToDoItemRequestBuilder().Build();
            var expected = new ToDoItemResponseBuilder()
                .From(request)
                .WithAccountId(accountId)
                .Build();

            var requestUri = String.Format(ToDoItemsUriFormat, accountId);

            await this.steps.WhenIPostAsync(requestUri, request).ConfigureAwait(false);

            await this.steps
                .ThenTheResponseStatusCodeShouldBe(HttpStatusCode.Created)
                .ThenTheResponseBodyShouldBe(expected, exclude => exclude.Id);
        }

        [Fact]
        public async Task PutAsync_ReturnNoContent()
        {
            var accountId = "webapitest-put";

            var entity = new ToDoItemEntityBuilder()
                .WithRowKey($"{accountId}-1")
                .WithPartitionKey(accountId)
                .Build();

            var updateRequest = new ToDoItemRequestBuilder()
                .WithName("Updated name")
                .WithDescription("Updated description")
                .WithIsComplete(false)
                .Build();

            var requestUri = String.Format(GetByAccountAndIdUriFormat, accountId, entity.RowKey);

            var expected = new ToDoItemResponseBuilder()
                .From(updateRequest)
                .WithId(entity.RowKey)
                .WithAccountId(entity.PartitionKey)
                .Build();

            await this.steps.GivenIHaveTheFollowingEntities(entity).ConfigureAwait(false);
            await this.steps.WhenIPutAsync(requestUri, updateRequest).ConfigureAwait(false);

            this.steps.ThenTheResponseStatusCodeShouldBe(HttpStatusCode.NoContent);

            await this.steps.WhenIGetAsync(requestUri).ConfigureAwait(false);
            await this.steps.ThenTheResponseBodyShouldBe(expected).ConfigureAwait(false);
        }

        [Fact]
        public async Task PatchAsync_ReturnOk()
        {
            var accountId = "webapitest-patch";
            var patchedValue = "patched name";

            var entity = new ToDoItemEntityBuilder()
                .WithRowKey($"{accountId}-1")
                .WithPartitionKey(accountId)
                .Build();

            var patchRequest = new ToDoItemPatchRequestBuilder()
                .WithName(patchedValue)
                .Build();

            var expected = new ToDoItemResponseBuilder()
                .From(entity)
                .WithName(patchedValue)
                .Build();
            
            var requestUri = String.Format(GetByAccountAndIdUriFormat, accountId, entity.RowKey);

            await this.steps.GivenIHaveTheFollowingEntities(entity).ConfigureAwait(false);
            await this.steps.WhenIPatchAsync(requestUri, patchRequest).ConfigureAwait(false);

            await this.steps
                .ThenTheResponseStatusCodeShouldBe(HttpStatusCode.OK)
                .ThenTheResponseBodyShouldBe(expected)
                .ConfigureAwait(false);
        }
    }
}