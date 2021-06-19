namespace AutomationTests.TestDataBuilders
{
    using Microsoft.AspNetCore.JsonPatch;
    using WebApi.Models.V1;

    internal sealed class ToDoItemPatchRequestBuilder
    {
        private string name;
        private string description;
        private bool? isComplete;
        
        public ToDoItemPatchRequestBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public ToDoItemPatchRequestBuilder WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        public ToDoItemPatchRequestBuilder WithIsComplete(bool isComplete)
        {
            this.isComplete = isComplete;
            return this;
        }

        public ToDoItemPatchRequestBuilder From(ToDoItemRequest request)
        {
            this.name = request.Name;
            this.description = request.Description;
            this.isComplete = request.IsComplete;
            return this;
        }

        public JsonPatchDocument<IUpdatableToDoItemDTO> Build()
        {
            var patchRequest = new JsonPatchDocument<IUpdatableToDoItemDTO>();

            if (this.name != null)
            {
                patchRequest.Replace(x => x.Name, this.name);
            }

            if (this.description != null)
            {
                patchRequest.Replace(x => x.Description, this.description);
            }

            if (this.isComplete.HasValue)
            {
                patchRequest.Replace(x => x.IsComplete, this.isComplete);
            }

            return patchRequest;
        }
    }
}