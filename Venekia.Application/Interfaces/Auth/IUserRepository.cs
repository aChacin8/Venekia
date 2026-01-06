using System.Threading.Tasks;

using Venekia.Domain.Entities;


namespace Venekia.Application.Interfaces.Auth
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task UpdateAsync (User user);
        Task <User?> GetByIdAsync (Guid Id);
        Task <User?> GetByEmailAsync (string email);
    }
}   