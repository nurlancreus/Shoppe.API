using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories;
using Shoppe.Domain.Entities.Base;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories
{
    public class WriteRepository<T> : Repository<T>, IWriteRepository<T> where T : BaseEntity
    {
        public WriteRepository(ShoppeDbContext context) : base(context)
        {
        }

        public bool Delete(T entity)
        {
            var entry = _context.Remove(entity);

            return entry.State == EntityState.Deleted;
        }

        public bool DeleteRange(IEnumerable<T> entities)
        {
            _context.RemoveRange(entities);

            return true;
        }

        public async Task<bool> InsertAsync(T entity)
        {
            var entry = await Table.AddAsync(entity);

            return _context.Entry(entry).State == EntityState.Added;
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            await Table.AddRangeAsync(entities);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public bool Update(T entity)
        {
            var entry = Table.Update(entity);
            return entry.State == EntityState.Modified;
        }
    }
}
