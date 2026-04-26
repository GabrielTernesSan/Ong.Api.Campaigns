using MediatR;
using Ong.Application.Requests;
using Ong.Commom;
using Ong.Domain;
using Ong.Domain.Repositories;

namespace Ong.Application.Handlers
{
    public class CreateCampaignHandler : IRequestHandler<CreateCampaignRequest, Response>
    {
        private readonly ICampaignRepository _campaignRepository;

        public CreateCampaignHandler(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;
        }

        public async Task<Response> Handle(CreateCampaignRequest request, CancellationToken cancellationToken)
        {
            var response = new Response();

            var campaign = Campaign.Create(
                request.Title,
                request.Description,
                request.StartDate,
                request.EndDate,
                request.FinancialGoal
            );

            await _campaignRepository.AddAsync(campaign);

            response.SetResult(campaign.Id);

            return response;
        }
    }
}
