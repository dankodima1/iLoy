using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Tms.Data.Domain;
using Tms.Dto;
using Tms.Enum;
using Tms.Enum.Extensions;

namespace Tms.Data.Demo
{
    public class DemoData
    {
        public IEnumerable<TaskItem> GetTaskItems()
        {
            DateTime startDateTimeUtc = new DateTime(2021, 9, 1, 10, 0, 0, DateTimeKind.Utc);
            DateTime finishDateTimeUtc = startDateTimeUtc.AddHours(1);

            // single tasks
            int i = 1;
            for (; i < 6; i++)
            {
                yield return new TaskItem
                {
                    Id = i,
                    Name = $"Task {i:000}",
                    Description = $"Test task has Id = {i:000}",
                    StartDateUtc = startDateTimeUtc.AddDays(i),
                    FinishDateUtc = finishDateTimeUtc.AddDays(i)
                };
            }

            // parent task
            yield return new TaskItem
            {
                Id = i,
                Name = $"Task {i:000} (parent)",
                Description = $"Test task has Id = {i:000}",
                StartDateUtc = startDateTimeUtc.AddDays(i).AddHours(i + 5),
                FinishDateUtc = finishDateTimeUtc.AddDays(i).AddHours(i + 5)
            };

            // subtasks
            int parentId = i++;
            for (; i < parentId + 5; i++)
            {
                yield return new TaskItem
                {
                    Id = i,
                    Name = $"Subtask {i:000}",
                    Description = $"Test subtask has Id = {i:000}, parent Id = {i:000}",
                    StartDateUtc = startDateTimeUtc.AddDays(i - parentId).AddHours(i + 6),
                    FinishDateUtc = finishDateTimeUtc.AddDays(i - parentId).AddHours(i + 6),
                    ParentId = parentId
                };
            }

            // in-progress single tasks
            int startId = i;
            for (; i < startId + 10; i++)
            {
                yield return new TaskItem
                {
                    Id = i,
                    Name = $"Task {i:000} ({TaskItemState.InProgress.Name()})",
                    Description = $"Test task has Id = {i:000}",
                    StartDateUtc = startDateTimeUtc.AddDays(i - startId),
                    FinishDateUtc = finishDateTimeUtc.AddDays(i - startId),
                    State = TaskItemState.InProgress
                };
            }
        }

        #region SINGLE TASK

        public string SingleTaskName
        {
            get => "Code review meeting";
        }

        public string SingleTaskDescription
        {
            get => "Discussing and reviewing the code";
        }

        public DateTime SingleTaskStartDateUtc
        {
            get
            {
                DateTime dateTimeUtc = DateTime.UtcNow;
                DateTime dateTimeUtcEvent = new DateTime(dateTimeUtc.Year, dateTimeUtc.Month, dateTimeUtc.Day, 15, 0, 0, DateTimeKind.Utc);
                return dateTimeUtcEvent;
            }
        }

        public DateTime SingleTaskFinishDateUtc
        {
            get => this.SingleTaskStartDateUtc.AddHours(1);
        }

        public TaskItem GetSingleTaskItem()
        {
            return new TaskItem
            {
                Name = this.SingleTaskName,
                Description = this.SingleTaskDescription,
                StartDateUtc = this.SingleTaskStartDateUtc,
                FinishDateUtc = this.SingleTaskFinishDateUtc
            };
        }

        public TaskItemDto GetSingleTaskItemDto()
        {
            return new TaskItemDto
            {
                Name = this.SingleTaskName,
                Description = this.SingleTaskDescription,
                StartDateUtc = this.SingleTaskStartDateUtc,
                FinishDateUtc = this.SingleTaskFinishDateUtc,
                Subtasks = new List<SubtaskDto>()
            };
        }

        #endregion SINGLE TASK
    }
}
