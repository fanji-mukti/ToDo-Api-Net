namespace WebApi.Models.V1
{
    /// <summary>
    /// Represents the response of to-do item request.
    /// </summary>
    public class ToDoItemResponse : IUpdatableToDoItemDTO
    {
        /// <summary>
        /// Gets the to-do item id.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets the account Id that associate with the to-do item.
        /// </summary>
        public string AccountId { get; init; }

        /// <summary>
        /// Gets the name of the to-do item.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the description of the to-do item.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Gets a value indicating whether the to-do item is complete.
        /// </summary>
        public bool IsComplete { get; init; }
    }
}