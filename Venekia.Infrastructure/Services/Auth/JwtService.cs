using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Venekia.Application.Interfaces.Auth;
using Venekia.Application.DTOs.Security;

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

        public UserClaims VerifyToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                ValidateIssuer = true,
                ValidIssuer = _issuer,

                ValidateAudience = true,
                ValidAudience = _audience,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(
                    token,
                    validationParameters,
                    out _
                );

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var email = principal.FindFirst(ClaimTypes.Email)?.Value;

                if (userId is null || email is null)
                    throw new SecurityTokenException("Claims requeridos no encontrados");

                return new UserClaims
                {
                    Id = Guid.Parse(userId),
                    Email = email
                };
            }
            catch (SecurityTokenException ex)
            {
                throw new UnauthorizedAccessException("Token inválido o expirado", ex);
            }
        }
    }
}