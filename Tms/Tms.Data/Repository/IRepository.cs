using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Tms.Data.Domain;

namespace Tms.Data.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Entities { get; set; }

        Task<T> GetAsync(int id);
        Task<IQueryable<T>> GetAsync(int[] ids);

        Task<T> CreateAsync(T entity);
        Task<IList<T>> CreateAsync(IList<T> entities);

        Task UpdateAsync(T entity);
        Task UpdateAsync(IList<T> entities);

        Task DeleteAsync(T entity);
        Task DeleteAsync(IList<T> entities);
    }
}
