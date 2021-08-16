using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tms.Dto
{
    public class SubtaskDto : BaseTaskItemDto
    {
        [JsonProperty("parentId")]
        public int? ParentId { get; set; }
    }
}
