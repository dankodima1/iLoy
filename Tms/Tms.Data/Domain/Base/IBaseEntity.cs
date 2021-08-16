using System;

namespace Tms.Data.Domain
{
    public interface IBaseEntity
    {
        public DateTime? CreatedOnUtc { get; set; }
        //public int? CreatedById { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
        //public int? UpdatedById { get; set; }
    }
}
