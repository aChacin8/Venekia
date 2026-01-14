using Venekia.Domain.Entities.Users;


namespace Venekia.Application.Interfaces.Users
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task UpdateAsync (User user);
        Task <User?> GetByIdAsync (Guid Id);
        Task <User?> GetByEmailAsync (string email);
        Task DeactivateUserAsync(Guid Id);
    }
}   