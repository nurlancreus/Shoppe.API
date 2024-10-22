using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shoppe.Application.Abstractions.Services.Storage.AWS;
using Shoppe.Application.Helpers;
using Shoppe.Application.Options.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Infrastructure.Concretes.Services.Storage.AWS
{
    public class AWSStorage : IAWSStorage
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly List<string> _uploadedFiles; // Track uploaded files for rollback

        public AWSStorage(IAmazonS3 s3Client, IOptions<StorageOptions> storageOptions)
        {
            _bucketName = storageOptions.Value.AWS.AWSS3.BucketName;
            _s3Client = s3Client;
            _uploadedFiles = [];
        }

        public async Task DeleteAsync(string path, string fileName)
        {
            var key = GetS3Key(path, fileName);
            await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            });
        }

        public async Task DeleteAllAsync(string path)
        {
            var prefix = path.EndsWith("/") ? path : $"{path}/";
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
                    await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
                    {
                        BucketName = _bucketName,
                        Key = s3Object.Key
                    });
                }

                listObjectsRequest.ContinuationToken = listObjectsResponse.NextContinuationToken;

            } while (listObjectsResponse.IsTruncated);
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
                if (!s3Object.Key.EndsWith("/"))
                {
                    string fileName = GetFileNameFromKey(s3Object.Key);
                    string pathName = GetPathFromKey(s3Object.Key);
                    files.Add((pathName, fileName));
                }
            }

            return files;
        }

        public async Task<bool> HasFileAsync(string path, string fileName)
        {
            var key = GetS3Key(path, fileName);

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

        public async Task<(string path, string fileName)> UploadAsync(string path, IFormFile formFile)
        {
            string newFileName = await FileHelpers.RenameFileAsync(path, formFile.FileName, HasFileAsync);
            var key = GetS3Key(path, newFileName);

            await UploadFileToS3Async(formFile, key);

            // Enlist this operation in the current transaction
            EnlistInTransaction(key);

            return (path, newFileName);
        }

        public async Task<List<(string path, string fileName)>> UploadMultipleAsync(string path, IFormFileCollection formFiles)
        {
            var uploadedFiles = new List<(string path, string fileName)>();

            foreach (var formFile in formFiles)
            {
                if (formFile.Length > 0)
                {
                    var (pathName, fileName) = await UploadAsync(path, formFile);
                    uploadedFiles.Add((pathName, fileName));
                }
            }

            return uploadedFiles;
        }

        // Helper methods to work with S3 keys and file operations
        private string GetS3Key(string path, string fileName)
        {
            return Path.Combine(path, fileName).Replace("\\", "/");
        }

        private async Task UploadFileToS3Async(IFormFile formFile, string key)
        {
            using var stream = formFile.OpenReadStream();
            var uploadRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = stream,
                ContentType = formFile.ContentType
            };

            await _s3Client.PutObjectAsync(uploadRequest);

            // Track uploaded file for possible rollback
            _uploadedFiles.Add(key);
        }

        private string GetFileNameFromKey(string key)
        {
            var keyParts = key.Split('/');
            return keyParts[^1];
        }

        private string GetPathFromKey(string key)
        {
            var keyParts = key.Split('/');
            return string.Join("/", keyParts.Take(keyParts.Length - 1));
        }

        private void EnlistInTransaction(string key)
        {
            if (Transaction.Current != null)
            {
                // Enlist this service in the current transaction
                Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
            }
        }

        // IEnlistmentNotification Implementation for TransactionScope
        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            // Nothing specific to prepare for S3, so we mark it as prepared
            preparingEnlistment.Prepared();
        }

        public void Commit(Enlistment enlistment)
        {
            // Transaction has been successfully committed, nothing to do
            _uploadedFiles.Clear(); // Clear the rollback list
            enlistment.Done();
        }

        public async void Rollback(Enlistment enlistment)
        {
            // Rollback: Delete all uploaded files from S3
            await RollbackUploadedFiles(_uploadedFiles);
            _uploadedFiles.Clear(); // Clear the rollback list
            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            // Handle any "in doubt" situation (rare in most cases)
            enlistment.Done();
        }

        // Helper method to rollback all uploaded files in case of failure
        private async Task RollbackUploadedFiles(List<string> fileKeys)
        {
            foreach (var key in fileKeys)
            {
                try
                {
                    await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
                    {
                        BucketName = _bucketName,
                        Key = key
                    });
                }
                catch (Exception ex)
                {
                    // Log error for failed rollback (optional)
                    Console.WriteLine($"Error rolling back file with key {key}: {ex.Message}");
                }
            }
        }
    }
}
