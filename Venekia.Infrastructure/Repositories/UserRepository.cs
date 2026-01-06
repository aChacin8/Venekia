using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using Venekia.Domain.Entities;
using Venekia.Infrastructure.Data;
using Venekia.Application.Interfaces.Auth;


namespace Venekia.Infrastructure.Repositories.Auth
{
    public class UserRepository : IUserRepository
    {
        private readonly VenekiaDb _context;

        public UserRepository(VenekiaDb context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync (User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync (string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email); //SingleOrDefault, para mantener unicidad, es decir email unico.
        }

        public async Task<User?> GetByIdAsync (Guid Id)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Id == Id);
        }
    }
}