namespace WebApi.Models
{
    public class ToDoItemRequest
    {
        /// <summary>
        /// Get the name of the to-do item.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Get the description of the to-do item.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Get the value indicating whether the to-do item is complete.
        /// </summary>
        public bool IsComplete { get; init; }
    }
}