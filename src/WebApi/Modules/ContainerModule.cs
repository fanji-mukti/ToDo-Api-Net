namespace WebApi.Modules
{
    using Autofac;
    using Core.Models;
    using Core.Repositories;
    using Core.Services;
    using EnsureThat;
    using WebApi.Mappings;

    /// <summary>
    /// This class register all dependency needed to run the Web API.
    /// </summary>
    public sealed class ContainerModule : Module
    {
        private readonly string storageConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerModule"/> class.
        /// </summary>
        /// <param name="connectionString">The storage connection string.</param>
        public ContainerModule(string connectionString)
        {
            this.storageConnectionString = EnsureArg.IsNotNullOrWhiteSpace(connectionString);
        }

        /// <inheritdoc/>
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(c => RepositoryFactory.CreateToDoItemRepository(this.storageConnectionString))
                .As<IRepository<ToDoItem>>()
                .SingleInstance();

            builder
                .Register(c => new ToDoService(c.Resolve<IRepository<ToDoItem>>()))
                .As<IToDoService>();

            builder
                .RegisterInstance(AutoMapperConfig.Initialize())
                .SingleInstance();
        }
    }
}