namespace AutomationTests.TestDataBuilders
{
    using WebApi.Models.V1;

    internal sealed class ToDoItemRequestBuilder
    {
        private string name = "todo name 1";
        private string description = "default description";
        private bool isComplete = true;

        public ToDoItemRequestBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public ToDoItemRequestBuilder WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        public ToDoItemRequestBuilder WithIsComplete(bool isComplete)
        {
            this.isComplete = isComplete;
            return this;
        }

        public ToDoItemRequest Build()
        {
            return new ToDoItemRequest
            {
                Name = this.name,
                Description = this.description,
                IsComplete = this.isComplete,
            };
        }
    }
}