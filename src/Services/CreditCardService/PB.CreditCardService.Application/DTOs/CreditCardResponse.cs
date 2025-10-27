using PB.CreditCardService.Domain.Enums;

namespace PB.CreditCardService.Application.DTOs
{
    public class CreditCardResponse
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CardNumberMasked { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal AvailableLimit { get; set; }
        public CardStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}