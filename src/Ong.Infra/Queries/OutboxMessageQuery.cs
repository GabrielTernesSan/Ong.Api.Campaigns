using Microsoft.EntityFrameworkCore;
using Ong.Domain.Queries;
using Ong.Domain.Queries.Responses;

namespace Ong.Infra.Queries
{
    public class OutboxMessageQuery : IOutboxMessageQuery
    {
        private readonly OngDbContext _context;

        public OutboxMessageQuery(OngDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OutboxMessageResponse>> ObterOutboxMessagesPendentesAsync()
        {
            return await _context.OutboxMessages
                .Where(m => m.ProcessedOn == null && m.RetryCount < 3)
                .Select(m => new OutboxMessageResponse
                {
                    Id = m.Id,
                    Type = m.Type,
                    Payload = m.Payload,
                    CreatedOn = m.OccurredOn,
                    ProcessedOn = m.ProcessedOn,
                    Error = m.Error
                }).ToListAsync();
        }
    }
}
