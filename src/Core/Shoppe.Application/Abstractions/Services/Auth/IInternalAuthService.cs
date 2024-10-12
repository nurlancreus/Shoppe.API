using Microsoft.AspNetCore.Http;
using Shoppe.Application.DTOs.Auth;
using Shoppe.Application.DTOs.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Services.Auth
{
    public interface IInternalAuthService
    {
        Task<TokenDTO> LoginAsync(LoginRequestDTO loginUserRequestDTO);
        Task<TokenDTO> RefreshTokenLoginAsync(string accessToken, string refreshToken);
    }
}
