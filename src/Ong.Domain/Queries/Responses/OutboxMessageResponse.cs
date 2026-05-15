namespace Ong.Domain.Queries.Responses
{
    public class OutboxMessageResponse
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public string? Error { get; set; }
    }
}
