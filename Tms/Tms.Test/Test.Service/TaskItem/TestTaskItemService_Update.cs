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
using Tms.Test.Extensions;
using Tms.Dto.Extensions;

namespace Tms.Test.Api
{
    [TestFixture]
    public class TestTaskItemService_Update
    {
        private readonly ITmsLogger _logger;
        private readonly ITaskItemService _taskItemService;
        private readonly TestClient _client;
        private readonly IEnumerable<TaskItem> _demoTaskItems;
        private readonly DemoData _demoData;

        public TestTaskItemService_Update()
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
        public async Task ShouldUpdate_And_Return_ChangedSingleTaskItem()
        {
            // data
            TaskItem taskItem_Src = _demoData.GetSingleTaskItem();
            TaskItem taskItem_Dest = _demoTaskItems.FirstOrDefault(x => x.Id == 6);
            taskItem_Dest = taskItem_Src.CopyTo(taskItem_Dest);

            // act
            await _taskItemService.UpdateAsync(taskItem_Dest);

            // assert
            Assert.IsNotNull(taskItem_Dest);
            Assert.Positive(taskItem_Dest.Id);
            Assert.AreEqual(taskItem_Dest.Id, taskItem_Dest.Id);
            Assert.AreEqual(taskItem_Src.Name, taskItem_Dest.Name);
            Assert.AreEqual(taskItem_Src.Description, taskItem_Dest.Description);
            Assert.AreEqual(taskItem_Src.StartDateUtc, taskItem_Dest.StartDateUtc);
            Assert.AreEqual(taskItem_Src.FinishDateUtc, taskItem_Dest.FinishDateUtc);
            //Assert.AreEqual(taskItem_Src.Subtasks.Count(), taskItem.Subtasks.Count());
        }

        [Test, Theory]
        public void ShouldReturn_BadRequest_TaskItemIsNull()
        {
            // act
            Assert.ThrowsAsync<Exception>(async () => await _taskItemService.UpdateAsync(null));
        }

        [Test, Theory]
        public void ShouldReturn_NotFound_TaskItem()
        {
            // data
            TaskItem taskItem_Dest = _demoTaskItems.FirstOrDefault();
            taskItem_Dest.Id = int.MaxValue;

            // act
            Assert.ThrowsAsync<Exception>(async () => await _taskItemService.UpdateAsync(taskItem_Dest));
        }

        [Test, Theory]
        public void ShouldReturn_BadRequest_IdIsZero()
        {
            // data
            TaskItem taskItem_Dest = _demoTaskItems.FirstOrDefault();
            taskItem_Dest.Id = 0;

            // act
            Assert.ThrowsAsync<Exception>(async () => await _taskItemService.UpdateAsync(taskItem_Dest));
        }

        [Test, Theory]
        public void ShouldReturn_BadRequest_NameIsNull()
        {
            // data
            TaskItem taskItem_Dest = _demoTaskItems.FirstOrDefault();
            taskItem_Dest.Name = String.Empty;

            // act
            Assert.ThrowsAsync<Exception>(async () => await _taskItemService.UpdateAsync(taskItem_Dest));
        }

        [Test, Theory]
        public void ShouldReturn_BadRequest_NameIsExceed()
        {
            // data
            TaskItem taskItem_Dest = _demoTaskItems.FirstOrDefault();
            taskItem_Dest.Name = new string('x', DtoExtensions.TaskItem_Name_MaxLength + 1);

            // act
            Assert.ThrowsAsync<Exception>(async () => await _taskItemService.UpdateAsync(taskItem_Dest));
        }

        [Test, Theory]
        public void ShouldReturn_BadRequest_DescriptionIsExceed()
        {
            // data
            TaskItem taskItem_Dest = _demoTaskItems.FirstOrDefault();
            taskItem_Dest.Description = new string('x', DtoExtensions.TaskItem_Description_MaxLength + 1);

            // act
            Assert.ThrowsAsync<Exception>(async () => await _taskItemService.UpdateAsync(taskItem_Dest));
        }
    }
}