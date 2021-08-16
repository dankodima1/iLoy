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

namespace Tms.Test.Service
{
    [TestFixture]
    public class TestTaskItemService_Get
    {
        private readonly ITmsLogger _logger;
        private readonly ITaskItemService _taskItemService;
        private readonly TestClient _client;
        private readonly IEnumerable<TaskItem> _demoTaskItems;
        private readonly DemoData _demoData;

        public TestTaskItemService_Get()
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
            _taskItemService = new TaskItemService(_logger, _client.TaskItemRepository.Object);
        }

        [Test, Theory]
        public async Task ShouldReturn_AllTasksItems()
        {
            // act
            var testTaskItems = await _taskItemService.GetAsync();

            // assert
            Assert.IsNotNull(testTaskItems);
            Assert.IsNotEmpty(testTaskItems);
            Assert.AreEqual(_demoTaskItems.Count(), testTaskItems.Count);
        }

        [Test, Theory]
        public async Task ShouldReturn_AllParentTasksItems_IncludeSubtasksAsChilds()
        {
            // act
            var testTaskItems = await _taskItemService.GetTasksOnlyAsync();

            // assert
            Assert.IsNotNull(testTaskItems);
            Assert.IsNotEmpty(testTaskItems);
            Assert.AreEqual(_demoTaskItems.Where(x => x.ParentId == null).Count(), testTaskItems.Count);
        }

        [Test, Theory]
        [TestCase(1)]
        [TestCase(6)]
        public async Task ShouldReturn_SingleTaskItem(int id)
        {
            // act
            var testTaskItem = await _taskItemService.GetAsync(id);

            // assert
            Assert.IsNotNull(testTaskItem);
            Assert.AreEqual(_demoTaskItems.FirstOrDefault(x => x.Id == id)?.Name, testTaskItem.Name);
        }

        [Test, Theory]
        [TestCase(0)]
        [TestCase(int.MaxValue)]
        public async Task ShouldReturn_IsNullTaskItem(int id)
        {
            // act
            var testTaskItem = await _taskItemService.GetAsync(id);

            // assert
            Assert.AreEqual(null, testTaskItem);
        }
    }
}