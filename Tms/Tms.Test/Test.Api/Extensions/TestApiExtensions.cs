using NUnit.Framework;

using Tms.Data.Domain;
using Tms.Dto;
using Tms.Dto.Extensions;

namespace Tms.Test.Api
{
    [TestFixture]
    public class TestApiExtensions
    {
        [Test]
        public void ShouldReturn_ErrorMessage_IsNull()
        {
            TaskItemDto taskItemDto = null;
            string msg = DtoExtensions.GetErrorMessage_IsNull(nameof(taskItemDto));

            Assert.AreEqual($"{nameof(taskItemDto)} is null", msg);
        }

        [Test]
        public void ShouldReturn_ErrorMessage_NotFound()
        {
            int id = 1;
            string msg = DtoExtensions.GetErrorMessage_NotFound(nameof(TaskItem), id);

            Assert.AreEqual($"{nameof(TaskItem)} with Id = ({id}) not found", msg);
        }

        [Test]
        public void ShouldReturn_ErrorMessage_ShouldBe()
        {
            int id = 1;
            string msg = DtoExtensions.GetErrorMessage_ShouldBe(nameof(id), nameof(TaskItem), id);

            Assert.AreEqual($"{nameof(id)} of the {nameof(TaskItem)} should be {id}", msg);
        }
    }
}