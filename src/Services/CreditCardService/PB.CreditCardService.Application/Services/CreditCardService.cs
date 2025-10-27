using PB.CreditCardService.Application.DTOs;
using PB.CreditCardService.Domain.Entities;
using PB.Shared.Core.Exceptions;

namespace PB.CreditCardService.Application.Services
{
    public class CreditCardService(ICreditCardRepository cardRepository) : ICreditCardService
    {
        private readonly ICreditCardRepository _cardRepository = cardRepository;

        public async Task<CreditCardResponse> IssueCardAsync(IssueCardRequest request)
        {
            // Gerar número do cartão (simulação)
            var card = new CreditCard(
               request.ProposalId,
               request.CustomerId,
               request.CreditLimit
           );

            await _cardRepository.AddAsync(card);

            return new CreditCardResponse
            {
                Id = card.Id,
                CustomerId = card.CustomerId,
                CardNumberMasked = MaskCardNumber(card.CardNumber),
                ExpirationDate = card.ExpirationDate,
                CreditLimit = card.CreditLimit,
                AvailableLimit = card.AvailableLimit,
                Status = card.Status,
                //CreatedAt = card.CreatedAt
            };
        }

        public async Task<CreditCardResponse> GetCardAsync(Guid cardId)
        {
            var card = await _cardRepository.GetByIdAsync(cardId);
            if (card == null)
            {
                throw new NotFoundException("Cartão não encontrado");
            }

            return MapToResponse(card);
        }

        public async Task<List<CreditCardResponse>> GetCardsByCustomerAsync(Guid customerId)
        {
            var cards = await _cardRepository.GetByCustomerIdAsync(customerId);
            return cards.Select(MapToResponse).ToList();
        }

        private CreditCardResponse MapToResponse(CreditCard card)
        {
            return new CreditCardResponse
            {
                Id = card.Id,
                CustomerId = card.CustomerId,
                CardNumberMasked = MaskCardNumber(card.CardNumber),
                ExpirationDate = card.ExpirationDate,
                CreditLimit = card.CreditLimit,
                AvailableLimit = card.AvailableLimit,
                Status = card.Status,
                //CreatedAt = card.CreatedAt
            };
        }

        private static string MaskCardNumber(string cardNumber)
        {
            if (cardNumber.Length != 16) return "****";
            return $"**** **** **** {cardNumber.Substring(12)}";
        }
    }
}