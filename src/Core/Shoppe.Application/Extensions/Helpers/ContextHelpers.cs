using Microsoft.AspNetCore.Http;
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

        public static ClaimsPrincipal GetUser(IHttpContextAccessor httpContext)
        {
            var user = httpContext.HttpContext?.User;

            return user ?? throw new AuthException(AuthErrorType.Authorize);
        }
    }
}
