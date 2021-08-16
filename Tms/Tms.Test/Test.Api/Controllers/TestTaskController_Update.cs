using System;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using NUnit.Framework;
using Newtonsoft.Json;

using Tms.Api.Mapping;
using Tms.Data.Domain;
using Tms.Data.Demo;
using Tms.Dto;
using Tms.Dto.Extensions;
using Tms.Test.Extensions;
using Tms.Enum;

namespace Tms.Test.Api
{
    [TestFixture]
    public class TestTaskController_Update
    {
        //private readonly ITmsLogger _logger;
        //private readonly ITaskItemService _taskItemService;
        private readonly TestClient _client;
        private readonly IEnumerable<TaskItem> _demoTaskItems;
        private readonly DemoData _demoData;

        public TestTaskController_Update()
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
        public async Task ShouldUpdate_And_Return_ChangedSingleTaskItem(string httpMethod)
        {
            // data
            TaskItemDto taskItemDto_Src = _demoData.GetSingleTaskItemDto();
            TaskItemDto taskItemDto_Dest = _demoTaskItems.FirstOrDefault(x => x.Id == 6).ToDto();
            taskItemDto_Dest = taskItemDto_Src.CopyTo(taskItemDto_Dest);

            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK_UPDATE}");
            var json = JsonConvert.SerializeObject(taskItemDto_Dest);
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
            Assert.AreEqual(taskItemDto_Dest.Id, taskItemDto.Id);
            Assert.AreEqual(taskItemDto_Src.Name, taskItemDto.Name);
            Assert.AreEqual(taskItemDto_Src.Description, taskItemDto.Description);
            Assert.AreEqual(taskItemDto_Src.StartDateUtc, taskItemDto.StartDateUtc);
            Assert.AreEqual(taskItemDto_Src.FinishDateUtc, taskItemDto.FinishDateUtc);
        }

        [Test, Theory]
        [TestCase(WebRequestMethods.Http.Post)]
        public async Task ShouldUpdate_Status_And_Return_Subtask(string httpMethod)
        {
            // data
            TaskItem taskItem_Dest = _demoTaskItems.FirstOrDefault(x => x.Id == 23);
            TaskItemDto taskItemDto_Dest = taskItem_Dest.ToDto();
            taskItemDto_Dest.State = TaskItemState.Completed;

            // act 1
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK_UPDATE}");
            var json = JsonConvert.SerializeObject(taskItemDto_Dest);
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
            Assert.AreEqual(taskItemDto_Dest.Id, taskItemDto.Id);
            Assert.AreEqual(taskItemDto_Dest.State, taskItemDto.State);

            // act 2
            request = new HttpRequestMessage(new HttpMethod(WebRequestMethods.Http.Get), $"{ApiList.CLIENT_API_TASK}{taskItem_Dest.ParentId}");
            response = await _client.SendAsync(request);

            // assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, response.GetErrorMessage());

            // json
            json = await response.Content.ReadAsStringAsync();
            taskItemRootDto = JsonConvert.DeserializeObject<TaskItemRootDto>(json);

            // assert
            Assert.IsNotNull(taskItemRootDto);
            Assert.IsNotEmpty(taskItemRootDto.Values);
            Assert.AreEqual(1, taskItemRootDto.Values.Count);

            // get first value
            taskItemDto = taskItemRootDto.Values.FirstOrDefault();

            // assert
            Assert.IsNotNull(taskItemDto);
            Assert.Positive(taskItemDto.Id);
            Assert.AreEqual(taskItem_Dest.ParentId, taskItemDto.Id);
            Assert.AreEqual(TaskItemState.Completed, taskItemDto.State);
        }

        [Test, Theory]
        [TestCase(WebRequestMethods.Http.Post)]
        public async Task ShouldReturn_BadRequest_TaskItemIsNull(string httpMethod)
        {
            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK_UPDATE}");
            var json = JsonConvert.SerializeObject(null);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);

            // assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, response.GetErrorMessage());
        }

        [Test, Theory]
        [TestCase(WebRequestMethods.Http.Post)]
        public async Task ShouldReturn_NotFound_TaskItem(string httpMethod)
        {
            // data
            TaskItemDto taskItemDto_Dest = _demoTaskItems.FirstOrDefault().ToDto();
            taskItemDto_Dest.Id = int.MaxValue;

            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK_UPDATE}");
            var json = JsonConvert.SerializeObject(taskItemDto_Dest);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);

            // assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, response.GetErrorMessage());
        }

        [Test, Theory]
        [TestCase(WebRequestMethods.Http.Post)]
        public async Task ShouldReturn_BadRequest_IdIsZero(string httpMethod)
        {
            // data
            TaskItemDto taskItemDto_Dest = _demoTaskItems.FirstOrDefault().ToDto();
            taskItemDto_Dest.Id = 0;

            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK_UPDATE}");
            var json = JsonConvert.SerializeObject(taskItemDto_Dest);
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
            TaskItemDto taskItemDto_Dest = _demoTaskItems.FirstOrDefault().ToDto();
            taskItemDto_Dest.Name = String.Empty;

            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK_UPDATE}");
            var json = JsonConvert.SerializeObject(taskItemDto_Dest);
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
            TaskItemDto taskItemDto_Dest = _demoTaskItems.FirstOrDefault().ToDto();
            taskItemDto_Dest.Name = new string('x', DtoExtensions.TaskItem_Name_MaxLength + 1);

            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK_UPDATE}");
            var json = JsonConvert.SerializeObject(taskItemDto_Dest);
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
            TaskItemDto taskItemDto_Dest = _demoTaskItems.FirstOrDefault().ToDto();
            taskItemDto_Dest.Description = new string('x', DtoExtensions.TaskItem_Description_MaxLength + 1);

            // act
            var request = new HttpRequestMessage(new HttpMethod(httpMethod), $"{ApiList.CLIENT_API_TASK_UPDATE}");
            var json = JsonConvert.SerializeObject(taskItemDto_Dest);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);

            // assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, response.GetErrorMessage());
        }
    }
}