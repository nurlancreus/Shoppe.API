using Microsoft.AspNetCore.Http;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Enums;
using Shoppe.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Extensions.Helpers
{
    public static class ContextHelpers
    {
        public static SortOption? ParseSortOption(string? sortBy)
        {
            if (string.IsNullOrEmpty(sortBy)) return null;

            return sortBy.ToLower() switch
            {
                "price-asc" => SortOption.PriceAsc,
                "price-desc" => SortOption.PriceDesc,
                "createdat-asc" => SortOption.CreatedAtAsc,
                "createdat-desc" => SortOption.CreatedAtDesc,
                _ => null
            };
        }

        public static ApplicationUser GetCurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext?.User == null || (!httpContext.User.Identity?.IsAuthenticated ?? false))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            // Retrieve claims from the current user's identity
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var firstNameClaim = httpContext.User.FindFirst(ClaimTypes.GivenName)?.Value;
            var lastNameClaim = httpContext.User.FindFirst(ClaimTypes.Surname)?.Value;
            var emailClaim = httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var userNameClaim = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID is missing in the claims.");
            }

            return new ApplicationUser
            {
                Id = userIdClaim,
                FirstName = firstNameClaim,
                LastName = lastNameClaim,
                Email = emailClaim,
                UserName = userNameClaim,
            };
        }

        public static bool IsAdmin(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;

            if ((httpContext?.User == null) || (!httpContext.User.Identity?.IsAuthenticated ?? false))
            {
                return false;
            }

            return httpContext.User.IsInRole("Admin");
        }

    }
}
