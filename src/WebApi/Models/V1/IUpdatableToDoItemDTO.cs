namespace WebApi.Models.V1
{
    /// <summary>
    /// Define updatable properties of to-do item.
    /// </summary>
    public interface IUpdatableToDoItemDTO
    {
        /// <summary>
        /// Get the name of the to-do item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get the description of the to-do item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get the value indicating whether the to-do item is complete.
        /// </summary>
        public bool IsComplete { get; set; }
    }
}