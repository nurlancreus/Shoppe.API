using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Shoppe.API.Configurations
{
    public static class AuthConfiguration
    {
        public static void ConfigureAuth(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
              //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
              //{
              //    options.Cookie.HttpOnly = true;
              //    options.Cookie.SecurePolicy = builder.Environment.IsProduction()
              //        ? CookieSecurePolicy.Always // Use this in production
              //        : CookieSecurePolicy.SameAsRequest; // For local development
              //    options.Cookie.SameSite = SameSiteMode.None; // Consider setting to SameSiteMode.None if cookies are not being sent
              //    options.Cookie.Name = "accessToken"; // Ensure this matches your client-side cookie name
              //})
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidateAudience = true,
                     ValidAudience = builder.Configuration["Token:Access:Audience"],
                     ValidateIssuer = true,
                     ValidIssuer = builder.Configuration["Token:Access:Issuer"],
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Token:Access:SecurityKey"]!)),

                     LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null && expires > DateTime.UtcNow,

                     NameClaimType = ClaimTypes.Name, // The value corresponding to the Name claim in the JWT can be obtained from the User.Identity.Name property.
                     RoleClaimType = ClaimTypes.Role
                 };
             });
        }
    }
}
