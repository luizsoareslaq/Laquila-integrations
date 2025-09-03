using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Laquila.Integrations.Application.DTO.Auth.Request;
using Laquila.Integrations.Application.DTO.Auth.Response;
using Laquila.Integrations.Application.Interfaces;
using Laquila.Integrations.Domain.Enums;
using Laquila.Integrations.Domain.Helpers;
using Laquila.Integrations.Domain.Interfaces.Repositories;
using Laquila.Integrations.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Laquila.Integrations.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        public AuthService(IHttpContextAccessor httpContextAccessor
                         , IConfiguration configuration
                         , IUserRepository userRepository
                         , IAuthRepository authRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _secretKey = configuration["Jwt:SecretKey"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _userRepository = userRepository;
            _authRepository = authRepository;
        }

        public async Task<TokenResponseDto> DoLoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepository.GetUserByUsername(dto.Username);

            if (!VerifyPassword(dto.Password, user.Hash, user.Salt) || user.StatusId != (int)ApiStatus.Active)
            {
                throw new UnauthorizedAccessException("Invalid login attemp.");
            }

            var token = await GenerateToken(user);

            return token;
        }

        public async Task<TokenResponseDto> GenerateToken(LaqApiUsers user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.Username),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _issuer),
                new Claim(JwtRegisteredClaimNames.Aud, _audience)
            };

            foreach (var role in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.RoleName));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expirationJwt = DateTime.Now.AddHours(1);

            var accessToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expirationJwt,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

            var context = _httpContextAccessor.HttpContext;
            if (context == null) throw new BadRequestException("Error while generating access token");

            var origin = context.Request.Headers["Origin"].ToString();

            var secureCookie = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = expirationJwt,
                Path = "/" 
            };

            var nonHttpOnlyCookie = new CookieOptions
            {
                HttpOnly = false,
                Secure = secureCookie.Secure,
                SameSite = secureCookie.SameSite,
                Expires = secureCookie.Expires
            };

            context.Response.Cookies.Append("jwt", tokenString, secureCookie);
            context.Response.Cookies.Append("userName", user.Username, nonHttpOnlyCookie);
            
            var newRefreshToken = GenerateSecureRefreshToken();
            var expirationRefreshToken = DateTime.Now.AddDays(7);


            await _authRepository.SaveTokenAsync(new LaqApiAuthTokens(user.Id, tokenString, newRefreshToken, expirationJwt, expirationRefreshToken));

            return new TokenResponseDto(tokenString,expirationJwt,newRefreshToken,expirationRefreshToken);
        }

        public async Task<Guid> GetIdByJwt()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context.Request.Cookies.TryGetValue("jwt", out var token))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return await Task.FromResult<Guid>(userId);
                }
            }

            return await Task.FromResult<Guid>(Guid.Empty);
        }

        public async Task<string> GetNameByJwt()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context?.Request?.Cookies.TryGetValue("jwt", out var token) == true)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var loginClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName);

                if (loginClaim != null)
                {
                    return await Task.FromResult(loginClaim.Value);
                }
            }

            return await Task.FromResult(string.Empty);
        }

        public async Task<string> GetRoleByJwt()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context?.Request?.Cookies.TryGetValue("jwt", out var token) == true)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var roleClaim = jwtToken.Claims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.Role);

                if (roleClaim != null)
                {
                    return roleClaim.Value;
                }
            }

            return await Task.FromResult(string.Empty);
        }


        public async Task<ClaimsPrincipal> ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return principal;
            }
            catch (Exception ex)
            {
                throw new Exception("Error when validating token", ex);
            }
        }

        public bool VerifyPassword(string password, string storedHash, string salt)
        {
            var hashToCompare = UserHelper.HashPassword(password, salt);
            return hashToCompare == storedHash;
        }

        public async Task<object> RefreshTokenAsync(string refreshToken)
        {
            var stored = await _authRepository.GetRefreshTokenAsync(refreshToken);

            if (stored == null || stored.RefreshTokenExpiresAt <= DateTime.Now)
                throw new UnauthorizedAccessException("Refresh Token is invalid or expired.");

            var user = await _userRepository.GetUserById(stored.Id);

            await _authRepository.DeleteTokenAsync(stored);

            var token = await GenerateToken(user); 
            var newRefreshToken = GenerateSecureRefreshToken();
            
            await _authRepository.SaveTokenAsync(new LaqApiAuthTokens(user.Id, token.JWTToken, newRefreshToken, token.JWTTokenExpirationDate, token.RefreshTokenExpirationDate));

            return token;
        }

        private string GenerateSecureRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}