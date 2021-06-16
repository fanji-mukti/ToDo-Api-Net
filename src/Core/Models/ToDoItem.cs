namespace Core.Models
{
    public sealed class ToDoItem
    {
        /// <summary>
        /// Get the to-do item id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Get the account Id that associate with the to-do item.
        /// </summary>
        public string AccountId { get; set; }

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