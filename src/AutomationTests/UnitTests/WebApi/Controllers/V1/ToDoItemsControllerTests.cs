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
        public async Task GetAsyncWithAccountId_ToDoItemsFound_ReturnCollectionOfToDoItemResponses()
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
                .Select(item => new ToDoItemResponseBuilder().From(item).Build());

            await this.steps
                .GivenIHaveTheFollowingToDoItems(RequestedAccountId, toDoItems)
                .WhenIGet(RequestedAccountId)
                .ConfigureAwait(false);

            this.steps.ThenItShouldReturnOkWithValue(expected);
        }

        [Fact]
        public async Task GetAsyncWithAccountIdAndId_ToDoItemFound_ReturnToDoItemResponse()
        {
            var toDoItem = new ToDoItemBuilder()
                .WithId(RequestedToDoItemId)
                .WithAccountId(RequestedAccountId)
                .Build();

            var expected = new ToDoItemResponseBuilder()
                .From(toDoItem)
                .Build();

            await this.steps
                .GivenIHaveTheFollowingToDoItem(RequestedAccountId, RequestedToDoItemId, toDoItem)
                .WhenIGet(RequestedAccountId, RequestedToDoItemId)
                .ConfigureAwait(false);

            this.steps.ThenItShouldReturnOkWithValue(expected);
        }

        [Fact]
        public async Task GetAsyncWithAccountIdAndId_ToDoItemNotFound_ReturnNotFound()
        {
            await this.steps
                .GivenIHaveTheFollowingToDoItem(RequestedAccountId, RequestedToDoItemId, null)
                .WhenIGet(RequestedAccountId, RequestedToDoItemId)
                .ConfigureAwait(false);

            this.steps.ThenItShouldReturnNotFound();
        }
    
        [Fact]
        public async Task PutAsync_ValidRequest_ReturnNoContent()
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
                .WhenIPut(RequestedAccountId, RequestedToDoItemId, updateRequest)
                .ConfigureAwait(false);

            this.steps
                .ThenTheToDoItemShouldBeUpdatedAs(expected)
                .ThenItShouldReturnNoContent();
        }

        [Fact]
        public async Task PutAsync_ToDoItemNotFound_ReturnNotFound()
        {
            var updateRequest = new ToDoItemRequestBuilder().Build();

            await this.steps
                .GivenIHaveTheFollowingToDoItem(RequestedAccountId, RequestedToDoItemId, null)
                .WhenIPut(RequestedAccountId, RequestedToDoItemId, updateRequest)
                .ConfigureAwait(false);

            this.steps.ThenItShouldReturnNotFound();
        }

        [Fact]
        public async Task PutAsync_InvalidRequest_ReturnBadRequest()
        {
            var updateRequest = new ToDoItemRequestBuilder()
                .WithName(string.Empty)
                .Build();

            await this.steps
                .GivenTheModelStateIsNotValid()
                .WhenIPut(RequestedAccountId, RequestedToDoItemId, updateRequest)
                .ConfigureAwait(false);

            this.steps
                .ThenItShouldReturnBadRequest();
        }

        [Fact]
        public async Task PatchAsync_ValidRequest_ReturnOk()
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
                .From(expected)
                .Build();

            await this.steps
                .GivenIHaveTheFollowingToDoItem(RequestedAccountId, RequestedToDoItemId, existingToDoItem)
                .GivenIamAbleToUpdateToDoItem()
                .WhenIPatch(RequestedAccountId, RequestedToDoItemId, patchRequest)
                .ConfigureAwait(false);

            this.steps
                .ThenTheToDoItemShouldBeUpdatedAs(expected)
                .ThenItShouldReturnOkWithValue(expectedResponse);
        }

        [Fact]
        public async Task PatchAsync_ToDoItemNotFound_ReturnNotFound()
        {
            var patchRequest = new ToDoItemPatchRequestBuilder()
                .WithDescription("update")
                .Build();

            await this.steps
                .GivenIHaveTheFollowingToDoItem(RequestedAccountId, RequestedToDoItemId, null)
                .WhenIPatch(RequestedAccountId, RequestedToDoItemId, patchRequest)
                .ConfigureAwait(false);

            this.steps.ThenItShouldReturnNotFound();
        }

        [Fact]
        public async Task PostAsync_ValidRequest_ReturnCreatedAtWithCorrectResponse()
        {
            var createRequest = new ToDoItemRequestBuilder().Build();
            var expected = new ToDoItemResponseBuilder()
                .From(createRequest)
                .WithId(null)
                .WithAccountId(RequestedAccountId)
                .Build();

            await this.steps
                .GivenIamAbleToCreateToDoItem()
                .WhenIPost(RequestedAccountId, createRequest)
                .ConfigureAwait(false);

            this.steps.ThenItShouldReturnCreatedAtWithValue(expected);
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_ReturnBadRequest()
        {
            var createRequest = new ToDoItemRequestBuilder().Build();

            await this.steps
                .GivenTheModelStateIsNotValid()
                .WhenIPost(RequestedAccountId, createRequest)
                .ConfigureAwait(false);

            this.steps.ThenItShouldReturnBadRequest();
        }
    }
}