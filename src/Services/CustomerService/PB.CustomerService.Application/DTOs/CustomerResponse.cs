namespace PB.CustomerService.Application.DTOs
{
    public record CustomerResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string CPF { get; init; }
        public string Email { get; init; }
        public DateTime DateOfBirth { get; init; }
        public decimal Income { get; init; }
        public string PhoneNumber { get; init; }
        public AddressDto Address { get; init; }
        public string Status { get; init; }
        public DateTime CreatedAt { get; init; }
    }

}
