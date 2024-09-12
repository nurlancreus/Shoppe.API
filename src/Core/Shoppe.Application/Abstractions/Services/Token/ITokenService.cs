using Shoppe.Application.DTOs.Token;
using Shoppe.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Token
{
    public interface ITokenService
    {
        Task<TokenDTO> CreateAccessTokenAsync(ApplicationUser user);
        Task<string> CreateRefreshTokenAsync();
        ClaimsPrincipal? GetPrincipalFromAccessToken(string? accessToken);
    }
}
