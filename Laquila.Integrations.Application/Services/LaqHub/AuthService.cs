using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Laquila.Integrations.Application.DTO.Auth.Request;
using Laquila.Integrations.Application.DTO.Auth.Response;
using Laquila.Integrations.Application.Interfaces.LaqHub;
using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Localization;
using Laquila.Integrations.Core.Shared;
using Laquila.Integrations.Domain.Enums;
using Laquila.Integrations.Domain.Helpers;
using Laquila.Integrations.Domain.Interfaces.Repositories.LaqHub;
using Laquila.Integrations.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static Laquila.Integrations.Application.Exceptions.ApplicationException;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Laquila.Integrations.Application.Services.LaqHub
{
    public class AuthService : IAuthService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly string lang = UserContext.Language ?? "en";

        public AuthService(IHttpContextAccessor httpContextAccessor
                         , IConfiguration configuration
                         , IUserRepository userRepository
                         , IAuthRepository authRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? throw new NotFoundException("JWT_SECRET_KEY não encontrado.");
            _issuer = Environment.GetEnvironmentVariable("ISSUER") ?? throw new NotFoundException("ISSUER não encontrado.");
            _audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? throw new NotFoundException("AUDIENTE não encontrado.");
            _userRepository = userRepository;
            _authRepository = authRepository;
        }

        public async Task<TokenResponseDto> DoLoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepository.GetUserByUsername(dto.Username);

            if (!VerifyPassword(dto.Password, user.Hash, user.Salt) || user.StatusId != (int)ApiStatusEnum.Active)
            {
                throw new UnauthorizedAccessException(MessageProvider.Get("InvalidLogin", lang));
            }

            var companyCnpj = CheckCompany(dto.Cnpj, user);

            var token = await GenerateToken(user, companyCnpj, user.Language);

            if(companyCnpj == null)
                throw new UnauthorizedAccessException(MessageProvider.Get("UserCompanyNotFound", lang));

            return token;
        }

        public async Task<TokenResponseDto> GenerateToken(LaqApiUsers user, string companyCnpj, string language = "en")
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim("CompanyCnpj", companyCnpj),
                new Claim("Language", language),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(user.UserRoles.Select(x => new Claim(ClaimTypes.Role, x.Role.RoleName)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            int expirationSeconds = 3600;
            var expirationJwt = DateTime.Now.AddSeconds(expirationSeconds);

            var accessToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expirationJwt,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                throw new Exceptions.ApplicationException.BadRequestException(MessageProvider.Get("GeneratingTokenError", lang));

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

            var newRefreshToken = DomainHelpers.GenerateSecureRefreshToken();
            var expirationRefreshToken = DateTime.Now.AddDays(7);


            await _authRepository.SaveTokenAsync(new LaqApiAuthTokens(user.Id
                                                                    , tokenString
                                                                    , newRefreshToken
                                                                    , expirationJwt
                                                                    , expirationRefreshToken
                                                                    , companyCnpj));

            return new TokenResponseDto(tokenString, expirationSeconds);
        }

        public Guid GetIdByJwt()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context.Request.Cookies.TryGetValue("jwt", out var token))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return userId;
                }
            }

            return Guid.Empty;
        }

        public string GetNameByJwt()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context?.Request?.Cookies.TryGetValue("jwt", out var token) == true)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var loginClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName);

                if (loginClaim != null)
                {
                    return loginClaim.Value;
                }
            }

            return string.Empty;
        }

        public string GetRoleByJwt()
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

            return string.Empty;
        }

        public ClaimsPrincipal ValidateToken(string token)
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
                throw new Exception(MessageProvider.Get("InvalidToken", lang), ex);
            }
        }

        public bool VerifyPassword(string password, string storedHash, string salt)
        {
            var hashToCompare = UserHelper.HashPassword(password, salt);
            return hashToCompare == storedHash;
        }

        public string CheckCompany(string? companyCnpj, LaqApiUsers user)
        {
            string selectedCnpj = string.Empty;

            if (user.UserCompanies.Count() == 0)
                throw new UnauthorizedAccessException(MessageProvider.Get("NoCompanyAssigned", user.Language));

            if (string.IsNullOrEmpty(companyCnpj))
            {
                if (user.UserCompanies.Count() > 1)
                    throw new Exceptions.ApplicationException.BadRequestException(MessageProvider.Get("MultipleCompaniesAssigned", user.Language));

                selectedCnpj = user.UserCompanies.First().Company.Document;

                return selectedCnpj;
            }
            else
            {
                var company = user.UserCompanies.FirstOrDefault(c => c.Company.Document == companyCnpj);
                if (company == null)
                    throw new UnauthorizedAccessException(MessageProvider.Get("UserCompanyNotFound", user.Language));

                selectedCnpj = company.Company.Document;

                return selectedCnpj;
            }            
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            var stored = await _authRepository.GetRefreshTokenAsync(dto.RefreshToken);

            if (stored == null || stored.RefreshTokenExpiresAt <= DateTime.Now)
                throw new UnauthorizedAccessException(MessageProvider.Get("InvalidRefreshToken", lang));

            var user = await _userRepository.GetUserById(stored.ApiUserId);

            await _authRepository.DeleteTokenAsync(stored);

            var token = await GenerateToken(user, stored.CompanyCnpj, user.Language);
            var newRefreshToken = DomainHelpers.GenerateSecureRefreshToken();

            return token;
        }
       
    }
}