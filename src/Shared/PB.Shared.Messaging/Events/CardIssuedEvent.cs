namespace PB.Shared.Messaging.Events
{
    public record CardIssuedEvent
    {
        public Guid CardId { get; init; }
        public Guid ProposalId { get; init; }
        public Guid CustomerId { get; init; }
        public string CardNumberMasked { get; init; }
        public decimal CreditLimit { get; init; }
        public DateTime IssuedAt { get; init; }
    }
}
