using PB.CreditCardService.Domain.Enums;
using PB.CreditCardService.Domain.Exceptions;

namespace PB.CreditCardService.Domain.Entities
{
    public class CreditCard
    {
        public Guid Id { get; private set; }
        public Guid ProposalId { get; private set; }
        public Guid CustomerId { get; private set; }
        public string CardNumber { get; private set; }
        public string CVV { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public decimal CreditLimit { get; private set; }
        public decimal AvailableLimit { get; private set; }
        public CardStatus Status { get; private set; }
        public DateTime IssuedAt { get; private set; }
        public DateTime? ActivatedAt { get; private set; }

        private CreditCard() { }

        public CreditCard(Guid proposalId, Guid customerId, decimal creditLimit)
        {
            Id = Guid.NewGuid();
            ProposalId = proposalId;
            CustomerId = customerId;
            CardNumber = GenerateCardNumber();
            CVV = GenerateCVV();
            ExpirationDate = DateTime.UtcNow.AddYears(5);
            CreditLimit = creditLimit;
            AvailableLimit = creditLimit;
            Status = CardStatus.Issued;
            IssuedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            if (Status != CardStatus.Issued)
                throw new DomainException("Apenas cartões recém-emitidos podem ser ativados");

            Status = CardStatus.Active;
            ActivatedAt = DateTime.UtcNow;
        }

        public void Block(string reason)
        {
            if (Status == CardStatus.Blocked || Status == CardStatus.Cancelled)
                throw new DomainException("Cartão já está bloqueado ou cancelado");

            Status = CardStatus.Blocked;
        }

        public void Cancel()
        {
            Status = CardStatus.Cancelled;
        }

        private static string GenerateCardNumber()
        {
            var random = new Random();
            var number = "5412"; // Binário fictício

            for (int i = 0; i < 12; i++)
            {
                number += random.Next(0, 10).ToString();
            }

            return number;
        }

        private static string GenerateCVV()
        {
            var random = new Random();
            return random.Next(100, 1000).ToString();
        }
    }
}
