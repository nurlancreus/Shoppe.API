using Microsoft.EntityFrameworkCore;
using Shoppe.Application.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        IProductReadRepository ProductReadRepository { get; }
        IProductWriteRepository ProductWriteRepository { get; }
        Task<int> SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
