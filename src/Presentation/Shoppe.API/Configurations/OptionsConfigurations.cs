using Shoppe.Application.Options.Storage;
using System.Configuration;

namespace Shoppe.API.Configurations
{
    public static class OptionsConfigurations
    {
        public static void ConfigureOptions (this WebApplicationBuilder builder)
        {
            builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection("Storage"));
        }
    }
}
