namespace AutomationTests.TestDataBuilders
{
    using Core.Models;
    using WebApi.Models.V1;

    internal sealed class ToDoItemResponseBuilder
    {
        private string id = "todo-1";
        private string accountId = "account-1";
        private string name = "todo name 1";
        private string description = "default description";
        private bool isComplete = true;

        public ToDoItemResponseBuilder WithId(string id)
        {
            this.id = id;
            return this;
        }

        public ToDoItemResponseBuilder WithAccountId(string accountId)
        {
            this.accountId = accountId;
            return this;
        }

        public ToDoItemResponseBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public ToDoItemResponseBuilder WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        public ToDoItemResponseBuilder WithIsComplete(bool isComplete)
        {
            this.isComplete = isComplete;
            return this;
        }

        public ToDoItemResponseBuilder From(ToDoItem toDoItem)
        {
            this.id = toDoItem.Id;
            this.accountId = toDoItem.AccountId;
            this.name = toDoItem.Name;
            this.description = toDoItem.Description;
            this.isComplete = toDoItem.IsComplete;

            return this;
        }

        public ToDoItemResponseBuilder From(ToDoItemRequest request)
        {
            this.name = request.Name;
            this.description = request.Description;
            this.isComplete = request.IsComplete;

            return this;
        }

        public ToDoItemResponse Build()
        {
            return new ToDoItemResponse
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