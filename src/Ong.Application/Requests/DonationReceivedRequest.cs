using MediatR;
using Ong.Commom;

namespace Ong.Application.Requests
{
    public class DonationReceivedRequest : IRequest<Response>
    {
        public Guid CampaignId { get; set; }
        public decimal Amount { get; set; }
    }
}
