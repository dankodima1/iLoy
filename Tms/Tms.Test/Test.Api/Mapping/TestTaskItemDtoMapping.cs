using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;

using Tms.Data.Domain;
using Tms.Dto;
using Tms.Data.Demo;
using Tms.Api.Mapping;

namespace Tms.Test.Api
{
    [TestFixture]
    public class TestTaskItemDtoMapping
    {
        private readonly DemoData _demoData;
        private readonly IEnumerable<TaskItem> _demoTaskItems;

        public TestTaskItemDtoMapping()
        {
            // demo data
            _demoData = new DemoData();
            _demoTaskItems = _demoData.GetTaskItems();
        }

        [Test, Theory]
        [TestCase(1, 6)]
        public void ShouldMap_TaskItem_To_TaskItem(int srcId, int destId)
        {
            // data
            TaskItem taskItem_Src = _demoTaskItems.FirstOrDefault(x => x.Id == srcId);
            TaskItem taskItem_Dest = _demoTaskItems.FirstOrDefault(x => x.Id == destId);

            // act
            taskItem_Src.CopyTo(taskItem_Dest);

            // assert
            Assert.IsNotNull(taskItem_Dest);
            Assert.AreEqual(destId, taskItem_Dest.Id);
            Assert.AreEqual(taskItem_Src.Name, taskItem_Dest.Name);
            Assert.AreEqual(taskItem_Src.Description, taskItem_Dest.Description);
            Assert.AreEqual(taskItem_Src.StartDateUtc, taskItem_Dest.StartDateUtc);
            Assert.AreEqual(taskItem_Src.FinishDateUtc, taskItem_Dest.FinishDateUtc);
            Assert.AreEqual(taskItem_Src.State, taskItem_Dest.State);
        }

        [Test, Theory]
        [TestCase(1, 6)]
        public void ShouldMap_TaskItemDto_To_TaskItemDto(int srcId, int destId)
        {
            // data
            TaskItemDto taskItemDto_Src = _demoData.GetSingleTaskItemDto();
            taskItemDto_Src.Id = srcId;

            TaskItemDto taskItemDto_Dest = _demoData.GetSingleTaskItemDto();
            taskItemDto_Dest.Id = destId;

            // act
            taskItemDto_Src.CopyTo(taskItemDto_Dest);

            // assert
            Assert.IsNotNull(taskItemDto_Dest);
            Assert.AreEqual(destId, taskItemDto_Dest.Id);
            Assert.AreEqual(taskItemDto_Src.Name, taskItemDto_Dest.Name);
            Assert.AreEqual(taskItemDto_Src.Description, taskItemDto_Dest.Description);
            Assert.AreEqual(taskItemDto_Src.StartDateUtc, taskItemDto_Dest.StartDateUtc);
            Assert.AreEqual(taskItemDto_Src.FinishDateUtc, taskItemDto_Dest.FinishDateUtc);
            Assert.AreEqual(taskItemDto_Src.State, taskItemDto_Dest.State);
        }

        [Test, Theory]
        [TestCase(1)]
        public void ShouldMap_TaskItem_To_TaskItemDto(int id)
        {
            // data
            TaskItem taskItem = _demoTaskItems.FirstOrDefault(x => x.Id == id);

            // act
            TaskItemDto taskItemDto = taskItem.ToDto();

            // assert
            Assert.IsNotNull(taskItemDto);
            Assert.AreEqual(taskItem.Id, taskItemDto.Id);
            Assert.AreEqual(taskItem.Name, taskItemDto.Name);
            Assert.AreEqual(taskItem.Description, taskItemDto.Description);
            Assert.AreEqual(taskItem.StartDateUtc, taskItemDto.StartDateUtc);
            Assert.AreEqual(taskItem.FinishDateUtc, taskItemDto.FinishDateUtc);
            Assert.AreEqual(taskItem.State, taskItemDto.State);
        }

        [Test, Theory]
        [TestCase(1)]
        public void ShouldMap_TaskItemDto_To_TaskItem(int id)
        {
            // data
            TaskItemDto taskItemDto = _demoData.GetSingleTaskItemDto();
            taskItemDto.Id = id;

            // act
            TaskItem taskItem = taskItemDto.ToEnt();

            // assert
            Assert.IsNotNull(taskItem);
            Assert.AreEqual(taskItemDto.Id, taskItem.Id);
            Assert.AreEqual(taskItemDto.Name, taskItem.Name);
            Assert.AreEqual(taskItemDto.Description, taskItem.Description);
            Assert.AreEqual(taskItemDto.StartDateUtc, taskItem.StartDateUtc);
            Assert.AreEqual(taskItemDto.FinishDateUtc, taskItem.FinishDateUtc);
            Assert.AreEqual(taskItemDto.State, taskItem.State);
        }

        [Test, Theory]
        [TestCase(1)]
        public void ShouldMap_TaskItemDto_To_TaskItem_ForExisting(int id)
        {
            // data
            TaskItemDto taskItemDto = _demoData.GetSingleTaskItemDto();
            taskItemDto.Id = id;

            // existing entity
            TaskItem taskItem = _demoTaskItems.FirstOrDefault(x => x.Id == id);

            // act
            taskItemDto.ToEnt(taskItem);

            // assert
            Assert.IsNotNull(taskItem);
            Assert.AreEqual(taskItemDto.Id, taskItem.Id);
            Assert.AreEqual(taskItemDto.Name, taskItem.Name);
            Assert.AreEqual(taskItemDto.Description, taskItem.Description);
            Assert.AreEqual(taskItemDto.StartDateUtc, taskItem.StartDateUtc);
            Assert.AreEqual(taskItemDto.FinishDateUtc, taskItem.FinishDateUtc);
            Assert.AreEqual(taskItemDto.State, taskItem.State);
        }

        [Test, Theory]
        public void ShouldMap_TaskItems_To_TaskItemDtos()
        {
            // data
            IList<TaskItem> taskItems = _demoTaskItems.ToList();

            // act
            IList<TaskItemDto> taskItemDtos = taskItems.ToDto();

            // assert
            Assert.IsNotNull(taskItemDtos);
            Assert.IsNotEmpty(taskItemDtos);
            Assert.AreEqual(taskItems.Count, taskItemDtos.Count);
            foreach (var taskItemDto in taskItemDtos)
            {
                TaskItem taskItem = taskItems.FirstOrDefault(x => x.Id == taskItemDto.Id);
                Assert.IsNotNull(taskItem);
                Assert.AreEqual(taskItem.Name, taskItemDto.Name);
                Assert.AreEqual(taskItem.Description, taskItemDto.Description);
                Assert.AreEqual(taskItem.StartDateUtc, taskItemDto.StartDateUtc);
                Assert.AreEqual(taskItem.FinishDateUtc, taskItemDto.FinishDateUtc);
                Assert.AreEqual(taskItem.State, taskItemDto.State);
            }
        }
    }
}