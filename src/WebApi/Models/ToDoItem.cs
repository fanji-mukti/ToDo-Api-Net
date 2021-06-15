using System;

namespace WebApi.Models
{
    public class ToDoItem
    {
        /// <summary>
        /// Get the to-do item id.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Get the account Id that associate with the to-do item.
        /// </summary>
        public string AccountId { get; init; }

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