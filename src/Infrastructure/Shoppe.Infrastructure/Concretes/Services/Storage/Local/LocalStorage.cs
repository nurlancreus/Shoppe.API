using Microsoft.AspNetCore.Http;
using Shoppe.Application.Abstractions.Services.Storage.Local;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Shoppe.Infrastructure.Concretes.Services.Storage.Local
{
    public class LocalStorage : ILocalStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task DeleteAsync(string path, string fileName)
        {

            if (await HasFileAsync(path, fileName))
                File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, path, fileName));

            await Task.CompletedTask;
        }


        public async Task<List<(string path, string fileName)>> GetFilesAsync(string path)
        {
            DirectoryInfo directory = new(Path.Combine(_webHostEnvironment.WebRootPath, path));
            return await Task.FromResult(directory.GetFiles().Select(f => (f.Name, directory.Name)).ToList());
        }

        public async Task<bool> HasFileAsync(string path, string fileName)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, path, fileName);
            return await Task.FromResult(File.Exists(filePath));
        }

        private static async Task<bool> CopyFileAsync(string path, IFormFile formFile)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: true);
                await formFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                throw new Exception($"Error copying file: {ex.Message}");
                // return false;
            }
        }

        public async Task<List<(string path, string fileName)>> UploadAsync(string path, IFormFileCollection formFiles)
        {
            if (formFiles.Count == 0)
            {
                throw new ArgumentException("No files uploaded.");
            }

            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

            await FileHelpers.EnsureDirectoryExists(uploadPath);

            var uploadResults = new List<(string path, string fileName)>();

            foreach (IFormFile formFile in formFiles)
            {
                string newFileName = await FileHelpers.RenameFileAsync(path, formFile.FileName, HasFileAsync);

                string fullPath = Path.Combine(uploadPath, newFileName);

                bool isCopied = await CopyFileAsync(fullPath, formFile);

                if (!isCopied)
                {
                    // Optionally, you could delete all uploaded files in case of failure.
                    await FileHelpers.CleanupFailedUploads(uploadResults);
                    throw new Exception("File upload failed.");
                }

                uploadResults.Add((path, newFileName));
            }

            return uploadResults;
        }

        public async Task DeleteAllAsync(string path)
        {
            var di = new DirectoryInfo(path);

            if (di.Exists)
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

            }

            await Task.CompletedTask;
        }
    }
}
