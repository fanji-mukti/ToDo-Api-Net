namespace WebApi.Modules
{
    using Autofac;
    using Core.Models;
    using Core.Repositories;
    using Core.Services;
    using WebApi.Mappings;

    /// <summary>
    /// This class register all dependency needed to run the Web API.
    /// </summary>
    public sealed class ContainerModule : Module
    {
        /// <inheritdoc/>
        protected override void Load(ContainerBuilder builder)
        {
            var connectionString = @"DefaultEndpointsProtocol=https;AccountName=muktisandboxstorage;AccountKey=LpYqeX/5+lI74G5mD29wA+Cwzktd3z12vSKsdHkNfQ9jFNblH3zeDoh4o4SwpaZK0hU99PbT1kBn2oBJTTUcyA==;EndpointSuffix=core.windows.net";

            builder
                .Register(c => RepositoryFactory.CreateToDoItemRepository(connectionString))
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