using MediatR;
using Ong.Application.Requests;
using Ong.Domain.Repositories.UnitOfWork;
using System.Text.Json;

namespace Ong.Application.Handlers
{
    public class ProcessOutboxHandler : IRequestHandler<ProcessOutboxRequest>
    {
        private readonly IOutboxMessageRepository _outboxRepository;
        private readonly IUnitOfWork _unitRepository;
        private readonly IMediator _mediator;

        public ProcessOutboxHandler(IOutboxMessageRepository outboxRepository, IMediator mediator, IUnitOfWork unitRepository)
        {
            _outboxRepository = outboxRepository;
            _mediator = mediator;
            _unitRepository = unitRepository;
        }

        public async Task Handle(ProcessOutboxRequest request, CancellationToken cancellationToken)
        {
            var messages = await _outboxRepository.GetUnprocessedAsync();

            foreach (var message in messages)
            {
                try
                {
                    var type = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .FirstOrDefault(t => t.Name == message.Type);

                    var payload = JsonSerializer.Deserialize(message.Payload, type!);

                    await _mediator.Publish(payload!, cancellationToken);

                    await _outboxRepository.MarkAsProcessedAsync(message.Id, DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    await _outboxRepository.MarkAsErrorAsync(message.Id, ex.Message);
                }
            }

            await _unitRepository.CommitAsync(cancellationToken);
        }
    }
}
