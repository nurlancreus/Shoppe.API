using Microsoft.AspNetCore.Http;
using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Application.Abstractions.Services.Storage
{
    public interface IStorage : IEnlistmentNotification
    {
        Task<(string path, string fileName)> UploadAsync(string path, IFormFile formFile);
        Task<List<(string path, string fileName)>> UploadMultipleAsync(string path, IFormFileCollection formFiles);
        Task DeleteAsync(string path, string fileName);
        Task DeleteAllAsync(string path);
        Task<List<(string path, string fileName)>> GetFilesAsync(string path);
        Task<bool> HasFileAsync(string path, string fileName);
    }
}
