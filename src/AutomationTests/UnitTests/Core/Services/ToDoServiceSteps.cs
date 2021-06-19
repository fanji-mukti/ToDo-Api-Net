namespace AutomationTests.UnitTests.Core.Services
{
    using global::Core.Models;
    using global::Core.Repositories;
    using global::Core.Services;
    using Moq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal sealed class ToDoServiceSteps : BaseTestSteps<ToDoServiceSteps>
    {
        private readonly Mock<IRepository<ToDoItem>> mockRepository = new();
        private readonly ToDoService toDoService;

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

        protected override ToDoServiceSteps GetStepClass()
        {
            return this;
        }
    }
}