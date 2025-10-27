namespace PB.Shared.Messaging.Events
{
    public record CustomerRegisteredEvent
    {
        public Guid CustomerId { get; init; }
        public string Name { get; init; }
        public string CPF { get; init; }
        public string Email { get; init; }
        public DateTime DateOfBirth { get; init; }
        public decimal Income { get; init; }
        public DateTime RegisteredAt { get; init; }
    }
}
