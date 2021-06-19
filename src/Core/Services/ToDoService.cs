namespace Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Models;
    using Core.Repositories;
    using EnsureThat;

    /// <summary>
    /// Provides functionality related to to-do item.
    /// </summary>
    public sealed class ToDoService : IToDoService
    {
        private readonly IRepository<ToDoItem> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoService"/> class.
        /// </summary>
        /// <param name="repository">The <see cref="IRepository{ToDoItem}"/>.</param>
        public ToDoService(IRepository<ToDoItem> repository)
        {
            this.repository = EnsureArg.IsNotNull(repository);
        }

        /// <inheritdoc/>
        public Task<ToDoItem> CreateAsync(ToDoItem itemToCreate)
        {
            EnsureArg.IsNotNull(itemToCreate, nameof(itemToCreate));

            return InsertAsync();

            async Task<ToDoItem> InsertAsync()
            {
                var createItem = new ToDoItem
                {
                    Id = Guid.NewGuid().ToString(),
                    AccountId = itemToCreate.AccountId,
                    Name = itemToCreate.Name,
                    Description = itemToCreate.Description,
                    IsComplete = itemToCreate.IsComplete,
                };

                await this.repository.AddAsync(createItem).ConfigureAwait(false);
                return createItem;
            }
        }

        /// <inheritdoc/>
        public Task<IEnumerable<ToDoItem>> RetrieveAsync(string accountId)
        {
            EnsureArg.IsNotNullOrWhiteSpace(accountId, nameof(accountId));

            return this.repository.GetAsync(accountId);
        }

        /// <inheritdoc/>
        public Task<ToDoItem> RetrieveAsync(string accountId, string id)
        {
            EnsureArg.IsNotNullOrWhiteSpace(accountId, nameof(accountId));
            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            return this.repository.GetAsync(accountId, id);
        }

        /// <inheritdoc/>
        public Task UpdateAsync(ToDoItem itemToUpdate)
        {
            EnsureArg.IsNotNull(itemToUpdate, nameof(itemToUpdate));

            return this.repository.UpdateAsync(itemToUpdate);
        }
    }
}