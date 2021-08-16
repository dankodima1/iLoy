using System;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using NUnit.Framework;
using Newtonsoft.Json;

using Tms.Data.Domain;
using Tms.Dto;
using Tms.Dto.Extensions;
using Tms.Data.Demo;
using Tms.Test.Extensions;

namespace Tms.Test.Api
{
    [TestFixture]
    public class TestTaskController_Create
    {
        //private readonly ITmsLogger _logger;
        //private readonly ITaskItemService _taskItemService;
        private readonly TestClient _client;
        private readonly IEnumerable<TaskItem> _demoTaskItems;
        private readonly DemoData _demoData;

        public TestTaskController_Create()
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
        [TestCase(WebRequestMethods.Http.Post)]
        public async Task ShouldCreate_And_Return_NewSingleTaskItem(string httpMethod)
        {
            // data
            TaskItemDto taskItemDto_Src = _demoData.GetSingleTaskItemDto();

            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK_CREATE}");
            var json = JsonConvert.SerializeObject(taskItemDto_Src);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);

            // assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, response.GetErrorMessage());

            // json
            json = await response.Content.ReadAsStringAsync();
            TaskItemRootDto taskItemRootDto = JsonConvert.DeserializeObject<TaskItemRootDto>(json);

            // assert
            Assert.IsNotNull(taskItemRootDto);
            Assert.IsNotEmpty(taskItemRootDto.Values);
            Assert.AreEqual(1, taskItemRootDto.Values.Count);

            // get first value
            TaskItemDto taskItemDto = taskItemRootDto.Values.FirstOrDefault();

            // assert
            Assert.IsNotNull(taskItemDto);
            Assert.Positive(taskItemDto.Id);
            Assert.AreEqual(taskItemDto_Src.Name, taskItemDto.Name);
            Assert.AreEqual(taskItemDto_Src.Description, taskItemDto.Description);
            Assert.AreEqual(taskItemDto_Src.StartDateUtc, taskItemDto.StartDateUtc);
            Assert.AreEqual(taskItemDto_Src.FinishDateUtc, taskItemDto.FinishDateUtc);
            Assert.AreEqual(taskItemDto_Src.Subtasks.Count(), taskItemDto.Subtasks.Count());
        }

        [Test, Theory]
        [TestCase(WebRequestMethods.Http.Post)]
        public async Task ShouldReturn_BadRequest_TaskItemIsNull(string httpMethod)
        {
            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK_CREATE}");
            var json = JsonConvert.SerializeObject(null);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);

            // assert
            //string msg = response.GetErrorMessage();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, response.GetErrorMessage());
        }

        [Test, Theory]
        [TestCase(WebRequestMethods.Http.Post)]
        public async Task ShouldReturn_BadRequest_IdIsNotZero(string httpMethod)
        {
            // data
            TaskItemDto taskItemDto_Src = _demoData.GetSingleTaskItemDto();
            taskItemDto_Src.Id = 1;

            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK_CREATE}");
            var json = JsonConvert.SerializeObject(taskItemDto_Src);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);

            // assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, response.GetErrorMessage());
        }

        [Test, Theory]
        [TestCase(WebRequestMethods.Http.Post)]
        public async Task ShouldReturn_BadRequest_NameIsNull(string httpMethod)
        {
            // data
            TaskItemDto taskItemDto_Src = _demoData.GetSingleTaskItemDto();
            taskItemDto_Src.Name = String.Empty;

            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK_CREATE}");
            var json = JsonConvert.SerializeObject(taskItemDto_Src);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);

            // assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, response.GetErrorMessage());
        }

        [Test, Theory]
        [TestCase(WebRequestMethods.Http.Post)]
        public async Task ShouldReturn_BadRequest_NameIsExceed(string httpMethod)
        {
            // data
            TaskItemDto taskItemDto_Src = _demoData.GetSingleTaskItemDto();
            taskItemDto_Src.Name = new string('x', DtoExtensions.TaskItem_Name_MaxLength + 1);

            // test
            //taskItemDto_Src.Id = 1;
            //taskItemDto_Src.Name = new string('x', DtoExtensions.TaskItem_Name_MaxLength + 1);
            //taskItemDto_Src.Description = new string('x', DtoExtensions.TaskItem_Description_MaxLength + 1);

            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK_CREATE}");
            var json = JsonConvert.SerializeObject(taskItemDto_Src);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);

            // assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, response.GetErrorMessage());
        }

        [Test, Theory]
        [TestCase(WebRequestMethods.Http.Post)]
        public async Task ShouldReturn_BadRequest_DescriptionIsExceed(string httpMethod)
        {
            // data
            TaskItemDto taskItemDto_Src = _demoData.GetSingleTaskItemDto();
            taskItemDto_Src.Description = new string('x', DtoExtensions.TaskItem_Description_MaxLength + 1);

            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK_CREATE}");
            var json = JsonConvert.SerializeObject(taskItemDto_Src);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);

            // assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, response.GetErrorMessage());
        }
    }
}