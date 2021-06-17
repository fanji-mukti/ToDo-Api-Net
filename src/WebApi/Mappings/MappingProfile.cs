namespace WebApi.Mappings
{
    using AutoMapper;
    using Core.Models;
    using WebApi.Models.V1;

    /// <summary>
    ///  Provides a named configuration for maps.
    /// </summary>
    public sealed class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            this.CreateMap<ToDoItem, ToDoItemResponse>().ReverseMap();
            this.CreateMap<ToDoItem, ToDoItemRequest>().ReverseMap();
        }
    }
}