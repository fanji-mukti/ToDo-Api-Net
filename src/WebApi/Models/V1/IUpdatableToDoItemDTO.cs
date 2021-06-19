namespace WebApi.Models.V1
{
    /// <summary>
    /// Define updatable properties of to-do item.
    /// </summary>
    public interface IUpdatableToDoItemDTO
    {
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