namespace Ong.Domain.Repositories.UnitOfWork
{
    public interface IOutboxMessageRepository
    {
        Task CreateAsync(OutboxMessage message, CancellationToken cancellationToken);
        Task UpdateAsync(OutboxMessage message, CancellationToken cancellationToken);
        Task<OutboxMessage?> GetOutboxMessageByIdAsync(Guid id);
    }
}
