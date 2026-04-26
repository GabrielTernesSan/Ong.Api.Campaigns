using Microsoft.EntityFrameworkCore;
using Ong.Domain;
using Ong.Domain.Repositories;

namespace Ong.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OngDbContext _context;

        public UserRepository(OngDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var entity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            return entity == null
                ? null
                : new User(entity.Id, entity.Name, entity.Email, entity.PasswordHash, entity.Role);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            return entity == null
                ? null
                : new User(entity.Id, entity.Name, entity.Email, entity.PasswordHash, entity.Role);
        }

        public async Task CreateAsync(User user)
        {
            var entity = new Tables.User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Role = user.Role
            };

            _context.Users.Add(entity);

            await _context.SaveChangesAsync();
        }
    }
}
