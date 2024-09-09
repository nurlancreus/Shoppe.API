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
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ShoppeDbContext _context;
        public Repository(ShoppeDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();
    }
}
