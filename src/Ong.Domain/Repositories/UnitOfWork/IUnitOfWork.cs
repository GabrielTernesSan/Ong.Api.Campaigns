namespace Ong.Domain.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken);
    }
}
