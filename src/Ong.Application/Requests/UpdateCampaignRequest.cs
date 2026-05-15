using FluentValidation;
using MediatR;
using Ong.Commom;
using Ong.Domain.Enums;

namespace Ong.Application.Requests
{
    public class UpdateCampaignRequest : IRequest<Response>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public decimal FinancialGoal { get; set; }
        public ECampaignStatus Status { get; set; }
    }

    public class UpdateCampaignRequestValidator : AbstractValidator<UpdateCampaignRequest>
    {
        public UpdateCampaignRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("O Id da campanha é obrigatório.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("O título da campanha é obrigatório.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("A descrição da campanha é obrigatória.");
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate).WithMessage("A data de início deve ser anterior à data de término.");
            RuleFor(x => x.EndDate).NotNull().WithMessage("A data de término da campanha é obrigatória.");
            RuleFor(x => x.FinancialGoal).GreaterThan(0).WithMessage("A meta financeira deve ser maior que zero.");
        }
    }
}