using MediatR;
using Ong.Application.Requests;
using Ong.Commom;
using Ong.Domain.Repositories.UnitOfWork;

namespace Ong.Application.Handlers
{
    public class UpdateOutboxHandler : IRequestHandler<UpdateOutboxRequest, Response>
    {
        private readonly IOutboxMessageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOutboxHandler(IOutboxMessageRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(UpdateOutboxRequest request, CancellationToken cancellationToken)
        {
            var response = new Response();

            var message = await _repository.GetOutboxMessageByIdAsync(request.Id);

            if (message == null)
            {
                response.AddError("OutboxMessage não encontrada!");
                return response;
            }

            if (request.Error != null)
                message.ErrorOccurred(request.Error);
            else
                message.UpdateProcessedTime(DateTime.UtcNow);

            await _repository.UpdateAsync(message, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return response;
        }
    }
}
