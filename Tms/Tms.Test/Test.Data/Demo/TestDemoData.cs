using System;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;

using NUnit.Framework;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using Moq;

using Tms.Api.Controllers;
using Tms.Data.Context;
using Tms.Data.Domain;
using Tms.Data.Demo;
using Tms.Logger;
using Tms.Service;
using Tms.Web;
using Tms.Dto;
using Tms.Data.Repository;
using Tms.Test.Extensions;

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