namespace PB.Shared.Messaging.Events
{
    public record ProposalApprovedEvent
    {
        public Guid ProposalId { get; init; }
        public Guid CustomerId { get; init; }
        public int Score { get; init; }
        public int ApprovedCards { get; init; }
        public decimal CreditLimitPerCard { get; init; }
        public DateTime ApprovedAt { get; init; }
    }
}
