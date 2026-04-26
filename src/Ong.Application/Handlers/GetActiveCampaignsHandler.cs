using MediatR;
using Ong.Application.Requests;
using Ong.Commom;
using Ong.Domain.Repositories;
using Ong.Domain.Repositories.UnitOfWork;

namespace Ong.Application.Handlers
{
    public class GetActiveCampaignsHandler : IRequestHandler<GetActiveCampaignsRequest, Response>
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IDonationRepository _donationRepository;

        public GetActiveCampaignsHandler(ICampaignRepository campaignRepository, IDonationRepository donationRepository)
        {
            _campaignRepository = campaignRepository;
            _donationRepository = donationRepository;
        }

        public async Task<Response> Handle(GetActiveCampaignsRequest request, CancellationToken cancellationToken)
        {
            var response = new Response();

            var campaigns = (await _campaignRepository.GetActiveAsync()).ToList();
            var totalAmounts = await _donationRepository.GetTotalAmountsByCampaignIdsAsync(campaigns.Select(campaign => campaign.Id));

            response.SetResult(campaigns.Select(c => new
            {
                c.Id,
                c.Title,
                c.FinancialGoal,
                TotalDonated = totalAmounts.TryGetValue(c.Id, out var totalDonated) ? totalDonated : 0m
            }));

            return response;
        }
    }
}