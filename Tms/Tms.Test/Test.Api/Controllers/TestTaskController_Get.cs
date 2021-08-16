using System.Net;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using NUnit.Framework;
using Newtonsoft.Json;

using Tms.Data.Domain;
using Tms.Dto;
using Tms.Data.Demo;
using Tms.Test.Extensions;

namespace Tms.Test.Api
{
    [TestFixture]
    public class TestTaskController_Get
    {
        //private readonly ITmsLogger _logger;
        //private readonly ITaskItemService _taskItemService;
        private readonly TestClient _client;
        private readonly IEnumerable<TaskItem> _demoTaskItems;
        private readonly DemoData _demoData;

        public TestTaskController_Get()
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
        [TestCase(WebRequestMethods.Http.Get, ApiList.API_TASK_GET)]
        public async Task ShouldReturn_AllParentTasksItems_IncludeSubtasksAsChilds(string httpMethod, string apiRoute)
        {
            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK}{apiRoute}");
            var response = await _client.SendAsync(request);

            // assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, response.GetErrorMessage());

            // json
            var json = await response.Content.ReadAsStringAsync();
            TaskItemRootDto taskItemRootDto = JsonConvert.DeserializeObject<TaskItemRootDto>(json);

            // assert
            Assert.IsNotNull(taskItemRootDto);
            Assert.IsNotEmpty(taskItemRootDto.Values);
            Assert.AreEqual(_demoTaskItems.Where(x => x.ParentId == null).Count(), taskItemRootDto.Values.Count);
            Assert.AreEqual(_demoTaskItems.Where(x => x.ParentId != null).Count(), taskItemRootDto.Values.SelectMany(x => x.Subtasks).Count());
        }

        [Test, Theory]
        [TestCase(WebRequestMethods.Http.Get, 1)]
        [TestCase(WebRequestMethods.Http.Get, 6)]
        public async Task ShouldReturn_Single_TaskItem(string httpMethod, int id)
        {
            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK}{id}");
            var response = await _client.SendAsync(request);

            // assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, response.GetErrorMessage());

            // json
            var json = await response.Content.ReadAsStringAsync();
            TaskItemRootDto taskItemRootDto = JsonConvert.DeserializeObject<TaskItemRootDto>(json);

            // assert
            Assert.IsNotNull(taskItemRootDto);
            Assert.IsNotEmpty(taskItemRootDto.Values);
            Assert.AreEqual(1, taskItemRootDto.Values.Count);

            // get first value
            TaskItemDto taskItemDto = taskItemRootDto.Values.FirstOrDefault();

            Assert.IsNotNull(taskItemDto);
            Assert.AreEqual(_demoTaskItems.FirstOrDefault(x => x.Id == id)?.Name, taskItemDto.Name);
            Assert.AreEqual(_demoTaskItems.Where(x => x.ParentId == id).Count(), taskItemDto.Subtasks.Count());
        }

        [Test, Theory]
        [TestCase(WebRequestMethods.Http.Get, 0)]
        [TestCase(WebRequestMethods.Http.Get, int.MaxValue)]
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