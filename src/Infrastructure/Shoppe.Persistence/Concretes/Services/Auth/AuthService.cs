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
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Shoppe.Persistence.Concretes.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly Shoppe.Application.Options.Token.TokenOptions _tokenOptions;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService, SignInManager<ApplicationUser> signInManager, IOptions<Shoppe.Application.Options.Token.TokenOptions> tokenOptions, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _tokenOptions = tokenOptions.Value;
            _configuration = configuration;
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
                    // Create user
                    var result = await _userManager.CreateAsync(user, registerRequest.Password);
                    if (!result.Succeeded)
                    {
                        throw new AuthException(AuthErrorType.Register);
                    }

                    // Sign in user
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Create token
                    var token = await _tokenService.CreateAccessTokenAsync(user);

                    // Update refresh token
                    await UpdateRefreshTokenAsync(token.RefreshToken, user, token.ExpiresAt);

                    // Complete transaction
                    transaction.Complete();

                    return token;
                }
            }
            catch (Exception)
            {
                // If something fails, remove the created user
                if (user.Id != null)
                {
                    await _userManager.DeleteAsync(user);
                }

                throw; // Re-throw the exception after cleanup
            }
        }

        public async Task<TokenDTO> LoginAsync(LoginRequestDTO loginUserRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginUserRequestDTO.Email)
           ?? throw new AuthException(AuthErrorType.Login);
            var signInResult = await _signInManager.PasswordSignInAsync(user.UserName!, loginUserRequestDTO.Password, loginUserRequestDTO.RememberMe, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                throw new AuthException(AuthErrorType.Login);
            }

            var token = await _tokenService.CreateAccessTokenAsync(user);

            await UpdateRefreshTokenAsync(token.RefreshToken, user, token.ExpiresAt);

            return token;
        }

        public async Task<TokenDTO> RefreshTokenLoginAsync(string accessToken, string refreshToken)
        {
            ClaimsPrincipal? principal = _tokenService.GetPrincipalFromAccessToken(accessToken) ?? throw new Exception("Invalid jwt access token");

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
                //double refreshTokenLifeTime = Convert.ToDouble(_tokenOptions.Refresh.RefreshTokenLifeTimeInMinutes);
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
