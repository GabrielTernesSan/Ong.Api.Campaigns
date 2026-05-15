using Ong.Domain.Queries.Responses;

namespace Ong.Domain.Queries
{
    public interface IOutboxMessageQuery
    {
        Task<IEnumerable<OutboxMessageResponse>> ObterOutboxMessagesPendentesAsync();
    }
}
