using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tms.Api.Mapping;
using Tms.Api.Extensions;
using Tms.Data.Domain;
using Tms.Dto;
using Tms.Logger;
using Tms.Service;
using Tms.Dto.Extensions;

namespace Tms.Api.Controllers
{
    /// <summary>
    /// controller for CRUD with TaskItem
    /// </summary>
    [ApiController]
    [Route(ApiList.API_TEMPLATE)]
    [AllowAnonymous]
    public class TaskController : ControllerBase
    {
        private readonly ITmsLogger _logger;
        private readonly ITaskItemService _taskItemService;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="taskItemService"></param>
        public TaskController(
            ITmsLogger logger,
            ITaskItemService taskItemService
            )
        {
            _logger = logger;
            _taskItemService = taskItemService;
        }

        /// <summary>
        /// Get all TaskItems
        /// </summary>
        /// <returns>TaskItemRootDto</returns>
        [HttpGet(ApiList.API_TASK_GET)]
        [ProducesResponseType(typeof(TaskItemRootDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTasksAsync()
        {
            try
            {
                _logger.LogRequest(this.Request);

                // get
                List<TaskItem> taskItems = await _taskItemService.GetTasksOnlyAsync();

                // to dto
                List<TaskItemDto> taskItemDtos = taskItems.ToDto().ToList();
                TaskItemRootDto caseRootObject = new TaskItemRootDto { Values = taskItemDtos };

                return Ok(caseRootObject);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ex.GetFullMessage());
            }
        }

        /// <summary>
        /// Get TaskItem by Id
        /// </summary>
        /// <returns>TaskItemRootDto</returns>
        [HttpGet(ApiList.API_TASK_GET_BY_ID)]
        [ProducesResponseType(typeof(TaskItemRootDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTaskAsync(int id)
        {
            try
            {
                _logger.LogRequest(this.Request);

                // get
                TaskItem taskItem = await _taskItemService.GetAsync(id);
                if (taskItem == null)
                {
                    ModelState.AddModelError(nameof(TaskItem), DtoExtensions.GetErrorMessage_NotFound(nameof(TaskItem), id));
                    string msg = ModelState.GetErrors();
                    _logger.Error(null, msg);
                    return NotFound(msg);
                }

                // to dto
                List<TaskItemDto> taskItemDtos = (taskItem == null) ? new List<TaskItemDto>() : new List<TaskItemDto> { taskItem.ToDto() };
                TaskItemRootDto caseRootObject = new TaskItemRootDto { Values = taskItemDtos };

                return Ok(caseRootObject);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ex.GetFullMessage());
            }
        }

        /// <summary>
        /// Create TaskItem
        /// </summary>
        /// <returns>TaskItemRootDto</returns>
        [HttpPost(ApiList.API_TASK_CREATE)]
        [ProducesResponseType(typeof(TaskItemRootDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CreateTaskAsync(TaskItemDto taskItemDto)
        {
            try
            {
                _logger.LogRequest(this.Request);

                if (taskItemDto == null)
                    ModelState.AddModelError(nameof(TaskItemDto), DtoExtensions.GetErrorMessage_IsNull(nameof(taskItemDto)));

                if (taskItemDto.Id != 0)
                    ModelState.AddModelError(nameof(taskItemDto.Id), DtoExtensions.GetErrorMessage_ShouldBe(nameof(taskItemDto.Id), nameof(TaskItemDto), 0));

                if (!ModelState.IsValid)
                {
                    string msg = ModelState.GetErrors();
                    _logger.Error(null, msg);
                    return BadRequest(msg);
                }

                // to ent
                TaskItem taskItem = taskItemDto.ToEnt();

                // create
                await _taskItemService.CreateAsync(taskItem);

                // to dto
                List<TaskItemDto> taskItemDtos = new List<TaskItemDto> { taskItem.ToDto() };
                TaskItemRootDto caseRootObject = new TaskItemRootDto { Values = taskItemDtos };

                return Ok(caseRootObject);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ex.GetFullMessage());
            }
        }

        /// <summary>
        /// Update TaskItem
        /// </summary>
        /// <returns>TaskItemRootDto</returns>
        [HttpPost(ApiList.API_TASK_UPDATE)]
        [ProducesResponseType(typeof(TaskItemRootDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateTaskAsync(TaskItemDto taskItemDto)
        {
            try
            {
                _logger.LogRequest(this.Request);

                if (taskItemDto == null)
                    ModelState.AddModelError(nameof(TaskItemDto), DtoExtensions.GetErrorMessage_IsNull(nameof(taskItemDto)));

                if (taskItemDto.Id == 0)
                    ModelState.AddModelError(nameof(taskItemDto.Id), DtoExtensions.GetErrorMessage_ShouldBe(nameof(taskItemDto.Id), nameof(TaskItemDto), $"greater then 0"));

                if (!ModelState.IsValid)
                {
                    string msg = ModelState.GetErrors();
                    _logger.Error(null, msg);
                    return BadRequest(msg);
                }

                // get
                TaskItem taskItem = await _taskItemService.GetAsync(taskItemDto.Id);
                if (taskItem == null)
                {
                    ModelState.AddModelError(nameof(TaskItem), DtoExtensions.GetErrorMessage_NotFound(nameof(TaskItem), taskItemDto.Id));
                    string msg = ModelState.GetErrors();
                    _logger.Error(null, msg);
                    return NotFound(msg);
                }

                // to ent
                taskItemDto.ToEnt(taskItem);

                // update
                await _taskItemService.UpdateAsync(taskItem);

                // to dto
                List<TaskItemDto> taskItemDtos = new List<TaskItemDto> { taskItem.ToDto() };
                TaskItemRootDto caseRootObject = new TaskItemRootDto { Values = taskItemDtos };

                return Ok(caseRootObject);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ex.GetFullMessage());
            }
        }

        /// <summary>
        /// Delete TaskItem by Id
        /// </summary>
        /// <returns></returns>
        [HttpDelete(ApiList.API_TASK_DELETE)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteTaskAsync(int id)
        {
            try
            {
                _logger.LogRequest(this.Request);

                // get
                TaskItem taskItem = await _taskItemService.GetAsync(id);
                if (taskItem == null)
                {
                    ModelState.AddModelError(nameof(TaskItem), DtoExtensions.GetErrorMessage_NotFound(nameof(TaskItem), id));
                    string msg = ModelState.GetErrors();
                    _logger.Error(null, msg);
                    return NotFound(msg);
                }

                // delete
                await _taskItemService.DeleteAsync(taskItem);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ex.GetFullMessage());
            }
        }

    }
}
