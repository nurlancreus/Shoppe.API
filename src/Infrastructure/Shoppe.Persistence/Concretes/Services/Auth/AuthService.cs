using Microsoft.AspNetCore.Http; // Add this for IHttpContextAccessor
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shoppe.Application.Abstractions.Services.Auth;
using Shoppe.Application.Abstractions.Services.Token;
using Shoppe.Application.DTOs.Auth;
using Shoppe.Application.DTOs.Token;
using Shoppe.Domain.Entities.Identity;
using Shoppe.Domain.Exceptions;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Persistence.Concretes.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly HttpContext? _httpContext;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<TokenDTO> RegisterAsync(RegisterRequestDTO registerRequest)
        {
            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                throw new AuthException(AuthErrorType.Register);
            }

            ApplicationUser user = new()
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                UserName = registerRequest.UserName,
                Email = registerRequest.Email
            };

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var result = await _userManager.CreateAsync(user, registerRequest.Password);
                    if (!result.Succeeded)
                    {
                        throw new AuthException(AuthErrorType.Register);
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    var token = await _tokenService.CreateAccessTokenAsync(user);
                    await UpdateRefreshTokenAsync(token.RefreshToken, user, token.ExpiresAt);

                    // Save tokens in cookies
              //      SaveTokensInCookies(_httpContext, token);

                    transaction.Complete();

                    return token;
                }
            }
            catch (Exception)
            {
                if (user.Id != null)
                {
                    await _userManager.DeleteAsync(user);
                }

                throw;
            }
        }

        public async Task<TokenDTO> LoginAsync(LoginRequestDTO loginUserRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginUserRequestDTO.Email)
                ?? throw new AuthException(AuthErrorType.Login);

            var signInResult = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                loginUserRequestDTO.Password,
                loginUserRequestDTO.RememberMe,
                lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                throw new AuthException(AuthErrorType.Login);
            }

            var token = await _tokenService.CreateAccessTokenAsync(user);
            await UpdateRefreshTokenAsync(token.RefreshToken, user, token.ExpiresAt);

            // Save tokens in cookies
           // SaveTokensInCookies(_httpContext, token);

            return token;
        }

        private static void SaveTokensInCookies(HttpContext? httpContext, TokenDTO token)
        {
            if (httpContext != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, // Prevents client-side scripts from accessing the cookie
                    //Secure = true,   // Set to true if using HTTPS
                    Expires = DateTime.UtcNow.AddDays(1), // Set the cookie expiration to the token expiration
                    SameSite = SameSiteMode.None,
                };

                // Set access token and refresh token in cookies
                httpContext.Response.Cookies.Append("AccessToken", token.AccessToken, cookieOptions);
                httpContext.Response.Cookies.Append("RefreshToken", token.RefreshToken, cookieOptions);
            }
        }

        public async Task<TokenDTO> RefreshTokenLoginAsync(string accessToken, string refreshToken)
        {
            ClaimsPrincipal? principal = _tokenService.GetPrincipalFromAccessToken(accessToken)
                ?? throw new Exception("Invalid jwt access token");

            string? name = principal.FindFirstValue(ClaimTypes.Name);
            ApplicationUser? user = await _userManager.FindByNameAsync(name);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenEndDate <= DateTime.UtcNow)
            {
                throw new Exception("Invalid refresh token");
            }

            var token = await _tokenService.CreateAccessTokenAsync(user);
            await UpdateRefreshTokenAsync(token.RefreshToken, user, token.ExpiresAt);

            return token;
        }

        public async Task UpdateRefreshTokenAsync(string refreshToken, ApplicationUser user, DateTime accessTokenLifeTime)
        {
            if (user != null)
            {
                double refreshTokenLifeTime = Convert.ToDouble(_configuration["Token:Refresh:RefreshTokenLifeTimeInMinutes"]);

                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenLifeTime.AddMinutes(refreshTokenLifeTime);

                await _userManager.UpdateAsync(user);
            }
            else
                throw new EntityNotFoundException(nameof(user), "User not found to update refresh token");
        }
    }
}
