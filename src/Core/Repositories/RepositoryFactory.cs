namespace Core.Repositories
{
    using AutoMapper;
    using Core.Models;
    using Core.Repositories.Entities;
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// Provides functionality to instatiate repository.
    /// </summary>
    public static class RepositoryFactory
    {
        private const string ToDoItemTableName = "ToDoItem";

        /// <summary>
        /// Create repository for to-do item.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The repository for to-do item.</returns>
        public static IRepository<ToDoItem> CreateToDoItemRepository(string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(ToDoItemTableName);

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfile());
            });

            var mapper = mapperConfig.CreateMapper();

            return new AzureTableRepository<ToDoItem, ToDoItemEntity>(table, mapper);
        }
    }
}