using MediatR;
using Ong.Application.Requests;
using Ong.Commom;
using Ong.Domain.Repositories;

namespace Ong.Application.Handlers
{
    public class UpdateCampaignHandler : IRequestHandler<UpdateCampaignRequest, Response>
    {
        private readonly ICampaignRepository _campaignRepository;

        public UpdateCampaignHandler(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;
        }

        public async Task<Response> Handle(UpdateCampaignRequest request, CancellationToken cancellationToken)
        {
            var response = new Response();

            var campaign = await _campaignRepository.GetByIdAsync(request.Id);

            if (campaign == null)
                return response.AddError("Campanha não encontrada.");

            campaign.UpdateTitle(request.Title);
            campaign.UpdateDescription(request.Description);
            campaign.UpdateDates(request.StartDate, request.EndDate);
            campaign.UpdateFinancialGoal(request.FinancialGoal);
            campaign.UpdateStatus(request.Status);

            await _campaignRepository.UpdateAsync(campaign);

            return response;
        }
    }
}
