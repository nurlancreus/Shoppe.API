using Microsoft.AspNetCore.Http;
using Shoppe.Application.Abstractions.Services.Storage.Local;
using Shoppe.Application.Abstractions.Services;
using Shoppe.Application.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            {
                File.Delete(GetFullPath(path, fileName));
            }
            await Task.CompletedTask;
        }

        public async Task<List<(string path, string fileName)>> GetFilesAsync(string path)
        {
            DirectoryInfo directory = new(GetFullPath(path));
            var files = directory.GetFiles().Select(f => (f.Name, directory.Name)).ToList();
            return await Task.FromResult(files);
        }

        public async Task<bool> HasFileAsync(string path, string fileName)
        {
            string filePath = GetFullPath(path, fileName);
            return await Task.FromResult(File.Exists(filePath));
        }

        public async Task<(string path, string fileName)> UploadAsync(string path, IFormFile formFile)
        {
            string uploadPath = GetFullPath(path);
            string newFileName = await FileHelpers.RenameFileAsync(path, formFile.FileName, HasFileAsync);

            string fullPath = Path.Combine(uploadPath, newFileName);
            bool isCopied = await CopyFileAsync(fullPath, formFile);

            if (!isCopied)
                throw new Exception("File copy failed.");

            return (path, newFileName);
        }

        public async Task<List<(string path, string fileName)>> UploadMultipleAsync(string path, IFormFileCollection formFiles)
        {
            if (formFiles.Count == 0)
            {
                throw new ArgumentException("No files uploaded.");
            }

            string uploadPath = GetFullPath(path);
            await FileHelpers.EnsureDirectoryExists(uploadPath);

            var uploadedFiles = new List<(string path, string fileName)>();

            foreach (var formFile in formFiles)
            {
                string newFileName = await FileHelpers.RenameFileAsync(path, formFile.FileName, HasFileAsync);
                string fullPath = Path.Combine(uploadPath, newFileName);

                bool isCopied = await CopyFileAsync(fullPath, formFile);

                if (!isCopied)
                {
                    await RollbackUploads(uploadedFiles);
                    throw new Exception("File upload failed. Rolling back previous uploads.");
                }

                uploadedFiles.Add((path, newFileName));
            }

            return uploadedFiles;
        }

        public async Task DeleteAllAsync(string path)
        {
            DirectoryInfo di = new(GetFullPath(path));
            if (di.Exists)
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }
            await Task.CompletedTask;
        }

        private string GetFullPath(string path, string fileName = null)
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, path, fileName ?? string.Empty);
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
                throw new Exception($"Error copying file: {ex.Message}");
            }
        }

        private async Task RollbackUploads(List<(string path, string fileName)> uploadedFiles)
        {
            foreach (var (path, fileName) in uploadedFiles)
            {
                string filePath = GetFullPath(path, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            await Task.CompletedTask;
        }
    }
}
