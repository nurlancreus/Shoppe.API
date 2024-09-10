using Microsoft.AspNetCore.Http;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Application.Helpers;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Infrastructure.Concretes.Services.Storage
{
    public class StorageService : IStorageService
    {
        private readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public StorageType StorageName
        {
            get
            {
                var name = _storage.GetType().Name.Replace("Storage", string.Empty);
                if (EnumHelpers.TryParseEnum(name, out StorageType storageType))
                {
                    return storageType;
                }

                else throw new InvalidOperationException("Cannot parse enum");
            }
        }

        public async Task DeleteAllAsync(string path)
          => await _storage.DeleteAllAsync(path);


        public async Task DeleteAsync(string path, string fileName)
            => await _storage.DeleteAsync(path, fileName);

        public Task<List<(string path, string fileName)>> GetFilesAsync(string path)
            => _storage.GetFilesAsync(path);

        public Task<bool> HasFileAsync(string path, string fileName)
            => _storage.HasFileAsync(path, fileName);

        public Task<List<(string path, string fileName)>> UploadAsync(string path, IFormFileCollection files)
            => _storage.UploadAsync(path, files);
    }
}