using Microsoft.AspNetCore.Http;
using Shoppe.Application.DTOs.Auth;
using Shoppe.Application.DTOs.Token;
using Shoppe.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Auth
{
    public interface IAuthService : IInternalAuthService
    {
        Task<TokenDTO> RegisterAsync(RegisterRequestDTO registerRequest);
        Task UpdateRefreshTokenAsync(string refreshToken, ApplicationUser user, DateTime accessTokenLifeTime);
    }
}
