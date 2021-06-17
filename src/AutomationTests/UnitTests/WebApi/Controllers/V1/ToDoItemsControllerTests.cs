namespace AutomationTests.UnitTests.WebApi.Controllers.V1
{
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
        public async Task GetWithAccountId_ToDoItemsFound_Success()
        {
            var toDoItems = new[]
            {
                new ToDoItem(),
            };

            var expected = new[]
            {
                new ToDoItemResponse(),
            };

            await this.steps
                .GivenIHaveTheFollowingToDoItems(toDoItems)
                .WhenICallGet("account")
                .ConfigureAwait(false);

            this.steps.ThenTheActionResultValueShouldBe(expected);
        }
    }
}