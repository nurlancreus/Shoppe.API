﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shoppe.Application.Abstractions.Services.Token;
using Shoppe.Application.DTOs.Token;
using Shoppe.Application.Options.Token;
using Shoppe.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Options = Shoppe.Application.Options.Token;

namespace Shoppe.Infrastructure.Concretes.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly Options.TokenOptions _tokenOptions;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public TokenService(IOptions<Options.TokenOptions> tokenOptions, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _tokenOptions = tokenOptions.Value;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<TokenDTO> CreateAccessTokenAsync(ApplicationUser appUser)
        {
            var lifeTime = _tokenOptions.Access.AccessTokenLifeTimeInMinutes;

            TokenDTO token = new()
            {
                ExpiresAt = DateTime.UtcNow.AddMinutes(lifeTime),
            };

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_tokenOptions.Access.SecurityKey));

            // Create the encrypted credentials.
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, appUser.UserName!), // Name claim (username)
                new(ClaimTypes.GivenName, appUser.FirstName!),
                new(ClaimTypes.Surname, appUser.LastName!),
                new(JwtRegisteredClaimNames.Sub, appUser.Id.ToString()), // Subject (user id)
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT unique ID (JTI)
                new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString()), // Issued at (Unix timestamp)
                new(ClaimTypes.NameIdentifier, appUser.Id), // Unique name identifier of the user (id)
                new(ClaimTypes.Email, appUser.Email!) // Email of the user
            };

            // Add role claims for the user.
            var userRoles = await _userManager.GetRolesAsync(appUser);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Set the token's configurations.
            JwtSecurityToken securityToken = new(
                audience: _tokenOptions.Access.Audience,
                issuer: _tokenOptions.Access.Issuer,
                expires: token.ExpiresAt,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: claims
            );

            // Create an instance of the token handler class.
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            // Generate the refresh token.
            token.RefreshToken = await CreateRefreshTokenAsync();

            return token;
        }

        public async Task<string> CreateRefreshTokenAsync()
        {
            return await Task.Run(() =>
              {
                  byte[] number = new byte[64];

                  using RandomNumberGenerator random = RandomNumberGenerator.Create();
                  random.GetBytes(number);

                  return Convert.ToBase64String(number);
              });
        }

        public ClaimsPrincipal? GetPrincipalFromAccessToken(string? accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _configuration[_tokenOptions.Access.Audience],
                ValidateIssuer = true,
                ValidIssuer = _configuration[_tokenOptions.Access.Issuer],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.Access.SecurityKey)),

                ValidateLifetime = false //should be false
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

            ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
