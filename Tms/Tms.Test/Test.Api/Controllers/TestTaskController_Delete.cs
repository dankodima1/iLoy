using System;
using System.Text;
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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Moq;

using Tms.Api.Controllers;
using Tms.Api.Mapping;
using Tms.Data.Domain;
using Tms.Data.Context;
using Tms.Data.Demo;
using Tms.Data.Repository;
using Tms.Logger;
using Tms.Service;
using Tms.Web;
using Tms.Dto;
using Tms.Test.Extensions;

namespace Tms.Test.Api
{
    [TestFixture]
    public class TestTaskController_Delete
    {
        private const string HttpDelete = "DELETE";
        //private readonly ITmsLogger _logger;
        //private readonly ITaskItemService _taskItemService;
        private readonly TestClient _client;
        private readonly IEnumerable<TaskItem> _demoTaskItems;
        private readonly DemoData _demoData;

        public TestTaskController_Delete()
        {
            // demo data
            _demoData = new DemoData();
            _demoTaskItems = _demoData.GetTaskItems();

            // create test client
            _client = new TestClient();
            _client.SetupContext();
            _client.SetupRepository(_demoTaskItems);

            // services
            //_logger = new TmsLogger();
            //_taskItemService = new TaskItemService(_logger, _client.TaskItemRepository.Object);
        }

        [Test, Theory]
        [TestCase(TestTaskController_Delete.HttpDelete, 1)]
        public async Task ShouldDelete_SingleTaskItem(string httpMethod, int id)
        {
            // act 1
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK}{id}");
            var response = await _client.SendAsync(request);

            // assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, response.GetErrorMessage());

            // act 2
            request = new HttpRequestMessage(new HttpMethod(WebRequestMethods.Http.Get), $"{ApiList.CLIENT_API_TASK}{id}");
            response = await _client.SendAsync(request);

            // assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, response.GetErrorMessage());
        }

        [Test, Theory]
        [TestCase(TestTaskController_Delete.HttpDelete, 0)]
        [TestCase(TestTaskController_Delete.HttpDelete, int.MaxValue)]
        public async Task ShouldReturn_NotFound_TaskItem(string httpMethod, int id)
        {
            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK}{id}");
            var response = await _client.SendAsync(request);

            // assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, response.GetErrorMessage());
        }
    }
}