using System;
using System.IO;
using System.Threading.Tasks;
using System.Net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tms.Api.Extensions;
using Tms.Data.Domain;
using Tms.Dto;
using Tms.Logger;
using Tms.Service;
using Tms.Dto.Extensions;

namespace Tms.Api.Controllers
{
    /// <summary>
    /// controller for generating reports
    /// </summary>
    [ApiController]
    [Route(ApiList.API_TEMPLATE)]
    [AllowAnonymous]
    public class ReportController : ControllerBase
    {
        private readonly ITmsLogger _logger;
        private readonly ITaskItemService _taskItemService;
        private readonly IReportService _reportService;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="taskItemService"></param>
        /// <param name="reportService"></param>
        public ReportController(
            ITmsLogger logger,
            ITaskItemService taskItemService,
            IReportService reportService
            )
        {
            _logger = logger;
            _taskItemService = taskItemService;
            _reportService = reportService;
        }

        /// <summary>
        /// Get report
        /// </summary>
        /// <returns>report stream</returns>
        [HttpGet(ApiList.API_REPORT_GET)]
        [ProducesResponseType(typeof(MemoryStream), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetReportAsync(DateTime? dateFrom, DateTime? dateTo)
        {
            try
            {
                _logger.LogRequest(this.Request);

                if (dateFrom == null)
                {
                    ModelState.AddModelError(nameof(TaskItem), DtoExtensions.GetErrorMessage_IsNull(nameof(dateFrom)));
                    string msg = ModelState.GetErrors();
                    _logger.Error(null, msg);
                    return NotFound(msg);
                }

                if (dateTo == null)
                {
                    ModelState.AddModelError(nameof(TaskItem), DtoExtensions.GetErrorMessage_IsNull(nameof(dateTo)));
                    string msg = ModelState.GetErrors();
                    _logger.Error(null, msg);
                    return NotFound(msg);
                }

                // generate report
                var buffer = await _reportService.GetReportByDatesAsync(dateFrom.Value, dateTo.Value);
                return Ok(buffer);
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
