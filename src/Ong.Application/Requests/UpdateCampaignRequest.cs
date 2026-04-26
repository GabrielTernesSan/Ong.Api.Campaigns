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
}