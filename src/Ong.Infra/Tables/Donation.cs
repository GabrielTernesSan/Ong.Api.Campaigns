namespace Ong.Infra.Tables
{
    public class Donation
    {
        public Guid Id { get; set; }
        public Campaign Campaign { get; set; } = null!;
        public Guid CampaignId { get; set; }
        public User User { get; set; } = null!;
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
