namespace AutomationTests.UnitTests.WebApi.Controllers.V1
{
    using AutomationTests.TestDataBuilders;
    using Core.Models;
    using global::WebApi.Models.V1;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public sealed class ToDoItemsControllerTests
    {
        private readonly ToDoItemsControllerSteps steps = new ToDoItemsControllerSteps();

        [Fact]
        public async Task GetWithAccountId_ToDoItemsFound_ReturnCollectionOfToDoItemResponses()
        {
            var requestedAccountId = "account-1";

            var toDoItems = new[]
            {
                new ToDoItemBuilder()
                    .WithAccountId(requestedAccountId)
                    .Build(),
                new ToDoItemBuilder()
                    .WithId("2")
                    .WithAccountId(requestedAccountId)
                    .Build(),
            };

            var expected = toDoItems
                .Select(item => new ToDoItemResponseBuilder().FromToDoItem(item).Build());

            await this.steps
                .GivenIHaveTheFollowingToDoItems(requestedAccountId, toDoItems)
                .WhenICallGet(requestedAccountId)
                .ConfigureAwait(false);

            this.steps.ThenTheActionResultValueShouldBe(expected);
        }
    }
}