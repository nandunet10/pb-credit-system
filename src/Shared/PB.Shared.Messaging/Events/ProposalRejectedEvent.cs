namespace PB.Shared.Messaging.Events
{
    public class ProposalRejectedEvent
    {
        public Guid ProposalId { get; init; }
        public Guid CustomerId { get; init; }
        public int Score { get; init; }
        public string RejectionReason { get; init; }
        public DateTime RejectedAt { get; init; }
    }
}
