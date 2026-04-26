using Microsoft.EntityFrameworkCore;
using Ong.Domain;
using Ong.Domain.Repositories.UnitOfWork;

namespace Ong.Infra.Repositories.UnitOfWork
{
    public class DonationRepository : IDonationRepository
    {
        private readonly OngDbContext _context;

        public DonationRepository(OngDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Donation donation)
        {
            var entity = new Tables.Donation
            {
                Id = donation.Id,
                CampaignId = donation.CampaignId,
                UserId = donation.UserId,
                Amount = donation.Amount,
                Date = donation.Date.ToUniversalTime()
            };

            await _context.Donations.AddAsync(entity);
        }

        public async Task<Donation?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Donations.FindAsync(id);

            if (entity == null)
                return null;

            return new Donation(
                entity.Id,
                entity.CampaignId,
                entity.UserId,
                entity.Amount,
                entity.Date
            );
        }

        public async Task<decimal> GetTotalAmountByCampaignIdAsync(Guid campaignId)
        {
            return await _context.Donations
                .Where(donation => donation.CampaignId == campaignId)
                .Select(donation => (decimal?)donation.Amount)
                .SumAsync() ?? 0m;
        }

        public async Task<Dictionary<Guid, decimal>> GetTotalAmountsByCampaignIdsAsync(IEnumerable<Guid> campaignIds)
        {
            var ids = campaignIds.Distinct().ToList();

            if (ids.Count == 0)
                return new Dictionary<Guid, decimal>();

            return await _context.Donations
                .Where(donation => ids.Contains(donation.CampaignId))
                .GroupBy(donation => donation.CampaignId)
                .Select(group => new
                {
                    CampaignId = group.Key,
                    TotalAmount = group.Sum(donation => donation.Amount)
                })
                .ToDictionaryAsync(item => item.CampaignId, item => item.TotalAmount);
        }
    }
}
