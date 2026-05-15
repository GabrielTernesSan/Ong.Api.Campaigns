namespace Ong.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task CreateAsync(User user);
    }
}
