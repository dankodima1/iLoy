using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Tms.Data.Domain;
using Tms.Data.Context;

namespace Tms.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        private DbSet<T> _entities { get; set; }
        public virtual DbSet<T> Entities
        {
            get => _entities ?? (_entities = _context.Set<T>());
            set => _entities = value;
        }

        public Repository(
            ApplicationDbContext context
            )
        {
            _context = context;
        }

        public async Task<T> GetAsync(int id)
        {
            //return await Entities.FindAsync(id);
            return Entities.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<IQueryable<T>> GetAsync(int[] ids)
        {
            //return (IQueryable<T>)await Entities.FindAsync(ids);
            return Entities.Where(x => ids.Contains(x.Id)).AsQueryable();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await Entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IList<T>> CreateAsync(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                await this.CreateAsync(entity);
            };
            return entities;
        }

        public async Task UpdateAsync(T entity)
        {
            Entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IList<T> entities)
        {
            Entities.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            Entities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(IList<T> entities)
        {
            Entities.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
