using Microsoft.AspNetCore.Http;
using Shoppe.Application.Abstractions.Services.Session;
using Shoppe.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Shoppe.Infrastructure.Concretes.Services.Session
{
    public class JwtSession : IJwtSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext? _httpContext;

        public JwtSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
        }

        public List<string> GetRoles()
        {
            if (_httpContext?.User == null || (!_httpContext.User.Identity?.IsAuthenticated ?? false))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            return _httpContext.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();
        }

        public ApplicationUser GetUser()
        {
            if (_httpContext?.User == null || (!_httpContext.User.Identity?.IsAuthenticated ?? false))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userIdClaim = _httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var firstNameClaim = _httpContext.User.FindFirst(ClaimTypes.GivenName)?.Value;
            var lastNameClaim = _httpContext.User.FindFirst(ClaimTypes.Surname)?.Value;
            var emailClaim = _httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var userNameClaim = _httpContext.User.FindFirst(ClaimTypes.Name)?.Value;

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

        public string GetUserEmail()
        {
            if (_httpContext?.User == null || (!_httpContext.User.Identity?.IsAuthenticated ?? false))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var email = _httpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                throw new UnauthorizedAccessException("User email is missing in the claims.");
            }

            return email;
        }

        public string GetUserId()
        {
            if (_httpContext?.User == null || (!_httpContext.User.Identity?.IsAuthenticated ?? false))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userId = _httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User ID is missing in the claims.");
            }

            return userId;
        }

        public string GetUserName()
        {
            if (_httpContext?.User == null || (!_httpContext.User.Identity?.IsAuthenticated ?? false))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userName = _httpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                throw new UnauthorizedAccessException("User name is missing in the claims.");
            }

            return userName;
        }

        public bool IsAdmin()
        {
            var userRoles = GetRoles();

            if (userRoles.Count > 0)
            {
                return userRoles.Contains("Admin") || userRoles.Contains("SuperAdmin");
            }

            return false;
        }

        public bool IsAuthenticated(bool throwException = false)
        => !throwException ? _httpContext?.User?.Identity?.IsAuthenticated ?? false : throw new UnauthorizedAccessException("User is not authenticated.");

        public bool IsSuperAdmin()
        {
            var userRoles = GetRoles();

            if (userRoles.Count > 0)
            {
                return userRoles.Contains("SuperAdmin");
            }

            return false;
        }

        public void ValidateSuperAdminAccess()
        {
            if (!IsSuperAdmin())
                throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        }

        public void ValidateAdminAccess()
        {
            if (!IsAdmin())
                throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        }

        public void ValidateAuthAccess(string id)
        {
            if (GetUserId() != id)
                throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        }

        public void ValidateRoleAccess(IEnumerable<string> roles)
        {
            bool roleExist = false;

            foreach (var role in roles)
            {
                if (GetRoles().Contains(role))
                {
                    roleExist = true;
                    break;
                }
            }

            if (!roleExist)
                throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        }


    }
}
