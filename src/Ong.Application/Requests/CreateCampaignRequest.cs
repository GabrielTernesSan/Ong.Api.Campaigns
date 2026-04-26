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
}
