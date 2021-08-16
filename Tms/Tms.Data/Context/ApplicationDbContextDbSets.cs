using Microsoft.EntityFrameworkCore;

using Tms.Data.Domain;

namespace Tms.Data.Context
{
    public partial class ApplicationDbContext
    {
        #region DB SETS

        public virtual DbSet<TaskItem> TaskItems { get; set; }

        #endregion DB SETS
    }
}
