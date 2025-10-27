using PB.CreditCardService.Application.DTOs;

namespace PB.CreditCardService.Application.Services
{
    public interface ICreditCardService
    {
        Task<CreditCardResponse> IssueCardAsync(IssueCardRequest request);
        Task<CreditCardResponse> GetCardAsync(Guid cardId);
        Task<List<CreditCardResponse>> GetCardsByCustomerAsync(Guid customerId);
    }
}