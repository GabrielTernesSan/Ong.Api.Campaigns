using Microsoft.EntityFrameworkCore;
using Ong.Domain;
using Ong.Domain.Enums;
using Ong.Domain.Repositories;

namespace Ong.Infra.Repositories
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly OngDbContext _context;

        public CampaignRepository(OngDbContext context)
        {
            _context = context;
        }

        public async Task<Campaign?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Campaigns.FindAsync(id);

            if (entity == null)
                return null;

            return Campaign.Restore(
                entity.Id,
                entity.Title,
                entity.Description,
                entity.StartDate,
                entity.EndDate,
                entity.FinancialGoal,
                entity.Status);
        }

        public async Task AddAsync(Campaign campaign)
        {
            var entity = new Tables.Campaign
            {
                Id = campaign.Id,
                Title = campaign.Title,
                Description = campaign.Description,
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate,
                FinancialGoal = campaign.FinancialGoal,
                Status = campaign.Status
            };

            _context.Campaigns.Add(entity);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Campaign campaign)
        {
            var entity = await _context.Campaigns.FindAsync(campaign.Id);

            if (entity == null)
                return;

            entity.Title = campaign.Title;
            entity.Description = campaign.Description;
            entity.StartDate = campaign.StartDate;
            entity.EndDate = campaign.EndDate;
            entity.FinancialGoal = campaign.FinancialGoal;
            entity.Status = campaign.Status;

            _context.Campaigns.Update(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Campaign>> GetActiveAsync()
        {
            var entities = await _context.Campaigns
                .AsNoTracking()
                .Where(x => x.Status == ECampaignStatus.Active && x.EndDate >= DateTimeOffset.UtcNow)
                .ToListAsync();

            return entities.Select(entity => Campaign.Restore(
                entity.Id,
                entity.Title,
                entity.Description,
                entity.StartDate,
                entity.EndDate,
                entity.FinancialGoal,
                entity.Status));
        }
    }
}
