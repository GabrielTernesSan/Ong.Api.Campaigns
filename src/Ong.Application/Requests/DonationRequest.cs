using FluentValidation;
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

    public class DonationRequestValidator : AbstractValidator<DonationRequest>
    {
        public DonationRequestValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("O valor da doação deve ser maior que zero.");
        }
    }
}
