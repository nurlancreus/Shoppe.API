using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shoppe.Application.Abstractions.Services.Storage.AWS;
using Shoppe.Application.Helpers;
using Shoppe.Application.Options.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Infrastructure.Concretes.Services.Storage.AWS
{
    public class AWSStorage : IAWSStorage
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly StorageOptions _storageOptions;

        public AWSStorage(IAmazonS3 s3Client, IOptions<StorageOptions> storageOptions)
        {
            _storageOptions = storageOptions.Value;
            _s3Client = s3Client;
            _bucketName = _storageOptions.AWS.AWSS3.BucketName;
        }

        public async Task DeleteAsync(string path, string fileName)
        {
            var key = Path.Combine(path, fileName).Replace("\\", "/");
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(deleteObjectRequest);
        }

        public async Task DeleteAllAsync(string path)
        {
            var prefix = path.Replace("\\", "/") + "/";

            var listObjectsRequest = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = prefix
            };

            ListObjectsV2Response listObjectsResponse;
            do
            {
                listObjectsResponse = await _s3Client.ListObjectsV2Async(listObjectsRequest);

                foreach (var s3Object in listObjectsResponse.S3Objects)
                {
                    var deleteObjectRequest = new DeleteObjectRequest
                    {
                        BucketName = _bucketName,
                        Key = s3Object.Key
                    };

                    await _s3Client.DeleteObjectAsync(deleteObjectRequest);
                }

                listObjectsRequest.ContinuationToken = listObjectsResponse.NextContinuationToken;

            } while (listObjectsResponse.IsTruncated); // Continue if there are more files to delete.
        }

        public async Task<List<(string path, string fileName)>> GetFilesAsync(string path)
        {
            var files = new List<(string path, string fileName)>();
            var listObjectsRequest = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = path.EndsWith("/") ? path : $"{path}/",
            };

            var listObjectsResponse = await _s3Client.ListObjectsV2Async(listObjectsRequest);

            foreach (var s3Object in listObjectsResponse.S3Objects)
            {
                // Ensure we're only getting files and not directories
                if (!s3Object.Key.EndsWith("/"))
                {
                    string[] pathAndName = s3Object.Key.Split('/');
                    string fileName = pathAndName[^1];
                    string pathName = pathAndName[0];

                    files.Add((pathName, fileName));
                }
            }

            return files;
        }


        public async Task<bool> HasFileAsync(string path, string fileName)
        {
            var key = Path.Combine(path, fileName).Replace("\\", "/");

            try
            {
                await _s3Client.GetObjectAsync(_bucketName, key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<(string path, string fileName)>> UploadAsync(string path, IFormFileCollection formFiles)
        {
            var uploadedFiles = new List<(string path, string fileName)>();

            foreach (var formFile in formFiles)
            {
                if (formFile.Length > 0)
                {
                    string fileNewName = await FileHelpers.RenameFileAsync(path, formFile.FileName, HasFileAsync);
                    var key = Path.Combine(path, fileNewName).Replace("\\", "/");

                    using var stream = formFile.OpenReadStream();
                    var uploadRequest = new PutObjectRequest
                    {
                        BucketName = _bucketName,
                        Key = key,
                        InputStream = stream,
                        ContentType = formFile.ContentType
                    };

                    await _s3Client.PutObjectAsync(uploadRequest);
                    uploadedFiles.Add((path, fileNewName));
                }
            }

            return uploadedFiles;
        }
    }
}
