namespace Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Models;
    using EnsureThat;

    /// <summary>
    /// Provides functionality related to to-do item.
    /// </summary>
    public sealed class ToDoService : IToDoService
    {
        private static readonly List<ToDoItem> ToDoItems = new List<ToDoItem>()
        {
            new ToDoItem
            {
                Id = "1",
                AccountId = "account_1",
                Name = "Learn .Net 5",
                Description = "Learn .Net 5 by creating a simple CRUD Web API",
                IsComplete = false,
            },
            new ToDoItem
            {
                Id = "2",
                AccountId = "account_1",
                Name = "Add Unit Test",
                Description = "Add unit test for the Web API project",
                IsComplete = false,
            },
        };

        /// <inheritdoc/>
        public Task<ToDoItem> CreateAsync(ToDoItem itemToCreate)
        {
            EnsureArg.IsNotNull(itemToCreate, nameof(itemToCreate));

            var createItem = new ToDoItem
            {
                Id = Guid.NewGuid().ToString(),
                AccountId = itemToCreate.AccountId,
                Name = itemToCreate.Name,
                Description = itemToCreate.Description,
                IsComplete = itemToCreate.IsComplete,
            };

            ToDoItems.Add(createItem);
            return Task.FromResult(createItem);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<ToDoItem>> RetrieveAsync(string accountId)
        {
            EnsureArg.IsNotNullOrWhiteSpace(accountId, nameof(accountId));

            var selectedItems = ToDoItems
                .Where(item => item.AccountId.Equals(accountId, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(selectedItems);
        }

        /// <inheritdoc/>
        public Task<ToDoItem> RetrieveAsync(string accountId, string id)
        {
            EnsureArg.IsNotNullOrWhiteSpace(accountId, nameof(accountId));
            EnsureArg.IsNotNullOrWhiteSpace(id, nameof(id));

            var selectedItem = ToDoItems
                .SingleOrDefault(item => item.AccountId.Equals(accountId, StringComparison.OrdinalIgnoreCase)
                    && item.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(selectedItem);
        }

        /// <inheritdoc/>
        public Task UpdateAsync(ToDoItem itemToUpdate)
        {
            EnsureArg.IsNotNull(itemToUpdate, nameof(itemToUpdate));

            var selectedItem = ToDoItems
                .Single(item => item.AccountId.Equals(itemToUpdate.AccountId, StringComparison.OrdinalIgnoreCase)
                    && item.Id.Equals(itemToUpdate.Id, StringComparison.OrdinalIgnoreCase));

            selectedItem.Name = itemToUpdate.Name;
            selectedItem.Description = itemToUpdate.Description;
            selectedItem.IsComplete = itemToUpdate.IsComplete;

            return Task.CompletedTask;
        }
    }
}