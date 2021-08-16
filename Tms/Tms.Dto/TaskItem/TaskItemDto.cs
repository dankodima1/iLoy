using System.Collections.Generic;

using Newtonsoft.Json;

namespace Tms.Dto
{
    public class TaskItemDto : BaseTaskItemDto
    {
        [JsonProperty("subtasks")]
        public virtual List<SubtaskDto> Subtasks { get; set; }
    }
}
