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
    }
}
