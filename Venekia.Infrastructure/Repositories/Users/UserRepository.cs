using Microsoft.EntityFrameworkCore;

using Venekia.Domain.Entities.Users;
using Venekia.Infrastructure.Data;
using Venekia.Application.Interfaces.Users;


namespace Venekia.Infrastructure.Repositories.Users
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

        public async Task DeactivateUserAsync(Guid id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id)
                ?? throw new Exception("User not found");

            user.Deactivate();

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}