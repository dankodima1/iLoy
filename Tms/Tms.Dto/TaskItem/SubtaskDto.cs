using Newtonsoft.Json;

namespace Tms.Dto
{
    public class SubtaskDto : BaseTaskItemDto
    {
        [JsonProperty("parentId")]
        public int? ParentId { get; set; }
    }
}
