using Microsoft.EntityFrameworkCore;

using Tms.Data.Domain;

namespace Tms.Data.Mapping
{
    public static class BaseEntityMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<TaskItem>()
                .Property(x => x.CreatedOnUtc)
                .HasDefaultValueSql("getutcdate()");

            builder.Entity<TaskItem>()
                .Property(x => x.UpdatedOnUtc)
                .HasDefaultValueSql("getutcdate()");
        }
    }
}
