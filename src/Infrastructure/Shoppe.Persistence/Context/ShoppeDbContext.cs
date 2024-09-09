using Microsoft.EntityFrameworkCore;
using Shoppe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Context
{
    public class ShoppeDbContext : DbContext
    {
        public ShoppeDbContext()
        {
            
        }
        public ShoppeDbContext(DbContextOptions<ShoppeDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Product> Products { get; set; }
    }
}
