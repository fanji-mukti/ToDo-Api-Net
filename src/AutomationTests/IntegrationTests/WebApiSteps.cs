namespace AutomationTests.IntegrationTests
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AutomationTests.TestHelpers;
    using Core.Repositories.Entities;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Azure.Cosmos.Table;
    using Newtonsoft.Json;

    internal sealed class WebApiSteps : BaseTestSteps<WebApiSteps>
    {
        private const string TableName = "ToDoItem";

        private readonly AzureStorageEmulator azureStorageEmulator;
        private readonly WebApplicationFactory<WebApi.Startup> factory;
        private readonly HttpClient client;
        private readonly CloudTable table;

        public WebApiSteps(AzureStorageEmulator azureStorageEmulator, WebApplicationFactory<WebApi.Startup> factory)
        {
            this.factory = factory;
            this.client = factory.CreateClient();

            this.azureStorageEmulator = azureStorageEmulator;
            this.table = azureStorageEmulator.TableClient.CreateTableIfNotExistsAsync(TableName).GetAwaiter().GetResult();
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

        public Task WhenIGetAsync(string requestUri)
        {
            return this.RecordExceptionAsync(() => this.client.GetAsync(requestUri));
        }

        public WebApiSteps ThenTheResponseStatusCodeShouldBe(HttpStatusCode expected)
        {
            var response = (HttpResponseMessage)this.Result;
            response.StatusCode.Should().BeEquivalentTo(expected);
            return this;
        }

        public async Task ThenTheResponseBodyShouldBe<T>(T expected)
        { 
            var response = (HttpResponseMessage)this.Result;
            var stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseBody = JsonConvert.DeserializeObject<T>(stringContent);

            responseBody.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
        }


        protected override WebApiSteps GetStepClass()
        {
            return this;
        }
    }
}