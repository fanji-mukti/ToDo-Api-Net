namespace AutomationTests.UnitTests.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using AutomationTests.TestDataBuilders;
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
                .ThenThrownExceptionShouldBe(typeof(ArgumentNullException));
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public async Task RetrieveAsyncWithAccountId_InvalidParameter_ThrowException(string requestedAccountId, Type expectedExceptionType)
        {
            await this.steps
                .WhenIRetrieveAsync(requestedAccountId)
                .ConfigureAwait(false);

            this.steps.ThenThrownExceptionShouldBe(expectedExceptionType);
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

        [Theory]
        [InlineData(null, RequestedToDoItemId, typeof(ArgumentNullException))]
        [InlineData("", RequestedToDoItemId, typeof(ArgumentException))]
        [InlineData(RequestedAccountId, null, typeof(ArgumentNullException))]
        [InlineData(RequestedAccountId, "", typeof(ArgumentException))]
        public async Task RetrieveAsyncWithAccountIdAndId_InvalidParameter_ThrowException(string accountId, string id, Type expectedExceptionType)
        {
            await this.steps
                .WhenIRetrieveAsync(accountId, id)
                .ConfigureAwait(false);

            this.steps
                .ThenThrownExceptionShouldBe(expectedExceptionType);
        }

        [Fact]
        public async Task RetrieveAsyncWithAccountIdAndId_ValidParameter_ReturnToDoItem()
        {
            var todoItem = new ToDoItemBuilder().Build();

            await this.steps
                .GivenIHaveTheFollowingToDoItem(RequestedAccountId, RequestedToDoItemId, todoItem)
                .WhenIRetrieveAsync(RequestedAccountId, RequestedToDoItemId)
                .ConfigureAwait(false);

            this.steps.ThenTheResultShouldBe(todoItem);
        }

        [Fact]
        public async Task CreateAsync_NullParameter_ThrowArgumentNullException()
        {
            await this.steps
                .WhenICreateAsync(null)
                .ConfigureAwait(false);

            this.steps.ThenThrownExceptionShouldBe(typeof(ArgumentNullException));
        }

        [Fact]
        public async Task CreateAsync_ValidParameter_ToDoItemCreatedSuccesfully()
        {
            var toDoItem = new ToDoItemBuilder().Build();

            await this.steps
                .GivenIAmAbleToCreateToDoItem()
                .WhenICreateAsync(toDoItem)
                .ConfigureAwait(false);

            this.steps
                .ThenTheToDoItemShouldBeCreated(toDoItem)
                .ThenTheReturnResultShouldBe(toDoItem);
        }
    }
}