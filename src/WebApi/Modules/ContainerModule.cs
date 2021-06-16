namespace WebApi.Modules
{
    using Autofac;
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
            builder
                .Register(c => new ToDoService())
                .As<IToDoService>();

            builder
                .RegisterInstance(AutoMapperConfig.Initialize())
                .SingleInstance();
        }
    }
}