namespace PB.Shared.Messaging.Events
{
    public record CardIssuanceFailedEvent
    {
        public Guid ProposalId { get; init; }
        public Guid CustomerId { get; init; }
        public int AttemptNumber { get; init; }
        public string ErrorMessage { get; init; }
        public DateTime FailedAt { get; init; }
    }
}
