using Microsoft.Extensions.Configuration;
using Shoppe.Application.Abstractions.Services.Storage.AWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Infrastructure.Concretes.Services.Storage.AWS
{
    public class AWSFileUrlGenerator : IAWSFileUrlGenerator
    {
        private readonly IConfiguration _configuration;

        public AWSFileUrlGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateUrl(string? pathName, string? fileName)
        {
            var bucketName = _configuration["Storage:AWS:AWSS3:BucketName"];
            var region = _configuration["Storage:AWS:Region"];

            if (string.IsNullOrEmpty(bucketName) || string.IsNullOrEmpty(region))
            {
                throw new Exception("Missing S3 bucket or region information in configuration.");
            }

            if (string.IsNullOrEmpty(pathName) || string.IsNullOrEmpty(fileName))
            {
                return string.Empty;
            }

            // Construct the S3 URL
            return $"https://{bucketName}.s3.{region}.amazonaws.com/{pathName}/{fileName}";
        }
    }
}
