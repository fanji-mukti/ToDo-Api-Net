namespace Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Models;

    /// <summary>
    /// Define the the funcationality of To Do Service.
    /// </summary>
    public interface IToDoService
    {
        /// <summary>
        /// Retrieve to-do item by account id.
        /// </summary>
        /// <param name="accountId">The account id of to-do items to retrieve.</param>
        /// <returns>A collection of <see cref="ToDoItem"/>.</returns>
        Task<IEnumerable<ToDoItem>> RetrieveAsync(string accountId);

        /// <summary>
        /// Retrieve to-do item.
        /// </summary>
        /// <param name="accountId">The account id of the to-do item to retrieve.</param>
        /// <param name="id">The id of the to-do item to retrieve.</param>
        /// <returns>The <see cref="ToDoItem"/>.</returns>
        Task<ToDoItem> RetrieveAsync(string accountId, string id);

        /// <summary>
        /// Update the to-do item.
        /// </summary>
        /// <param name="itemToUpdate">The updated <see cref="ToDoItem"/>.</param>
        /// <returns>A <see cref="Task"/> of updating to-do item.</returns>
        Task UpdateAsync(ToDoItem itemToUpdate);

        /// <summary>
        /// Create new to-do item.
        /// </summary>
        /// <param name="itemToCreate">The <see cref="ToDoItem"/> to create.</param>
        /// <returns>The newly create <see cref="ToDoItem"/>.</returns>
        Task<ToDoItem> CreateAsync(ToDoItem itemToCreate);
    }
}