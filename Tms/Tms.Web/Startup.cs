using System;
using System.IO;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using AutoMapper;

using Tms.Api.Mapping;
using Tms.Data.Context;
using Tms.Web.Infrastructure;

namespace Tms.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString) || connectionString == "InMemoryDatabase")
            {
                // use in-memory database
                ConfigureInMemoryDatabases(services);
            }
            else
            {
                // use real database
                ConfigureProductionServices(services);
            }
        }

        private void ConfigureInMemoryDatabases(IServiceCollection services)
        {
            // create a new service provider
            var provider = services
                .AddEntityFrameworkInMemoryDatabase()
                .AddEntityFrameworkProxies()
                .BuildServiceProvider();

            // use in-memory database
            services
                .AddDbContext<ApplicationDbContext>(options =>
                    options
                        .UseInMemoryDatabase("InMemoryDatabase")
                        .UseInternalServiceProvider(provider)
                        .UseLazyLoadingProxies()
                        );

            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            // use real database
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrEmpty(connectionString))
            {
                services
                    .AddDbContext<ApplicationDbContext>(options =>
                        options
                            .UseSqlServer(connectionString)
                            .EnableSensitiveDataLogging()
                            .UseLazyLoadingProxies());
            }

            ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // swagger
            string swaggerVersion = "v1";
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerVersion, new OpenApiInfo { Title = "API", Version = swaggerVersion });
                c.IncludeXmlComments($@"{System.AppDomain.CurrentDomain.BaseDirectory}\Tms.Api.xml");

                // loads generated documnt xml files to swagger service. It will be shown as comments for rest api
                foreach (var name in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml"))
                {
                    c.IncludeXmlComments(name);
                }
            });

            // configure auto mapper
            services.AddAutoMapper();

            // configure services
            services.ServicesInitialize();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // swagger
            app.UseSwagger();
            app.UseSwaggerUI(x => {
                x.SwaggerEndpoint("../swagger/v1/swagger.json", "API V1");
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // migration
            app.UseMigration(env);
        }
    }

    public static class ConfigureServicesUtils
    {
        /// <summary>
        /// Register and configure AutoMapper
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAutoMapper(this IServiceCollection services)
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperConfigurationApi());
            });

            // Registering
            IMapper mapper = mappingConfig.CreateMapper();
            AutoMapperConfiguration.Init(mappingConfig, mapper);
            services.AddSingleton(mapper);
        }
    }
}
