using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
