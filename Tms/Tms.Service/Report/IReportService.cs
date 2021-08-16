using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tms.Data.Domain;
using System.IO;

namespace Tms.Service
{
    public interface IReportService
    {
        IQueryable<TaskItem> GetInProgressTasks_ByDates(DateTime dateFrom, DateTime dateTo);
        Task<byte[]> GetReportByDatesAsync(DateTime dateFrom, DateTime dateTo);
    }
}
