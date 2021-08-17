using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Tms.Data.Context;
using Tms.Web;

namespace Tms.Test.Extensions
{
    public class TestServerFactory<TStartup> : WebApplicationFactory<Startup>
    {
        private const string AppsettingsJsonFile = "appsettings.Test.json";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                var configuration = (IConfiguration)new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile(AppsettingsJsonFile)
                    .Build();

                config.AddConfiguration(configuration);
            });

            builder.ConfigureServices(services =>
            {
                // create a new service provider
                var provider = services
                    .AddEntityFrameworkInMemoryDatabase()
                    .AddEntityFrameworkProxies()
                    .BuildServiceProvider();

                // add a database context for testing
                services
                    .AddDbContext<ApplicationDbContext>(options =>
                        options
                            .UseInMemoryDatabase("InMemoryDatabase")
                            .UseInternalServiceProvider(provider)
                            .UseLazyLoadingProxies()
                            );

                // build the service provider
                var serviceProvider = services.BuildServiceProvider();

                // create a scope to obtain a reference to the database
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();

                    // ensure the database is created
                    db.Database.EnsureCreated();
                }
            });
        }
    }
}
