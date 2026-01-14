namespace Venekia.Application.DTOs.Security
{
    public class UserClaims
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = default!;
    }
}

