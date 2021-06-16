namespace WebApi.Mappings
{
    using AutoMapper;

    /// <summary>
    /// Provides configuration for mapper.
    /// </summary>
    public static class AutoMapperConfig
    {
        /// <summary>
        /// Initialize mapper.
        /// </summary>
        /// <returns>The configured <see cref="IMapper"/>.</returns>
        public static IMapper Initialize()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfile());
            });

            return mapperConfig.CreateMapper();
        }
    }
}