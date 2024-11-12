using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Storage
{
    public interface IStorageService : IStorage
    {
        public StorageType StorageName { get; }
        Task DeleteMultipleAsync<T>(ICollection<T> files) where T : ApplicationFile;

    }
}
