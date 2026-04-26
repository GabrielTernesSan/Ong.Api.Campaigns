using MediatR;

namespace Ong.Commom
{
    public class DonationCreated : INotification
    {
        public Guid DonationId { get; set; }
        public Guid UserId { get; set; }
        public Guid CampaignId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }

        public DonationCreated(Guid donationId, Guid userId, Guid campaignId, decimal amount, DateTime timestamp)
        {
            DonationId = donationId;
            UserId = userId;
            CampaignId = campaignId;
            Amount = amount;
            Timestamp = timestamp;
        }
    }
}
