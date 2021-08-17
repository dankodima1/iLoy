using System;
using Microsoft.EntityFrameworkCore;

using Tms.Data.Domain;
using Tms.Data.Demo;
using Tms.Logger;

namespace Tms.Data.Context
{
    public partial class ApplicationDbContext
    {
        private void SeedData(ModelBuilder modelBuilder)
        {
            try
            {
                this.SeedDataTaskItems(modelBuilder);
            }
            catch (Exception ex)
            {
                ITmsLogger _logger = new TmsLogger();
                _logger.Error(ex);
            }
        }

        private void SeedDataTaskItems(ModelBuilder modelBuilder)
        {
            DemoData demoData = new DemoData();
            modelBuilder.Entity<TaskItem>().HasData(demoData.GetTaskItems());
        }
    }
}
