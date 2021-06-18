namespace AutomationTests.UnitTests.WebApi.Controllers.V1
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Core.Models;
    using Core.Services;
    using FluentAssertions;
    using global::WebApi.Controllers.V1;
    using global::WebApi.Mappings;
    using global::WebApi.Models.V1;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Moq;

    internal sealed class ToDoItemsControllerSteps : BaseTestSteps<ToDoItemsControllerSteps>
    {
        private readonly Mock<IToDoService> mockService = new();
        private readonly IMapper mapper = AutoMapperConfig.Initialize();
        private readonly ToDoItemsController controller;

        private ToDoItem actualUpdatedToDoItem;

        public ToDoItemsControllerSteps()
        {
            this.controller = new ToDoItemsController(this.mockService.Object, this.mapper);
        }

        public ToDoItemsControllerSteps GivenIHaveTheFollowingToDoItems(string accountId, IEnumerable<ToDoItem> toDoItems)
        {
            this.mockService
                .Setup(x => x.RetrieveAsync(accountId))
                .ReturnsAsync(toDoItems);

            return this;
        }

        public ToDoItemsControllerSteps GivenIHaveTheFollowingToDoItem(string accountId, string id, ToDoItem toDoItem)
        {
            this.mockService
                .Setup(x => x.RetrieveAsync(accountId, id))
                .ReturnsAsync(toDoItem);

            return this;
        }

        public ToDoItemsControllerSteps GivenIamAbleToUpdateToDoItem()
        {
            this.mockService
                .Setup(x => x.UpdateAsync(It.IsAny<ToDoItem>()))
                .Callback<ToDoItem>(
                    updatedItem =>
                    {
                        this.actualUpdatedToDoItem = updatedItem;
                    })
                .Returns(Task.CompletedTask);

            return this;
        }

        public ToDoItemsControllerSteps GivenTheModelStateIsNotValid()
        { 
            this.controller.ModelState.AddModelError("any property", "Required");
            return this;
        }

        public Task WhenIGet(string accountId)
        {
            return this.RecordExceptionAsync(() => this.controller.Get(accountId));
        }

        public Task WhenIGet(string accountId, string id)
        {
            return this.RecordExceptionAsync(() => this.controller.Get(accountId, id));
        }

        public Task WhenIPut(string accountId, string id, ToDoItemRequest request)
        {
            return this.RecordExceptionAsync(() => this.controller.Put(accountId, id, request));
        }

        public Task WhenIPatch(string accountId, string id, JsonPatchDocument<IUpdatableToDoItemDTO> request)
        {
            return this.RecordExceptionAsync(() => this.controller.Patch(accountId, id, request));
        }

        public ToDoItemsControllerSteps ThenTheToDoItemShouldBeUpdatedAs(ToDoItem expected)
        {
            this.actualUpdatedToDoItem.Should().BeEquivalentTo(expected);
            return this;
        }

        public ToDoItemsControllerSteps ThenItShouldReturnOkWithValue(IEnumerable<ToDoItemResponse> expected)
        {
            var actionResult = this.Result as ActionResult<IEnumerable<ToDoItemResponse>>;
            var okResult = actionResult.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());

            return this;
        }

        public ToDoItemsControllerSteps ThenItShouldReturnOkWithValue(ToDoItemResponse expected)
        {
            var actionResult = this.Result as ActionResult<ToDoItemResponse>;
            var okResult = actionResult.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());

            return this;
        }

        public ToDoItemsControllerSteps ThenItShouldReturnNotFound()
        {
            var actionResult = this.Result as ActionResult<ToDoItemResponse>;
            if (actionResult != default)
            {
                actionResult.Result.Should().BeEquivalentTo(new NotFoundResult());
            }
            else
            {
                this.Result.Should().BeEquivalentTo(new NotFoundResult());
            }

            return this;
        }

        public ToDoItemsControllerSteps ThenItShouldReturnNoContent()
        { 
            this.Result.Should().BeEquivalentTo(new NoContentResult());
            return this;
        }

        public ToDoItemsControllerSteps ThenItShouldReturnBadRequest()
        {
            this.Result.Should().BeEquivalentTo(new BadRequestResult());
            return this;
        }

        protected override ToDoItemsControllerSteps GetStepClass()
        {
            return this;
        }
    }
}