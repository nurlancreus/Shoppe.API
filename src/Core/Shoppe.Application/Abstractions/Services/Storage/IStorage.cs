using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Storage
{
    public interface IStorage
    {
        Task<List<(string path, string fileName)>> UploadAsync(string path, IFormFileCollection formFiles);
        Task DeleteAsync(string path, string fileName);
        Task DeleteAllAsync(string path);
        Task<List<(string path, string fileName)>> GetFilesAsync(string path);
        Task<bool> HasFileAsync(string path, string fileName);
    }
}
