using System;
using System.Linq;
using System.Diagnostics;

using NUnit.Framework;
using Tms.Data.Demo;

namespace Tms.Test.Data
{
    [TestFixture]
    public class NonDataDrivenTests
    {
        [Test]
        public void TestDemoData_WriteToOutput()
        {
            DemoData demoData = new DemoData();
            var demoTaskItems = demoData.GetTaskItems();
            string msg = string.Join(Environment.NewLine, demoTaskItems.Select(x => x.Show));
            Debug.WriteLine(msg);
        }

    }
}