namespace Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Models;

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
    }
}