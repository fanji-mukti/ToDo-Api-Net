namespace WebApi.Models.V1
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents to-do item request.
    /// </summary>
    public sealed class ToDoItemRequest : IUpdatableToDoItemDTO
    {
        /// <summary>
        /// Gets the name of the to-do item.
        /// </summary>
        [Required]
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