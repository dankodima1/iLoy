using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using Tms.Dto.Extensions;
using Tms.Enum;

namespace Tms.Data.Domain
{
    [Table("TaskItem")]
    public class TaskItem : BaseEntity
    {
        [Required, MaxLength(DtoExtensions.TaskItem_Name_MaxLength)]
        public string Name { get; set; }
        
        [MaxLength(DtoExtensions.TaskItem_Description_MaxLength)]
        public string Description { get; set; }

        public DateTime? StartDateUtc { get; set; }
        public DateTime? FinishDateUtc { get; set; }
        public TaskItemState State { get; set; }

        #region SUBTASKS

        public int? ParentId { get; set; }
        public virtual TaskItem Parent { get; set; }

        private ICollection<TaskItem> _subtasks;
        public virtual ICollection<TaskItem> Subtasks
        {
            get => _subtasks ?? (_subtasks = new List<TaskItem>());
            protected set => _subtasks = value;
        }

        [NotMapped]
        public virtual IQueryable<TaskItem> SubtasksOrdered
        {
            get => Subtasks.AsQueryable().OrderBy(x => x.Name);
        }

        #endregion SUBTASKS

        #region INFO

        public string TypeName
        {
            get => this.GetType().Name;
        }

        public string Info
        {
            get => $"{TypeName} = ({Id} {Name})";
        }

        public string Show
        {
            get => $"Id = ({Id}) Description = ({Description}) StartDateUtc = ({StartDateUtc}) FinishDateUtc = ({FinishDateUtc}) ParentId = ({ParentId})";
        }

        #endregion INFO

    }
}
