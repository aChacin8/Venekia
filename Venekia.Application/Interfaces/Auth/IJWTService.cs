using Venekia.Application.DTOs.Auth;

namespace Venekia.Application.Interfaces.Auth
{
    public interface IJwtService
    {
        string GenerateToken (UserClaims userClaims);
    }
}