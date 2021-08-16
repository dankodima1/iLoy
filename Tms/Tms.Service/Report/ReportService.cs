using System;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using CsvHelper;
using CsvHelper.Configuration;

using Tms.Data.Domain;
using Tms.Enum;
using Tms.Logger;
using Tms.Data.Repository;
using Tms.Dto.Extensions;
using Tms.Enum.Extensions;

namespace Tms.Service
{
    public class ReportService : IReportService
    {
        private readonly ITmsLogger _logger;
        private readonly IRepository<TaskItem> _taskItemRepository;
        private const string CsvDelimiter = ",";

        public ReportService(
            ITmsLogger logger,
            IRepository<TaskItem> taskItemRepository
            )
        {
            _logger = logger;
            _taskItemRepository = taskItemRepository;
        }

        public IQueryable<TaskItem> GetInProgressTasks_ByDates(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                var query = from t in _taskItemRepository.Entities.AsQueryable()
                            where (t.State == TaskItemState.InProgress)
                                && (t.StartDateUtc >= dateFrom && t.StartDateUtc <= dateTo)
                            select t;

                return query;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        public async Task<byte[]> GetReportByDatesAsync(DateTime dateFrom, DateTime dateTo)
        {
            // get records
            var taskItems = await this.GetInProgressTasks_ByDates(dateFrom, dateTo).ToListAsync();
            //var taskItems = await _taskItemRepository.Entities.ToListAsync();

            // csv config
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                Delimiter = ReportService.CsvDelimiter
            };

            // write csv
            //byte[] buffer = null;
            //using (var stream = new MemoryStream())
            //using (var writer = new StreamWriter(stream))
            //using (var csvWriter = new CsvWriter(writer, csvConfig))
            //{
            //    this.WriteCaption(csvWriter, dateFrom, dateTo);
            //    //this.WriteData(csvWriter, taskItems);
            //    //await csvWriter.WriteRecordsAsync(taskItems);

            //    stream.Position = 0;
            //    buffer = stream.GetBuffer();
            //}

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(streamWriter, csvConfig))
                {
                    this.WriteCaption(csvWriter, dateFrom, dateTo);
                    this.WriteData(csvWriter, taskItems);
                    //await csvWriter.WriteRecordsAsync(taskItems);
                }
                return memoryStream.ToArray();
            }
        }

        private void WriteCaption(CsvWriter csvWriter, DateTime dateFrom, DateTime dateTo)
        {
            csvWriter.WriteField($"Report");
            csvWriter.NextRecord();
            csvWriter.WriteField($"Tasks with the state:");
            csvWriter.WriteField(TaskItemState.InProgress.Name());
            csvWriter.NextRecord();
            csvWriter.WriteField($"Start date by UTC:");
            csvWriter.WriteField($"{dateFrom}");
            csvWriter.WriteField($"{dateTo}");
            csvWriter.NextRecord();
            csvWriter.WriteField("Name");
            csvWriter.WriteField("Subtask name");
            csvWriter.WriteField("Description");
            csvWriter.WriteField("StartDateUtc");
            csvWriter.WriteField("FinishDateUtc");
            csvWriter.WriteField("Subtask state");
            //csvWriter.WriteField("Subtasks");
            csvWriter.NextRecord();
        }

        private void WriteData(CsvWriter csvWriter, List<TaskItem> taskItems)
        {
            foreach (var taskItem in taskItems)
            {
                csvWriter.WriteField($"{taskItem.Name}");
                csvWriter.WriteField(string.Empty);
                csvWriter.WriteField($"{taskItem.Description}");
                csvWriter.WriteField($"{taskItem.StartDateUtc}");
                csvWriter.WriteField($"{taskItem.FinishDateUtc}");
                //csvWriter.WriteField($"{taskItem.State.Name()}");
                //csvWriter.WriteField($"{taskItem.Subtasks}");
                csvWriter.NextRecord();

                WriteSubtasks(csvWriter, taskItem.Subtasks.ToList());
            }
        }

        private void WriteSubtasks(CsvWriter csvWriter, List<TaskItem> subtasks)
        {
            foreach (var subtask in subtasks)
            {
                csvWriter.WriteField(string.Empty);
                csvWriter.WriteField($"{subtask.Name}");
                csvWriter.WriteField($"{subtask.Description}");
                csvWriter.WriteField($"{subtask.StartDateUtc}");
                csvWriter.WriteField($"{subtask.FinishDateUtc}");
                csvWriter.WriteField($"{subtask.State.Name()}");
                csvWriter.NextRecord();
            }
        }
    }
}
