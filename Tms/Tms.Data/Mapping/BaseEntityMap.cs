using Microsoft.EntityFrameworkCore;

using Tms.Data.Domain;

namespace Tms.Data.Mapping
{
    public static class BaseEntityMap
    {
        public static void Configure(ModelBuilder builder)
        {
            //builder.Entity<TaskItem>()
            //    .HasOne(x => x.Parent).WithMany(x => x.Subtasks).HasForeignKey(x => x.ParentId)
            //    .OnDelete(DeleteBehavior.SetNull);// delete Subtask if deleted Parent

            builder.Entity<TaskItem>()
                .Property(x => x.CreatedOnUtc)
                .HasDefaultValueSql("getutcdate()");

            builder.Entity<TaskItem>()
                .Property(x => x.UpdatedOnUtc)
                .HasDefaultValueSql("getutcdate()");
        }
    }
}
