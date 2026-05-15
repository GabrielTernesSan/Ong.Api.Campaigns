using FluentValidation;
using MediatR;
using Ong.Commom;

namespace Ong.Application.Requests
{
    public class DonationReceivedRequest : IRequest<Response>
    {
        public Guid CampaignId { get; set; }
        public decimal Amount { get; set; }
    }

    public class DonationReceivedRequestValidator : AbstractValidator<DonationReceivedRequest>
    {
        public DonationReceivedRequestValidator()
        {
            RuleFor(x => x.CampaignId).NotEmpty().WithMessage("O ID da campanha é obrigatório.");

            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("O valor da doação deve ser maior que zero.");
        }
    }

}
