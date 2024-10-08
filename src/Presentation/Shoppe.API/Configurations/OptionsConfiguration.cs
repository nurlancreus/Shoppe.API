﻿using Microsoft.AspNetCore.Identity;
using Shoppe.Application.Options.Storage;
using System.Configuration;

namespace Shoppe.API.Configurations
{
    public static class OptionsConfiguration
    {
        public static void ConfigureOptions (this WebApplicationBuilder builder)
        {
            builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection("Storage"));
            builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("Token"));
        }
    }
}
