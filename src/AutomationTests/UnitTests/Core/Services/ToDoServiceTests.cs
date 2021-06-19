namespace AutomationTests.UnitTests.Core.Services
{
    using AutomationTests.TestDataBuilders;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public sealed class ToDoServiceTests
    {
        private const string RequestedAccountId = "account-1";
        private const string RequestedToDoItemId = "todo-itemid-1";

        private readonly ToDoServiceSteps steps = new();

        [Fact]
        public void Initialization_NullRepository_ThrowArgumentNullException()
        {
            this.steps
                .WhenIInitialize(isNullRepository: true)
                .ThenExceptionShouldBeThrown(typeof(ArgumentNullException));
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public async Task RetrieveAsyncWithAccountId_InvalidParameter_ThrowException(string requestedAccountId, Type expectedExceptionType)
        {
            await this.steps
                .WhenIRetrieveAsync(requestedAccountId)
                .ConfigureAwait(false);

            this.steps.ThenExceptionShouldBeThrown(expectedExceptionType);
        }

        [Fact]
        public async Task RetrieveAsyncWithAccountId_ValidParameter_ReturnCollectionOfToDoItem()
        {
            var toDoItems = new[]
            {
                new ToDoItemBuilder()
                    .WithName("item 1")
                    .Build(),
                new ToDoItemBuilder()
                    .WithDescription("some description")
                    .WithIsComplete(false)
                    .Build(),
            };

            await this.steps
                .GivenIHaveTheFollowingToDoItems(RequestedAccountId, toDoItems)
                .WhenIRetrieveAsync(RequestedAccountId)
                .ConfigureAwait(false);

            this.steps
                .ThenTheResultShouldBe(toDoItems);
        }
    }
}