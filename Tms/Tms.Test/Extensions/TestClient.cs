using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Moq;

using Tms.Data.Context;
using Tms.Data.Domain;
using Tms.Data.Repository;
using Tms.Web;

namespace Tms.Test.Extensions
{
    public class TestClient
    {
        private readonly TestServerFactory<Startup> _serverFactory;
        private readonly HttpClient _client;
        private Mock<ApplicationDbContext> _context;

        private Mock<DbSet<TaskItem>> _taskItemDbSet;
        private Mock<Repository<TaskItem>> _taskItemRepository;

        public HttpClient Client { get => _client; }
        public Mock<ApplicationDbContext> Context { get => _context; }
        public Mock<Repository<TaskItem>> TaskItemRepository { get => _taskItemRepository; }

        public TestClient()
        {
            // create test client
            _serverFactory = new TestServerFactory<Startup>();
            _client = _serverFactory.CreateClient();
        }

        public void SetupContext()
        {
            // context options
            var mockOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("InMemoryDatabase")
                    .UseLazyLoadingProxies()
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
            _serverFactory?.Dispose();
            _client?.Dispose();
        }
    }
}
