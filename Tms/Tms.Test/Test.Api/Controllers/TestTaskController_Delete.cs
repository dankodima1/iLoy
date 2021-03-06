using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

using NUnit.Framework;

using Tms.Data.Domain;
using Tms.Data.Demo;
using Tms.Dto;
using Tms.Test.Extensions;

namespace Tms.Test.Api
{
    [TestFixture]
    public class TestTaskController_Delete
    {
        private const string HttpDelete = "DELETE";
        private readonly TestClient _client;
        private readonly DemoData _demoData;
        private readonly IEnumerable<TaskItem> _demoTaskItems;

        public TestTaskController_Delete()
        {
            // demo data
            _demoData = new DemoData();
            _demoTaskItems = _demoData.GetTaskItems();

            // create test client
            _client = new TestClient();
            _client.SetupContext();
            _client.SetupRepository(_demoTaskItems);
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