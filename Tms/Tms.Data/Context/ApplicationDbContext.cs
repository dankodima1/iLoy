using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Tms.Data.Domain;
using Tms.Data.Mapping;
using Tms.Logger;

namespace Tms.Data.Context
{
    // Initial migration:
    // Open IDE tab: Package Manager Console 
    // PM> add-migration migr01
    // PM> update-database
    //
    // Remove migration:
    // PM> update-database migr00
    //      or to the base level:
    //      update-database -migration:0
    // PM> remove-migration
    //
    // Reverting migration 4
    // PM> update-database -migration migr03
    // PM> remove-migration
    //
    // Remove migration to Base - empty database:
    // PM> update-database -migration:0
    public partial class ApplicationDbContext : DbContext
    {
        private readonly ITmsLogger _logger;

        public ApplicationDbContext(
            ) : base()
        {
        }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options
            ) : base(options)
        {
        }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ITmsLogger logger
            ) : base(options)
        {
            _logger = logger;
        }

        #region BASE

        /// <summary>
        /// Further configuration the model
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configure my entities
            BaseEntityMap.Configure(modelBuilder);

            // custom seed data
            this.SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            this.OnBeforeSaving();
            return base.SaveChanges();// it is will call SaveChanges(bool acceptAllChangesOnSuccess)
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);// it is will call SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        }

        private void OnBeforeSaving()
        {
            try
            {
                List<string> validationErrors = new List<string>();
                var currentDate = DateTime.UtcNow;
                foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity is IBaseEntity))
                {
                    var entity = entry.Entity as IBaseEntity;
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedOnUtc = currentDate;
                        entity.UpdatedOnUtc = currentDate;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        if (entity.CreatedOnUtc == DateTime.MinValue)
                        {
                            entity.CreatedOnUtc = currentDate;
                        }
                        entity.UpdatedOnUtc = currentDate;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        #endregion BASE

    }
}
