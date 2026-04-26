namespace Ong.Domain
{
    public class Donation
    {
        public Guid Id { get; }
        public Guid CampaignId { get; private set; }
        public Guid UserId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTimeOffset Date { get; private set; }

        public Donation(Guid campaignId, Guid userId, decimal amount, DateTimeOffset date)
        {
            CampaignId = campaignId;
            UserId = userId;
            Amount = amount;
            Date = date;
        }

        public Donation(Guid id, Guid campaignId, Guid userId, decimal amount, DateTimeOffset date)
            : this(campaignId, userId, amount, date)
        {
            Id = id;
        }
    }
}
