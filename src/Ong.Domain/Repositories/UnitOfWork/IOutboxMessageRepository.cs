namespace Ong.Domain.Repositories.UnitOfWork
{
    public interface IOutboxMessageRepository
    {
        Task CreateAsync(OutboxMessage message);
        Task MarkAsProcessedAsync(Guid id, DateTime processedOn, string? error = null);
        Task MarkAsErrorAsync(Guid id, string error);
        Task<IEnumerable<OutboxMessage>> GetUnprocessedAsync();
    }
}
