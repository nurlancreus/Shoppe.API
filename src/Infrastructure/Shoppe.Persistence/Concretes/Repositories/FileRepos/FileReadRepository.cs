using Shoppe.Application.Abstractions.Repositories.FileRepos;
using Shoppe.Domain.Entities.Files;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.FileRepos
{
    public class FileReadRepository : ReadRepository<ApplicationFile>, IFileReadRepository
    {
        public FileReadRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
