using System;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

using NUnit.Framework;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Moq;

using Tms.Api.Controllers;
using Tms.Data.Domain;
using Tms.Logger;
using Tms.Service;
using Tms.Web;
using Tms.Dto;
using Tms.Data.Context;
using Tms.Data.Demo;
using Tms.Data.Repository;
using Tms.Test.Extensions;
using System.Text;
using System.IO;

namespace Tms.Test.Api
{
    [TestFixture]
    public class TestReportController
    {
        private readonly ITmsLogger _logger;
        //private readonly ITaskItemService _taskItemService;
        private readonly TestClient _client;
        private readonly IEnumerable<TaskItem> _demoTaskItems;
        private readonly DemoData _demoData;

        public TestReportController()
        {
            // demo data
            _demoData = new DemoData();
            _demoTaskItems = _demoData.GetTaskItems();

            // create test client
            _client = new TestClient();
            _client.SetupContext();
            _client.SetupRepository(_demoTaskItems);

            // services
            _logger = new TmsLogger();
            //_taskItemService = new TaskItemService(_logger, _client.TaskItemRepository.Object);
        }

        [Test, Theory]
        [TestCase(WebRequestMethods.Http.Get, ApiList.API_REPORT_GET)]
        public async Task ShouldReturn_CsvReport_AsMemoryStream(string httpMethod, string apiRoute)
        {
            // data
            DateTime startDateTimeUtc = new DateTime(2021, 1, 1, 10, 0, 0, DateTimeKind.Utc);
            DateTime finishDateTimeUtc = new DateTime(2021, 12, 1, 10, 0, 0, DateTimeKind.Utc);

            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_REPORT}{apiRoute}?dateFrom={startDateTimeUtc}&dateTo={finishDateTimeUtc}");
            //request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            //{
            //    { "dateFrom", startDateTimeUtc.ToString() },
            //    { "dateTo", finishDateTimeUtc.ToString() }
            //});
            var response = await _client.SendAsync(request);

            // assert
            //string msg = response.GetErrorMessage();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // json
            var json = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(json);
            Assert.Positive(json.Length);

            try
            {
                //if (json.Length != 0)
                //{
                //    string filePath = "file.csv";
                //    //await File.WriteAllTextAsync(filePath, json);
                //    //using (MemoryStream stream = new MemoryStream(json.ToCharArray()))
                //    //{
                //    //    File.WriteAllTextAsync(filePath, json);
                //    //}
                //}

                string filePath = "file.csv";
                byte[] buffer = JsonConvert.DeserializeObject<byte[]>(json);
                await File.WriteAllBytesAsync(filePath, buffer);

                //using (MemoryStream stream = new MemoryStream(buffer))
                //using (FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                //{
                //    //byte[] bytes = new byte[buffer.Length];
                //    //buffer.ReadAsync(bytes, 0, (int)buffer.Length);

                //    //await file.WriteAsync(buffer, 0, buffer.Length);
                //    //await file.FlushAsync();
                //}
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            // assert
            //Assert.IsNotNull(taskItemRootDto);
            //Assert.IsNotEmpty(taskItemRootDto.Values);
            //Assert.AreEqual(_demoTaskItems.Where(x => x.ParentId == null).Count(), taskItemRootDto.Values.Count);
            //Assert.AreEqual(_demoTaskItems.Where(x => x.ParentId != null).Count(), taskItemRootDto.Values.SelectMany(x => x.Subtasks).Count());
        }

    }
}
