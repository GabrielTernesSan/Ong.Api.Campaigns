namespace Ong.Domain
{
    public class OutboxMessage
    {
        public Guid Id { get; private set; }
        public string Type { get; private set; }
        public string Payload { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? ProcessedOn { get; set; }
        public string? Error { get; set; }
        public int RetryCount { get; set; }

        public OutboxMessage(Guid id, string type, string payload, DateTime createdOn, DateTime? processedOn = null, string? error = null, int retryCount = 0)
        {
            Id = id;
            Type = type;
            Payload = payload;
            CreatedOn = createdOn;
            ProcessedOn = processedOn;
            Error = error;
            RetryCount = retryCount;
        }

        public void UpdateProcessedTime(DateTime processedOn) =>
            ProcessedOn = processedOn;

        public void ErrorOccurred(string error)
        {
            RetryCount++;
            Error = error;
        }
    }
}
