using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Venekia.Application.Interfaces.Auth;
using Venekia.Application.DTOs.Auth;

namespace Venekia.Infrastructure.Services.Auth
{
    public class JwtService : IJwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationMinutes;

        public JwtService(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:SecretKey"]!;
            _issuer = configuration["Jwt:Issuer"]!;
            _audience = configuration["Jwt:Audience"]!;
            var expirationString = configuration["Jwt:ExpirationMinutes"];

            if (!int.TryParse(expirationString, out var expirationMinutes))
            {
                throw new Exception("Jwt:ExpirationMinutes is missing or invalid");
            }

            _expirationMinutes = expirationMinutes;
        }

        public string GenerateToken(UserClaims userClaims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userClaims.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, userClaims.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}