using System.Collections.Generic;

using Newtonsoft.Json;

namespace Tms.Dto
{
    public class TaskItemRootDto
    {
        public TaskItemRootDto()
        {
            Values = new List<TaskItemDto>();
        }

        [JsonProperty("values")]
        public IList<TaskItemDto> Values { get; set; }
    }
}
