using Core.Repositories.Entities;

namespace AutomationTests.TestDataBuilders
{
    internal sealed class ToDoItemEntityBuilder
    {

        private string rowKey = "rowKey-1";
        private string partitionKey = "partition-1";
        private string name = "todo name 1";
        private string description = "default description";
        private bool isComplete = true;

        public ToDoItemEntityBuilder WithRowKey(string id)
        {
            this.rowKey = id;
            return this;
        }

        public ToDoItemEntityBuilder WithPartitionKey(string accountId)
        {
            this.partitionKey = accountId;
            return this;
        }

        public ToDoItemEntityBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public ToDoItemEntityBuilder WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        public ToDoItemEntityBuilder WithIsComplete(bool isComplete)
        {
            this.isComplete = isComplete;
            return this;
        }

        public ToDoItemEntity Build()
        {
            return new ToDoItemEntity
            {
                RowKey = this.rowKey,
                PartitionKey = this.partitionKey,
                Name = this.name,
                Description = this.description,
                IsComplete = this.isComplete,
            };
        }
    }
}