namespace AutomationTests.IntegrationTests
{
    using System;
    using System.Linq.Expressions;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using AutomationTests.TestHelpers;
    using Core.Repositories.Entities;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Azure.Cosmos.Table;
    using Newtonsoft.Json;
    using WebApi.Models.V1;

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

        public Task WhenIPostAsync(string requestUri, ToDoItemRequest request)
        {
            var jsonBody = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            return this.RecordExceptionAsync(() => this.client.PostAsync(requestUri, content));
        }

        public WebApiSteps ThenTheResponseStatusCodeShouldBe(HttpStatusCode expected)
        {
            var response = (HttpResponseMessage)this.Result;
            response.StatusCode.Should().BeEquivalentTo(expected);
            return this;
        }

        public async Task ThenTheResponseBodyShouldBe<T>(T expected)
        { 
            var responseBody = await GetResponseBodyAsync<T>((HttpResponseMessage)this.Result).ConfigureAwait(false);
            responseBody.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
        }

        public async Task ThenTheResponseBodyShouldBe(ToDoItemResponse expected, Expression<Func<ToDoItemResponse, object>> excludeExpression)
        { 
            var responseBody = await GetResponseBodyAsync<ToDoItemResponse>((HttpResponseMessage)this.Result).ConfigureAwait(false);
            responseBody.Should().BeEquivalentTo(expected, options => options.Excluding(x => x.Id));
        }

        protected override WebApiSteps GetStepClass()
        {
            return this;
        }

        private static async Task<T> GetResponseBodyAsync<T>(HttpResponseMessage response)
        {
            var stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(stringContent);
        }
    }
}