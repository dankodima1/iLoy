using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Tms.Dto.Extensions;
using Tms.Logger;

namespace Tms.Data.Context
{
    public static class ApplicationDbContextUtils
    {
        // creating db
        public static void UseMigration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            // migration
            try
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    ApplicationDbContext _context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    if (_context == null)
                    {
                        ITmsLogger _logger = new TmsLogger();
                        _logger.Error(null, DtoExtensions.GetErrorMessage_IsNull(nameof(_context)));
                        return;
                    }

                    if (_context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
                    {
                        return;
                    }

                    _context.Database.EnsureCreated();
                }
            }
            catch (Exception ex)
            {
                ITmsLogger _logger = new TmsLogger();
                _logger.Error(ex);
            }
        }
    }
}
