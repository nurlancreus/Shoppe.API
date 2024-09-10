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
    }
}
