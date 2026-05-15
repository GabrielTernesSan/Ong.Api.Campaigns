using MediatR;
using Ong.Application.Requests;
using Ong.Commom;
using Ong.Domain.Enums;
using Ong.Domain.Repositories;
using Ong.Domain.Repositories.UnitOfWork;

namespace Ong.Application.Handlers
{
    public class DonationReceivedHandler : IRequestHandler<DonationReceivedRequest, Response>
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IDonationRepository _donationRepository;

        public DonationReceivedHandler(ICampaignRepository campaignRepository, IDonationRepository donationRepository)
        {
            _campaignRepository = campaignRepository;
            _donationRepository = donationRepository;
        }

        public async Task<Response> Handle(DonationReceivedRequest request, CancellationToken cancellationToken)
        {
            var response = new Response();

            var campaign = await _campaignRepository.GetByIdAsync(request.CampaignId);

            if (campaign == null)
                return response.AddError("Campanha não encontrada.");

            if (campaign.Status == ECampaignStatus.Canceled || campaign.Status == ECampaignStatus.Completed)
                return response;

            var totalRaised = await _donationRepository.GetTotalAmountByCampaignIdAsync(request.CampaignId);

            if (totalRaised >= campaign.FinancialGoal)
            {
                campaign.MarkAsCompleted();
                await _campaignRepository.UpdateAsync(campaign);
            }

            return response;
        }
    }
}
