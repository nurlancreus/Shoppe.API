using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ShoppeDbContext>
    {

        public ShoppeDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShoppeDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=ShoppeDevDb;Integrated Security=true;TrustServerCertificate=true;");

            return new ShoppeDbContext(optionsBuilder.Options);
        }
    }
}
