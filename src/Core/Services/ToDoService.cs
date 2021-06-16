namespace Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Models;

    public sealed class ToDoService : IToDoService
    {
        ///<inheritdoc/>
        public Task<IEnumerable<ToDoItem>> RetrieveAsync(string accountId)
        {
            throw new System.NotImplementedException();
        }

        ///<inheritdoc/>
        public Task<ToDoItem> RetrieveAsync(string accountId, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}