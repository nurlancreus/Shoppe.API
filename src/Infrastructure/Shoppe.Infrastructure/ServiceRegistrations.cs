using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoppe.Application.Abstractions.Services.Storage.AWS;
using Shoppe.Application.Abstractions.Services.Storage;
using Shoppe.Domain.Enums;
using Shoppe.Infrastructure.Concretes.Services.Storage.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoppe.Infrastructure.Concretes.Services.Storage.AWS;

namespace Shoppe.Infrastructure
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
        {
            return services;
        }

        public static void AddStorage(this IServiceCollection services, StorageType storageType, IConfiguration configuration)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;

                case StorageType.AWS:
                    ConfigureAWSServices(services, configuration);
                    break;

                default:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }

        private static void ConfigureAWSServices(IServiceCollection services, IConfiguration configuration)
        {
            var awsOptions = configuration.GetAWSOptions();
            awsOptions.Credentials = new Amazon.Runtime.BasicAWSCredentials(
                configuration["Storage:AWS:AccessKey"],
                configuration["Storage:AWS:SecretAccessKey"]);
            awsOptions.Region = Amazon.RegionEndpoint.GetBySystemName(configuration["Storage:AWS:Region"]); // Ensure the region is set


            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonS3>();

            services.AddScoped<IStorage, AWSStorage>();
        }
    }
}
