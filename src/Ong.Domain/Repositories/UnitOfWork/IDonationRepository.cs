namespace Ong.Domain.Repositories.UnitOfWork
{
    public interface IDonationRepository
    {
        Task<Donation?> GetByIdAsync(Guid id);
        Task CreateAsync(Donation donation);
        Task<decimal> GetTotalAmountByCampaignIdAsync(Guid campaignId);
        Task<Dictionary<Guid, decimal>> GetTotalAmountsByCampaignIdsAsync(IEnumerable<Guid> campaignIds);
    }
}
