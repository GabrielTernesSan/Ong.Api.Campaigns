using MediatR;
using Ong.Commom;
using System.Text.Json.Serialization;

namespace Ong.Application.Requests
{
    public class DonationRequest : IRequest<Response>
    {
        [JsonIgnore]
        public Guid CampaignId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
