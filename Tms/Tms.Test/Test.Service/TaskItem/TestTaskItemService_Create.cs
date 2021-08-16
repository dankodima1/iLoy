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
using Tms.Dto.Extensions;

namespace Tms.Test.Service
{
    [TestFixture]
    public class TestTaskItemService_Create
    {
        private readonly ITmsLogger _logger;
        private readonly ITaskItemService _taskItemService;
        private readonly TestClient _client;
        private readonly IEnumerable<TaskItem> _demoTaskItems;
        private readonly DemoData _demoData;

        public TestTaskItemService_Create()
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
        public async Task ShouldCreate_And_Return_NewSingleTaskItem()
        {
            // data
            TaskItem taskItem_Src = _demoData.GetSingleTaskItem();

            // act
            var taskItem = await _taskItemService.CreateAsync(taskItem_Src);

            // assert
            Assert.IsNotNull(taskItem);
            Assert.Positive(taskItem.Id);
            Assert.AreEqual(taskItem_Src.Name, taskItem.Name);
            Assert.AreEqual(taskItem_Src.Description, taskItem.Description);
            Assert.AreEqual(taskItem_Src.StartDateUtc, taskItem.StartDateUtc);
            Assert.AreEqual(taskItem_Src.FinishDateUtc, taskItem.FinishDateUtc);
            Assert.AreEqual(taskItem_Src.Subtasks.Count(), taskItem.Subtasks.Count());
        }

        [Test, Theory]
        public void ShouldReturn_BadRequest_TaskItemIsNull()
        {
            // act
            Assert.ThrowsAsync<Exception>(async () => await _taskItemService.CreateAsync(null), DtoExtensions.GetErrorMessage_IsNull("taskItem"));
        }

        [Test, Theory]
        public void ShouldReturn_BadRequest_IdIsNotZero()
        {
            // data
            TaskItem taskItem_Src = _demoData.GetSingleTaskItem();
            taskItem_Src.Id = 1;

            // act
            Assert.ThrowsAsync<Exception>(async () => await _taskItemService.CreateAsync(taskItem_Src));
        }

        [Test, Theory]
        public void ShouldReturn_BadRequest_NameIsNull()
        {
            // data
            TaskItem taskItem_Src = _demoData.GetSingleTaskItem();
            taskItem_Src.Name = String.Empty;

            // act
            Assert.ThrowsAsync<Exception>(async () => await _taskItemService.CreateAsync(taskItem_Src));
        }

        [Test, Theory]
        public void ShouldReturn_BadRequest_NameIsExceed()
        {
            // data
            TaskItem taskItem_Src = _demoData.GetSingleTaskItem();
            taskItem_Src.Name = new string('x', DtoExtensions.TaskItem_Name_MaxLength + 1);

            // act
            Assert.ThrowsAsync<Exception>(async () => await _taskItemService.CreateAsync(taskItem_Src));
        }

        [Test, Theory]
        public void ShouldReturn_BadRequest_DescriptionIsExceed()
        {
            // data
            TaskItem taskItem_Src = _demoData.GetSingleTaskItem();
            taskItem_Src.Description = new string('x', DtoExtensions.TaskItem_Description_MaxLength + 1);

            // act
            Assert.ThrowsAsync<Exception>(async () => await _taskItemService.CreateAsync(taskItem_Src));
        }
    }
}