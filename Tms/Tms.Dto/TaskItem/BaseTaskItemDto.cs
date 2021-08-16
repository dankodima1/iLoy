using System;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using Tms.Enum;
using Tms.Dto.Base;
using Tms.Dto.Extensions;

namespace Tms.Dto
{
    public abstract class BaseTaskItemDto : BaseDto
    {
        [JsonProperty("name")]
        [Required(ErrorMessage = "name is required")]
        [StringLength(DtoExtensions.TaskItem_Name_MaxLength, ErrorMessage = DtoExtensions.Message_TaskItem_Name_MaxLength)]
        public string Name { get; set; }

        [JsonProperty("description")]
        [StringLength(DtoExtensions.TaskItem_Description_MaxLength, ErrorMessage = DtoExtensions.Message_TaskItem_Description_MaxLength)]
        public string Description { get; set; }

        [JsonProperty("startDateUtc")]
        public DateTime? StartDateUtc { get; set; }

        [JsonProperty("finishDateUtc")]
        public DateTime? FinishDateUtc { get; set; }

        [JsonProperty("state")]
        public TaskItemState State { get; set; }
    }
}
