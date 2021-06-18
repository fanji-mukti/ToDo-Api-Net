namespace AutomationTests.TestDataBuilders
{
    using Core.Models;
    using WebApi.Models.V1;

    internal sealed class ToDoItemBuilder
    {
        private string id = "todo-1";
        private string accountId = "account-1";
        private string name = "todo name 1";
        private string description = "default description";
        private bool isComplete = true;

        public ToDoItemBuilder WithId(string id)
        {
            this.id = id;
            return this;
        }

        public ToDoItemBuilder WithAccountId(string accountId)
        {
            this.accountId = accountId;
            return this;
        }

        public ToDoItemBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public ToDoItemBuilder WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        public ToDoItemBuilder WithIsComplete(bool isComplete)
        {
            this.isComplete = isComplete;
            return this;
        }

        public ToDoItemBuilder From(ToDoItemRequest request)
        {
            this.name = request.Name;
            this.description = request.Description;
            this.isComplete = request.IsComplete;
            return this;
        }

        public ToDoItem Build()
        {
            return new ToDoItem
            {
                Id = this.id,
                AccountId = this.accountId,
                Name = this.name,
                Description = this.description,
                IsComplete = this.isComplete,
            };
        }
    }
}