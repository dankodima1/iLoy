using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

using Tms.Data.Context;
using Tms.Data.Domain;
using Tms.Data.Repository;
using Tms.Web;

namespace Tms.Test.Extensions
{
    public class TestClient
    {
        //private readonly TestServer _server;
        private readonly TestServerFactory<Startup> _serverFactory;
        private readonly HttpClient _client;
        private Mock<ApplicationDbContext> _context;

        private Mock<DbSet<TaskItem>> _taskItemDbSet;
        private Mock<Repository<TaskItem>> _taskItemRepository;

        //public TestServer Server { get => _server; }
        public HttpClient Client { get => _client; }
        public Mock<ApplicationDbContext> Context { get => _context; }
        public Mock<Repository<TaskItem>> TaskItemRepository { get => _taskItemRepository; }

        public TestClient()
        {
            //var configuration = (IConfiguration)new ConfigurationBuilder()
            //    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            //    .AddJsonFile("appsettings.Test.json")
            //    .Build();

            // create test server and client
            //_server = new TestServer(new WebHostBuilder()
            //    .UseEnvironment("Development")
            //    .UseStartup<Startup>()
            //    .UseConfiguration(configuration)
            //    );

            //var projectDir = Directory.GetCurrentDirectory();
            //var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.Test.json");

            _serverFactory = new TestServerFactory<Startup>();
            //    _serverFactory.WithWebHostBuilder(builder =>
            //    {
            //        builder.ConfigureAppConfiguration((context, config) =>
            //        {
            //            config.AddJsonFile(configPath);
            //            //config.AddConfiguration(configuration);
            //        });
            //    })
            //);

            // create test client
            _client = _serverFactory.CreateClient();
        }

        public void SetupContext()
        {
            // create a new service provider
            //var provider = services
            //    .AddEntityFrameworkInMemoryDatabase()
            //    .AddEntityFrameworkProxies()
            //    .BuildServiceProvider();

            // context options
            var mockOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("InMemoryDatabase")
                    //.UseLazyLoadingProxies()
                    .Options;

            // mock context
            _context = new Mock<ApplicationDbContext>(mockOptionsBuilder);
        }

        public void SetupRepository(IEnumerable<TaskItem> demoTaskItems)
        {
            if (_context == null)
                throw new Exception("call SetupContext first");

            // mock demo data
            _taskItemDbSet = new Mock<DbSet<TaskItem>>();
            _taskItemDbSet.SetSource(demoTaskItems);

            //_context.Setup(x => x.TaskItems).Returns(_taskItemDbSet.Object);
            _context.Setup(x => x.Set<TaskItem>()).Returns(_taskItemDbSet.Object);

            // mock TaskItem repository
            _taskItemRepository = new Mock<Repository<TaskItem>>(_context.Object);
            _taskItemRepository.Setup(x => x.Entities).Returns(_taskItemDbSet.Object);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await _client.SendAsync(request);
        }

        public void Dispose()
        {
            //_server?.Dispose();
            _serverFactory?.Dispose();
            _client?.Dispose();
        }
    }
}
