using Microsoft.AspNetCore.Http;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Application.Helpers;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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

        public void Commit(Enlistment enlistment) => _storage.Commit(enlistment);

        public async Task DeleteAllAsync(string path)
          => await _storage.DeleteAllAsync(path);


        public async Task DeleteAsync(string path, string fileName)
            => await _storage.DeleteAsync(path, fileName);

        public async Task DeleteMultipleAsync<T>(ICollection<T> files) where T : ApplicationFile
        {
            foreach (var file in files)
            {
                await _storage.DeleteAsync(file.PathName, file.FileName);
            }
        }

        public Task<List<(string path, string fileName)>> GetFilesAsync(string path)
            => _storage.GetFilesAsync(path);

        public Task<bool> HasFileAsync(string path, string fileName)
            => _storage.HasFileAsync(path, fileName);

        public void InDoubt(Enlistment enlistment) => _storage.InDoubt(enlistment);

        public void Prepare(PreparingEnlistment preparingEnlistment) => _storage.Prepare(preparingEnlistment);

        public void Rollback(Enlistment enlistment) => _storage.Rollback(enlistment);

        public Task<(string path, string fileName)> UploadAsync(string path, IFormFile formFile)
         => _storage.UploadAsync(path, formFile);

        public Task<List<(string path, string fileName)>> UploadMultipleAsync(string path, IFormFileCollection files)
            => _storage.UploadMultipleAsync(path, files);
    }
}