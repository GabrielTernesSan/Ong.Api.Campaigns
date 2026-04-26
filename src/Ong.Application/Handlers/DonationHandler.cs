using MediatR;
using Ong.Application.Requests;
using Ong.Commom;
using Ong.Domain;
using Ong.Domain.Enums;
using Ong.Domain.Repositories;
using Ong.Domain.Repositories.UnitOfWork;
using System.Text.Json;

namespace Ong.Application.Handlers
{
    public class DonationHandler : IRequestHandler<DonationRequest, Response>
    {
        private readonly IDonationRepository _donationRepository;
        private readonly IOutboxMessageRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICampaignRepository _campaignRepository;

        public DonationHandler(IDonationRepository donationRepository, IOutboxMessageRepository outboxRepository, IUnitOfWork unitOfWork, ICampaignRepository campaignRepository)
        {
            _donationRepository = donationRepository;
            _outboxRepository = outboxRepository;
            _unitOfWork = unitOfWork;
            _campaignRepository = campaignRepository;
        }

        public async Task<Response> Handle(DonationRequest request, CancellationToken cancellationToken)
        {
            var response = new Response();

            var campaign = await _campaignRepository.GetByIdAsync(request.CampaignId);

            if (campaign is null)
                return response.AddError("Campanha não encontrada.");

            if (campaign.IsCompleted() || campaign.Status == ECampaignStatus.Canceled)
                return response.AddError("Não é possível doar para campanhas encerradas ou canceladas.");

            var donationId = Guid.NewGuid();

            var donation = new Donation(
                donationId,
                request.CampaignId,
                request.UserId,
                request.Amount,
                DateTimeOffset.Now
            );

            var donationEvent = new DonationCreated(
                donationId,
                request.UserId,
                request.CampaignId,
                request.Amount,
                DateTime.UtcNow
            );

            var outboxMessage = new OutboxMessage(
                Guid.NewGuid(),
                typeof(DonationCreated).Name!,
                JsonSerializer.Serialize(donationEvent),
                DateTime.UtcNow
            );

            await _donationRepository.CreateAsync(donation);

            await _outboxRepository.CreateAsync(outboxMessage);

            await _unitOfWork.CommitAsync(cancellationToken);

            return response;
        }
    }
}
