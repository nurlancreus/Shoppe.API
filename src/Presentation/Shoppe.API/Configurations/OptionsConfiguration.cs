using Shoppe.Application.Options.Mail;
using Shoppe.Application.Options.Storage;
using Shoppe.Application.Options.Token;
using System.Configuration;

namespace Shoppe.API.Configurations
{
    public static class OptionsConfiguration
    {
        public static void ConfigureOptions (this WebApplicationBuilder builder)
        {
            builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection("Storage"));
            builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("Token"));
            builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailConfiguration"));
        }
    }
}
