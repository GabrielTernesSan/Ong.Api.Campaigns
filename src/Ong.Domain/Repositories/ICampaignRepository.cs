namespace Ong.Domain.Repositories
{
    public interface ICampaignRepository
    {
        Task<Campaign?> GetByIdAsync(Guid id);
        Task AddAsync(Campaign campaign);
        Task UpdateAsync(Campaign campaign);
        Task<IEnumerable<Campaign>> GetActiveAsync();
    }
}