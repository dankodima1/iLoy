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
using Tms.Api.Extensions;
using Tms.Dto.Extensions;

namespace Tms.Test.Dto
{
    [TestFixture]
    public class TestDtoExtensions
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