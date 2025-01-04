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
using Shoppe.Infrastructure.Concretes.Services.Storage;
using Shoppe.Application.Abstractions.Pagination;
using Shoppe.Infrastructure.Concretes.Services;
using Shoppe.Application.Abstractions.Services.Token;
using Shoppe.Infrastructure.Concretes.Services.Token;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Infrastructure.Concretes.Services.Session;
using Shoppe.Application.Abstractions.Services.Content;
using Shoppe.Infrastructure.Concretes.Services.Content;
using Shoppe.Application.Abstractions.Services.Mail;
using Shoppe.Infrastructure.Concretes.Services.Mail;
using Shoppe.Application.Abstractions.Services.Mail.Templates;
using Shoppe.Application.Abstractions.Services.Calculator;
using Shoppe.Application.Abstractions.Services.Validation;
using Shoppe.Infrastructure.Concretes.Services.Validation;
using Shoppe.Infrastructure.Concretes.Services.Payment.PayPal;
using Shoppe.Application.Abstractions.Services.Payment;
using Shoppe.Infrastructure.Concretes.Services.Payment;
using Shoppe.Infrastructure.Concretes.Services.Payment.Stripe;

namespace Shoppe.Infrastructure
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
        {
            #region Service Registrations
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IPaginationService, PaginationService>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IJwtSession, JwtSession>();
            services.AddScoped<IContentUpdater, ContentUpdater>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IAccountEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IContactEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IOrderEmailTemplateService, EmailTemplateService>();

            services.AddScoped<ICalculatorService, CalculatorService>();
            services.AddScoped<IDiscountCalculatorService, CalculatorService>();
            services.AddScoped<ICouponCalculatorService, CalculatorService>();
            services.AddScoped<IRatingCalculatorService, CalculatorService>();
            services.AddScoped<IShippingCalculatorService, CalculatorService>();
            services.AddScoped<IBasketCalculatorService, CalculatorService>();
            services.AddScoped<IPaymentCalculatorService, CalculatorService>();

            services.AddScoped<IAddressValidationService, AddressValidationService>();

            #region Payments
            services.AddSingleton<PaypalClient>();

            services.AddScoped<IPayPalService, PayPalService>();
            services.AddScoped<IStripeService, StripeService>();

            services.AddScoped<IPaymentService, PaymentService>();
            #endregion;

            #endregion

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
                    services.AddScoped<IStorage, AWSStorage>();
                    services.AddScoped<IFileUrlGenerator, AWSFileUrlGenerator>();
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


        }
    }
}
