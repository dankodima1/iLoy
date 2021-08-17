using AutoMapper;

using Tms.Data.Domain;
using Tms.Dto;

namespace Tms.Api.Mapping
{
    public class MapperConfigurationApi : Profile
    {
        public MapperConfigurationApi()
        {
            CreateMap<TaskItem, TaskItem>()
                .IgnoreNonExisting()
                .IgnoreBase()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.Subtasks, y => y.Ignore())
                .ForMember(x => x.SubtasksOrdered, y => y.Ignore());

            CreateMap<TaskItem, TaskItemDto>()
                .IgnoreNonExisting()
                .ForMember(x => x.Subtasks, y => y.MapFrom(s => s.SubtasksOrdered));

            CreateMap<TaskItem, SubtaskDto>()
                .IgnoreNonExisting();

            CreateMap<TaskItemDto, TaskItem>()
                .IgnoreNonExisting()
                .ForMember(x => x.Subtasks, y => y.Ignore());

            CreateMap<TaskItemDto, TaskItemDto>()
                .IgnoreNonExisting()
                .ForMember(x => x.Id, y => y.Ignore());
        }
    }
}
