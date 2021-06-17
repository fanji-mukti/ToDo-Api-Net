namespace AutomationTests.UnitTests.WebApi.Controllers.V1
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutomationTests.TestDataBuilders;
    using Xunit;

    public sealed class ToDoItemsControllerTests
    {
        private const string RequestedAccountId = "account-1";
        private const string RequestedToDoItemId = "todo-itemid-1";

        private readonly ToDoItemsControllerSteps steps = new ToDoItemsControllerSteps();

        [Fact]
        public async Task GetWithAccountId_ToDoItemsFound_ReturnCollectionOfToDoItemResponses()
        {
            var toDoItems = new[]
            {
                new ToDoItemBuilder()
                    .WithAccountId(RequestedAccountId)
                    .Build(),
                new ToDoItemBuilder()
                    .WithId("2")
                    .WithAccountId(RequestedAccountId)
                    .Build(),
            };

            var expected = toDoItems
                .Select(item => new ToDoItemResponseBuilder().FromToDoItem(item).Build());

            await this.steps
                .GivenIHaveTheFollowingToDoItems(RequestedAccountId, toDoItems)
                .WhenICallGet(RequestedAccountId)
                .ConfigureAwait(false);

            this.steps.ThenTheActionResultValueShouldBe(expected);
        }

        [Fact]
        public async Task GetWithAccountIdAndId_ToDoItemFound_ReturnToDoItemResponse()
        {
            var toDoItem = new ToDoItemBuilder()
                .WithId(RequestedToDoItemId)
                .WithAccountId(RequestedAccountId)
                .Build();

            var expected = new ToDoItemResponseBuilder()
                .FromToDoItem(toDoItem)
                .Build();

            await this.steps
                .GivenIHaveTheFollowingToDoItem(RequestedAccountId, RequestedToDoItemId, toDoItem)
                .WhenICallGet(RequestedAccountId, RequestedToDoItemId)
                .ConfigureAwait(false);

            this.steps.ThenTheActionResultValueShouldBe(expected);
        }

        [Fact]
        public async Task GetWithAccountIdAndId_ToDoItemNotFound_ReturnNotFound()
        {
            await this.steps
                .GivenIHaveTheFollowingToDoItem(RequestedAccountId, RequestedToDoItemId, null)
                .WhenICallGet(RequestedAccountId, RequestedToDoItemId)
                .ConfigureAwait(false);

            this.steps.ThenItShouldReturnNotFound();
        }
    }
}