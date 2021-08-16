using System.Collections.Generic;

using Tms.Data.Domain;
using Tms.Dto;

namespace Tms.Api.Mapping
{
    public static class TaskItemDtoMapping
    {
        // Copy

        public static TaskItem CopyTo(this TaskItem item, TaskItem itemDest)
        {
            return item.MapTo(itemDest);
        }

        public static TaskItemDto CopyTo(this TaskItemDto item, TaskItemDto itemDest)
        {
            return item.MapTo(itemDest);
        }

        // TaskItem

        public static TaskItemDto ToDto(this TaskItem item)
        {
            return item.MapTo<TaskItem, TaskItemDto>();
        }

        public static TaskItem ToEnt(this TaskItemDto item)
        {
            return item.MapTo<TaskItemDto, TaskItem>();
        }

        public static TaskItem ToEnt(this TaskItemDto item, TaskItem itemDest)
        {
            return item.MapTo(itemDest);
        }

        public static IList<TaskItemDto> ToDto(this IList<TaskItem> items)
        {
            return items.MapTo<IList<TaskItem>, IList<TaskItemDto>>();
        }

        public static IList<TaskItem> ToEnt(this IList<TaskItemDto> itemDtos)
        {
            return itemDtos.MapTo<IList<TaskItemDto>, IList<TaskItem>>();
        }

        // SubtaskItem

        public static SubtaskDto ToSubtaskDto(this TaskItem item)
        {
            return item.MapTo<TaskItem, SubtaskDto>();
        }

        public static TaskItem ToEnt(this SubtaskDto item)
        {
            return item.MapTo<SubtaskDto, TaskItem>();
        }

        public static IList<SubtaskDto> ToSubtaskDto(this IList<TaskItem> items)
        {
            return items.MapTo<IList<TaskItem>, IList<SubtaskDto>>();
        }

        public static IList<TaskItem> ToEnt(this IList<SubtaskDto> itemDtos)
        {
            return itemDtos.MapTo<IList<SubtaskDto>, IList<TaskItem>>();
        }
    }
}
