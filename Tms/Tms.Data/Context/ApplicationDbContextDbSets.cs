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
    public partial class ApplicationDbContext
    {
        #region DB SETS

        public virtual DbSet<TaskItem> TaskItems { get; set; }

        #endregion DB SETS
    }
}
