using FluentValidation;
using MediatR;
using Ong.Commom;

namespace Ong.Application.Requests
{
    public class CreateCampaignRequest : IRequest<Response>
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public decimal FinancialGoal { get; set; }
    }

    public class CreateCampignRequestValidator : AbstractValidator<CreateCampaignRequest>
    {
        public CreateCampignRequestValidator()
        {
            RuleFor(x => x.Title).NotNull().WithMessage("O título é obrigatório.")
                .MaximumLength(100).WithMessage("O título deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Description).NotNull().WithMessage("A descrição é obrigatória.")
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.");

            RuleFor(x => x.StartDate).NotNull().WithMessage("A data de início é obrigatória.")
                .LessThan(x => x.EndDate).WithMessage("A data de início deve ser anterior à data de término.");

            RuleFor(x => x.EndDate).NotNull().WithMessage("A data de término é obrigatória.");

            RuleFor(x => x.FinancialGoal).GreaterThan(0).WithMessage("A meta financeira deve ser maior que zero.");
        }
    }
}
