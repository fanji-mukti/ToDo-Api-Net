namespace AutomationTests.UnitTests.Core.Services
{
    using global::Core.Models;
    using global::Core.Repositories;
    using global::Core.Services;
    using Moq;

    internal sealed class ToDoServiceSteps : BaseTestSteps<ToDoServiceSteps>
    {
        private readonly Mock<IRepository<ToDoItem>> mockRepository = new();
        private readonly ToDoService toDoService;

        public ToDoServiceSteps()
        {
            this.toDoService = new ToDoService(this.mockRepository.Object);
        }

        protected override ToDoServiceSteps GetStepClass()
        {
            return this;
        }
    }
}