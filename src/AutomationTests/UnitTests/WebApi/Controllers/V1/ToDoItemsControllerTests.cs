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

            this.steps.ThenItShouldReturnOkWithValue(expected);
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

            this.steps.ThenItShouldReturnOkWithValue(expected);
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
    
        [Fact]
        public async Task Put_ValidRequest_ReturnNoContent()
        {
            var existingToDoItem = new ToDoItemBuilder()
                .WithId(RequestedToDoItemId)
                .WithAccountId(RequestedAccountId)
                .Build();

            var updateRequest = new ToDoItemRequestBuilder()
                .WithName("updated name")
                .WithDescription("updated description")
                .WithIsComplete(false)
                .Build();

            var expected = new ToDoItemBuilder()
                .From(updateRequest)
                .WithId(RequestedToDoItemId)
                .WithAccountId(RequestedAccountId)
                .Build();

            await this.steps
                .GivenIHaveTheFollowingToDoItem(RequestedAccountId, RequestedToDoItemId, existingToDoItem)
                .GivenIamAbleToUpdateToDoItem()
                .WhenICallPut(RequestedAccountId, RequestedToDoItemId, updateRequest)
                .ConfigureAwait(false);

            this.steps
                .ThenTheToDoItemShouldBeUpdatedAs(expected)
                .ThenItShouldReturnNoContent();
        }

        [Fact]
        public async Task Put_ToDoItemNotFound_ReturnNotFound()
        {
            var updateRequest = new ToDoItemRequestBuilder().Build();

            await this.steps
                .GivenIHaveTheFollowingToDoItem(RequestedAccountId, RequestedToDoItemId, null)
                .WhenICallPut(RequestedAccountId, RequestedToDoItemId, updateRequest)
                .ConfigureAwait(false);

            this.steps.ThenItShouldReturnNotFound();
        }

        [Fact]
        public async Task Put_InvalidRequest_ReturnBadRequest()
        {
            var updateRequest = new ToDoItemRequestBuilder()
                .WithName(string.Empty)
                .Build();

            await this.steps
                .GivenTheModelStateIsNotValid()
                .WhenICallPut(RequestedAccountId, RequestedToDoItemId, updateRequest)
                .ConfigureAwait(false);

            this.steps
                .ThenItShouldReturnBadRequest();
        }

        [Fact]
        public async Task Patch_ValidRequest_ReturnOk()
        {
            var existingToDoItem = new ToDoItemBuilder()
                .WithId(RequestedToDoItemId)
                .WithAccountId(RequestedAccountId)
                .Build();

            var patchedDescription = "patched description";
            var patchRequest = new ToDoItemPatchRequestBuilder()
                .WithDescription(patchedDescription)
                .Build();

            var expected = new ToDoItemBuilder()
                .WithId(RequestedToDoItemId)
                .WithAccountId(RequestedAccountId)
                .WithDescription(patchedDescription)
                .Build();

            var expectedResponse = new ToDoItemResponseBuilder()
                .FromToDoItem(expected)
                .Build();

            await this.steps
                .GivenIHaveTheFollowingToDoItem(RequestedAccountId, RequestedToDoItemId, existingToDoItem)
                .GivenIamAbleToUpdateToDoItem()
                .WhenICallPatch(RequestedAccountId, RequestedToDoItemId, patchRequest)
                .ConfigureAwait(false);

            this.steps
                .ThenTheToDoItemShouldBeUpdatedAs(expected)
                .ThenItShouldReturnOkWithValue(expectedResponse);
        }
    }
}