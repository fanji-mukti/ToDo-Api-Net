namespace Core.Repositories.Entities
{
    using AutoMapper;
    using Core.Models;

    /// <summary>
    /// Provides Azure Table Entities mapping profile.
    /// </summary>
    internal sealed class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            this.CreateMap<ToDoItem, ToDoItemEntity>()
                .ForMember(dest => dest.RowKey, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PartitionKey, opt => opt.MapFrom(src => src.AccountId))
                .ReverseMap();
        }
    }
}