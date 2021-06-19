namespace Core.Repositories.Entities
{
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    ///  Represents the entity of to-do item in the Table service.
    /// </summary>
    internal sealed class ToDoItemEntity : TableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemEntity"/> class.
        /// </summary>
        public ToDoItemEntity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemEntity"/> class.
        /// </summary>
        /// <param name="accountId">The account id that the to-do item belong to.</param>
        /// <param name="id">The id of the to-do item.</param>
        public ToDoItemEntity(string accountId, string id)
            : base(accountId, id)
        {
        }

        /// <summary>
        /// Gets the name of the to-do item.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the description of the to-do item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets a value indicating whether the to-do item is complete.
        /// </summary>
        public bool IsComplete { get; set; }
    }
}