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

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            return entity == null
                ? null
                : new User(entity.Id, entity.Name, entity.Email);
        }

        public async Task CreateAsync(User user)
        {
            var entity = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (entity != null)
            {
                entity.Email = user.Email;
            }
            else
            {
                entity = new Tables.User
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                };

                _context.Users.Add(entity);
            }

            await _context.SaveChangesAsync();
        }
    }
}
