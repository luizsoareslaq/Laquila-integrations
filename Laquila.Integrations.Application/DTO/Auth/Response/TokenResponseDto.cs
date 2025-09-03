using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Application.DTO.Auth.Response
{
    public class TokenResponseDto
    {
        public TokenResponseDto(string jwtToken, DateTime jwtTokenExpirationDate, string refreshToken, DateTime refreshTokenExpirationDate)
        {
            this.JWTToken = jwtToken;
            this.JWTTokenExpirationDate = jwtTokenExpirationDate;
            this.RefreshToken = refreshToken;
            this.RefreshTokenExpirationDate = refreshTokenExpirationDate;
        }
        public string JWTToken { get; set; }
        public DateTime JWTTokenExpirationDate { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationDate { get; set; }
    }
}