using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Tms.Data.Domain;
using Tms.Logger;
using Tms.Data.Repository;
using Tms.Dto.Extensions;
using Tms.Enum;

namespace Tms.Service
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITmsLogger _logger;
        private readonly IRepository<TaskItem> _taskItemRepository;

        public TaskItemService(
            ITmsLogger logger,
            IRepository<TaskItem> taskItemRepository
            )
        {
            _logger = logger;
            _taskItemRepository = taskItemRepository;
        }

        public async Task<TaskItem> GetAsync(int entityId)
        {
            try
            {
                TaskItem entity = await _taskItemRepository.GetAsync(entityId);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        public async Task<List<TaskItem>> GetAsync()
        {
            try
            {
                var entities = await _taskItemRepository.Entities.ToListAsync();
                return entities;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new List<TaskItem>();
            }
        }

        public async Task<List<TaskItem>> GetTasksOnlyAsync()
        {
            try
            {
                var entities = await _taskItemRepository.Entities.Where(x => x.ParentId == null).ToListAsync();
                return entities;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new List<TaskItem>();
            }
        }

        public async Task<TaskItem> CreateAsync(TaskItem entity)
        {
            if (entity == null)
                throw new Exception(DtoExtensions.GetErrorMessage_IsNull(nameof(entity)));

            // create entity
            entity = await _taskItemRepository.CreateAsync(entity);

            // update state
            await this.UpdateParentStateAsync(entity);

            return entity;
        }

        public async Task UpdateAsync(TaskItem entity)
        {
            if (entity == null)
                throw new Exception(DtoExtensions.GetErrorMessage_IsNull(nameof(entity)));

            // update entity
            await _taskItemRepository.UpdateAsync(entity);

            // update state
            await this.UpdateParentStateAsync(entity);
        }

        public async Task DeleteAsync(TaskItem entity)
        {
            if (entity == null)
                throw new Exception(DtoExtensions.GetErrorMessage_IsNull(nameof(entity)));

            // delete subtasks
            if (entity.HasSubtasks)
                await _taskItemRepository.DeleteAsync(entity.Subtasks.ToList());

            // delete entity
            int? parentId = entity.ParentId;
            await _taskItemRepository.DeleteAsync(entity);

            // update state
            await this.UpdateParentStateAsync(parentId);
        }

        #region UPDATE PARENT STATE

        public async Task UpdateParentStateAsync(int? parentId)
        {
            if (parentId == null)
                return;

            TaskItem taskItem = await _taskItemRepository.GetAsync(parentId.Value);
            if (taskItem == null)
            {
                _logger.Error(null, DtoExtensions.GetErrorMessage_NotFound(nameof(taskItem), parentId));
                return;
            }

            await this.UpdateParentStateAsync(taskItem);
        }

        public async Task UpdateParentStateAsync(TaskItem taskItem)
        {
            TaskItem taskItemForUpdate = null;
            if (taskItem.HasSubtasks)
                taskItemForUpdate = taskItem;
            else if (taskItem.HasParent)
                taskItemForUpdate = taskItem.Parent;
            else
                return;

            // update state for parent if changed
            TaskItemState newState = this.CalculateTaskState(taskItemForUpdate);
            await this.UpdateTaskState_IfChangedAsync(taskItemForUpdate, newState);
        }

        private TaskItemState CalculateTaskState(TaskItem taskItem)
        {
            if (taskItem == null)
            {
                _logger.Error(null, DtoExtensions.GetErrorMessage_IsNull(nameof(taskItem)));
                return taskItem.State;
            }

            if (!taskItem.HasSubtasks)
            {
                _logger.Error(null, $"Expected Subtasks for {taskItem.Info}");
                return taskItem.State;
            }

            TaskItemState taskItemState;

            // calc new state
            bool isCompleted = !taskItem.Subtasks.Where(x => x.State != TaskItemState.Completed).Any();
            bool isInProgress = taskItem.Subtasks.Where(x => x.State == TaskItemState.InProgress).Any();

            if (isCompleted)
                taskItemState = TaskItemState.Completed;
            else if (isInProgress)
                taskItemState = TaskItemState.InProgress;
            else
                taskItemState = TaskItemState.Planned;

            return taskItemState;
        }

        private async Task UpdateTaskState_IfChangedAsync(TaskItem taskItem, TaskItemState newState)
        {
            if (newState == taskItem.State)
                return;

            taskItem.State = newState;
            await _taskItemRepository.UpdateAsync(taskItem);
        }

        #endregion UPDATE PARENT STATE
    }
}
