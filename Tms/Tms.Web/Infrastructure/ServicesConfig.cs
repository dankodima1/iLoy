using Microsoft.Extensions.DependencyInjection;

using Tms.Data.Domain;
using Tms.Data.Repository;
using Tms.Logger;
using Tms.Service;

namespace Tms.Web.Infrastructure
{
    public static class ServicesConfig
    {
        public static void ServicesInitialize(this IServiceCollection services)
        {
            services.AddScoped<ITmsLogger, TmsLogger>();
            services.AddScoped<ITaskItemService, TaskItemService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IRepository<TaskItem>, Repository<TaskItem>>();
        }
    }
}
