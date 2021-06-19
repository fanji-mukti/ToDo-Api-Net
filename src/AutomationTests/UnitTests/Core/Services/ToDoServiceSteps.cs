namespace AutomationTests.UnitTests.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Core.Models;
    using global::Core.Repositories;
    using global::Core.Services;
    using FluentAssertions;
    using Moq;

    internal sealed class ToDoServiceSteps : BaseTestSteps<ToDoServiceSteps>
    {
        private readonly Mock<IRepository<ToDoItem>> mockRepository = new();
        private readonly ToDoService toDoService;
        
        private ToDoItem createdItem;

        public ToDoServiceSteps()
        {
            this.toDoService = new ToDoService(this.mockRepository.Object);
        }

        public ToDoServiceSteps GivenIHaveTheFollowingToDoItems(string accountId, IEnumerable<ToDoItem> toDoItems)
        {
            this.mockRepository
                .Setup(x => x.GetAsync(accountId))
                .ReturnsAsync(toDoItems);

            return this;
        }

        public ToDoServiceSteps GivenIHaveTheFollowingToDoItem(string accountId, string id, ToDoItem toDoItem)
        {
            this.mockRepository
                .Setup(x => x.GetAsync(accountId, id))
                .ReturnsAsync(toDoItem);

            return this;
        }

        public ToDoServiceSteps GivenIAmAbleToCreateToDoItem()
        {
            this.mockRepository
                .Setup(x => x.AddAsync(It.IsAny<ToDoItem>()))
                .Callback<ToDoItem>(request =>
                {
                    this.createdItem = request;
                });

            return this;
        }

        public ToDoServiceSteps WhenIInitialize(bool isNullRepository)
        {
            var repo = isNullRepository ?
                null :
                this.mockRepository.Object;

            return this.RecordException(() => new ToDoService(repo));
        }

        public Task WhenIRetrieveAsync(string accountId)
        {
            return this.RecordExceptionAsync(() => this.toDoService.RetrieveAsync(accountId));
        }

        public Task WhenIRetrieveAsync(string accountId, string id)
        {
            return this.RecordExceptionAsync(() => this.toDoService.RetrieveAsync(accountId, id));
        }

        public Task WhenICreateAsync(ToDoItem toDoItem)
        {
            return this.RecordExceptionAsync(() => this.toDoService.CreateAsync(toDoItem));
        }

        public ToDoServiceSteps ThenTheToDoItemShouldBeCreated(ToDoItem expected)
        {
            this.createdItem.Should().BeEquivalentTo(expected, options => options.Excluding(x => x.Id));
            return this;
        }

        public ToDoServiceSteps ThenTheReturnResultShouldBe(ToDoItem expected)
        {
            var returnResult = this.Result as ToDoItem;
            returnResult.Should().BeEquivalentTo(expected, options => options.Excluding(x => x.Id));
            return this;
        }

        protected override ToDoServiceSteps GetStepClass()
        {
            return this;
        }
    }
}