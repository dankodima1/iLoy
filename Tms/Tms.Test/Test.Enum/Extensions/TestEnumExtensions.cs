using NUnit.Framework;

using Tms.Enum;
using Tms.Enum.Extensions;

namespace Tms.Test.Dto
{
    [TestFixture]
    public class TestEnumExtensions
    {
        [Test]
        public void ShouldReturn_Name_Of_TaskItemState()
        {
            Assert.AreEqual("Completed", TaskItemState.Completed.Name());
        }
    }
}