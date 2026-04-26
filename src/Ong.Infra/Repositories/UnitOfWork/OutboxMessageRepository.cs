using Microsoft.EntityFrameworkCore;
using Ong.Domain;
using Ong.Domain.Repositories.UnitOfWork;

namespace Ong.Infra.Repositories.UnitOfWork
{
    public class OutboxMessageRepository : IOutboxMessageRepository
    {
        private readonly OngDbContext _context;

        public OutboxMessageRepository(OngDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(OutboxMessage message)
        {
            var entity = new Tables.OutboxMessage
            {
                Id = message.Id,
                Type = message.Type,
                Payload = message.Payload,
                OccurredOn = message.CreatedOn,
                ProcessedOn = message.ProcessedOn,
                Error = message.Error
            };

            await _context.OutboxMessages.AddAsync(entity);
        }

        public async Task MarkAsProcessedAsync(Guid id, DateTime processedOn, string? error = null)
        {
            var entity = await _context.OutboxMessages.FindAsync(id);

            if (entity != null)
            {
                entity.ProcessedOn = processedOn;
                entity.Error = error;

                _context.OutboxMessages.Update(entity);
            }
        }

        public async Task<IEnumerable<OutboxMessage>> GetUnprocessedAsync()
        {
            var entities = await _context.OutboxMessages
                .Where(m => m.ProcessedOn == null)
                .ToListAsync();

            return entities.Select(e => new OutboxMessage(
                e.Id,
                e.Type,
                e.Payload,
                e.OccurredOn,
                e.ProcessedOn,
                e.Error
            ));
        }

        public async Task MarkAsErrorAsync(Guid id, string error)
        {
            var entity = await _context.OutboxMessages.FindAsync(id);

            if (entity != null)
            {
                entity.Error = error;

                _context.OutboxMessages.Update(entity);
            }
        }
    }
}
