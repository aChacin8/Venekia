using Venekia.Application.DTOs.Security;

namespace Venekia.Application.Interfaces.Auth
{
    public interface IJwtService
    {
        string GenerateToken (UserClaims userClaims);
        UserClaims VerifyToken (string token);
    }
}