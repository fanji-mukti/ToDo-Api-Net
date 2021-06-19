namespace Core.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Define the contract for repository.
    /// </summary>
    /// <typeparam name="T">The object that being stored in the repository.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Add item to the repository.
        /// </summary>
        /// <param name="itemToAdd">Item to be added.</param>
        /// <returns>Add operation task.</returns>
        Task AddAsync(T itemToAdd);

        /// <summary>
        /// Update item.
        /// </summary>
        /// <param name="itemToUpdate">item to be updated.</param>
        /// <returns>Update operation task.</returns>
        Task UpdateAsync(T itemToUpdate);

        /// <summary>
        /// Get all the item in the partition.
        /// </summary>
        /// <param name="partitionKey">The partition key where the item is stored.</param>
        /// <returns>Collection of item that stored in the partition.</returns>
        Task<IEnumerable<T>> GetAsync(string partitionKey);

        /// <summary>
        /// Get a specific item.
        /// </summary>
        /// <param name="partitionKey">The partition key where to item is stored.</param>
        /// <param name="id">The id of the item.</param>
        /// <returns>The requested item.</returns>
        Task<T> GetAsync(string partitionKey, string id);
    }
}