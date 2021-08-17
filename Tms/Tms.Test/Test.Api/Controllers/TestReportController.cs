using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using NUnit.Framework;
using Newtonsoft.Json;

using Tms.Data.Domain;
using Tms.Logger;
using Tms.Dto;
using Tms.Data.Demo;
using Tms.Test.Extensions;

namespace Tms.Test.Api
{
    [TestFixture]
    public class TestReportController
    {
        private readonly ITmsLogger _logger;
        private readonly TestClient _client;
        private readonly DemoData _demoData;
        private readonly IEnumerable<TaskItem> _demoTaskItems;

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
        }

        [Test, Theory]
        [TestCase(WebRequestMethods.Http.Get, ApiList.API_REPORT_GET)]
        public async Task ShouldReturn_CsvReport_And_SaveToFile(string httpMethod, string apiRoute)
        {
            // data
            DateTime startDateTimeUtc = new DateTime(2021, 1, 1, 10, 0, 0, DateTimeKind.Utc);
            DateTime finishDateTimeUtc = new DateTime(2021, 12, 1, 10, 0, 0, DateTimeKind.Utc);

            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_REPORT}{apiRoute}?dateFrom={startDateTimeUtc}&dateTo={finishDateTimeUtc}");
            var response = await _client.SendAsync(request);

            // assert
            //string msg = response.GetErrorMessage();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // json
            var json = await response.Content.ReadAsStringAsync();

            // assert
            Assert.IsNotNull(json);
            Assert.Positive(json.Length);

            // get buffer
            byte[] buffer = JsonConvert.DeserializeObject<byte[]>(json);

            // assert
            Assert.IsNotNull(buffer);
            Assert.Positive(buffer.Length);

            // save to file
            string filePath = "file.csv";
            string[] lines = null;
            try
            {
                await File.WriteAllBytesAsync(filePath, buffer);
                lines = await File.ReadAllLinesAsync(filePath);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            // assert
            Assert.IsNotNull(lines);
            Assert.IsNotEmpty(lines);
            Assert.AreEqual(21, lines.Count());
        }

    }
}
