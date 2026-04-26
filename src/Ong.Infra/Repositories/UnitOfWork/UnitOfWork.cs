using Ong.Domain.Repositories.UnitOfWork;

namespace Ong.Infra.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OngDbContext _context;

        public UnitOfWork(OngDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
