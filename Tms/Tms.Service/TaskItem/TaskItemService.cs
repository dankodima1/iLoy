using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Tms.Data.Domain;
using Tms.Logger;
using Tms.Data.Repository;
using Tms.Dto.Extensions;

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

            entity = await _taskItemRepository.CreateAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(TaskItem entity)
        {
            if (entity == null)
                throw new Exception(DtoExtensions.GetErrorMessage_IsNull(nameof(entity)));

            await _taskItemRepository.UpdateAsync(entity);
        }

        //public async Task DeleteAsync(int entityId)
        //{
        //    var entity = await _taskItemRepository.GetAsync(entityId);
        //    if (entity == null)
        //        throw new Exception($"{nameof(TaskItem)} (Id = {entityId}) not found");

        //    if (entity.Subtasks.Any())
        //        await _taskItemRepository.DeleteAsync(entity.Subtasks.ToList());

        //    await _taskItemRepository.DeleteAsync(entity);
        //}

        public async Task DeleteAsync(TaskItem entity)
        {
            if (entity == null)
                throw new Exception(DtoExtensions.GetErrorMessage_IsNull(nameof(entity)));
            if (entity == null)
                throw new Exception(DtoExtensions.GetErrorMessage_IsNull(nameof(entity)));

            if (entity.Subtasks.Any())
                await _taskItemRepository.DeleteAsync(entity.Subtasks.ToList());

            await _taskItemRepository.DeleteAsync(entity);
        }
    }
}
