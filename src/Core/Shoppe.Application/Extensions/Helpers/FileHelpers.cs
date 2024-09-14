using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Helpers
{
    public static class FileHelpers
    {
        private static readonly Dictionary<string, string> CharacterReplacements = new()
        {
            {"\"", ""}, {"!", ""}, {"'", ""}, {"^", ""}, {"+", ""}, {"%", ""},
            {"&", ""}, {"/", ""}, {"(", ""}, {")", ""}, {"=", ""}, {"?", ""},
            {"_", ""}, {" ", "-"}, {"@", ""}, {"€", ""}, {"¨", ""}, {"~", ""},
            {",", ""}, {";", ""}, {":", ""}, {".", "-"}, {"Ə", "e"},{"ə", "e"}, {"Ö", "o"}, {"ö", "o"},
            {"Ü", "u"}, {"ü", "u"}, {"ı", "i"}, {"İ", "i"}, {"ğ", "g"}, {"Ğ", "g"},
            {"æ", ""}, {"ß", ""}, {"â", "a"}, {"î", "i"}, {"ş", "s"}, {"Ş", "s"},
            {"Ç", "c"}, {"ç", "c"}, {"<", ""}, {">", ""}, {"|", ""}
        };

        public static string CharacterRegulatory(string name)
        {
            foreach (var replacement in CharacterReplacements)
            {
                name = name.Replace(replacement.Key, replacement.Value);
            }
            return name;
        }

        public static async Task<string> RenameFileAsync(string path, string fileName, Func<string, string, Task<bool>> hasFileAsync)
        {
            string oldName = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string newFileName = $"{CharacterRegulatory(oldName)}{extension}";

            int counter = 1;
            while (await hasFileAsync(path, newFileName))
            {
                newFileName = $"{CharacterRegulatory(oldName)}-{counter++}{extension}";
            }

            return newFileName;
        }

        public static async Task EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                await Task.CompletedTask; // Simulating async for consistency

            }
        }

        public static async Task CleanupFailedUploads(List<(string fileName, string path)> uploadedFiles)
        {
            foreach (var (_, path) in uploadedFiles)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);

                    await Task.CompletedTask; // Simulating async for consistency
                }
            }
        }

        public static bool IsImage(this IFormFile formFile)
        {
            return formFile.ContentType.Contains("image");
        }

        public static bool IsSizeOk(this IFormFile formFile, int mb)
        {
            // Convert file length from bytes to megabytes
            double fileSizeInMB = formFile.Length / (1024.0 * 1024.0);
            return fileSizeInMB <= mb;
        }

        public static bool RestrictExtension(this IFormFile formFile, string[]? permittedExtensions = null)
        {
            permittedExtensions ??= [".jpg", ".png", ".gif"];

            string extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
            return !string.IsNullOrEmpty(extension) && permittedExtensions.Contains(extension);

        }

        public static bool RestrictMimeTypes(this IFormFile formFile, string[]? permittedMimeTypes = null)
        {
            permittedMimeTypes ??= ["image/jpeg", "image/png", "image/gif"];

            string mimeType = formFile.ContentType;
            return permittedMimeTypes.Contains(mimeType);

        }
    }
}
