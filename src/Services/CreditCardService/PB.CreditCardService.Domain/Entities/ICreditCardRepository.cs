namespace PB.CreditCardService.Domain.Entities
{
    public interface ICreditCardRepository
    {
        Task<CreditCard> GetByIdAsync(Guid id);
        Task<List<CreditCard>> GetByCustomerIdAsync(Guid customerId);
        Task AddAsync(CreditCard card);
        Task UpdateAsync(CreditCard card);
    }
}
