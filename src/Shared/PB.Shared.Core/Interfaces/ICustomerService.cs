using PB.CustomerService.Application.DTOs;
using PB.Shared.Core.Results;

namespace PB.Shared.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<Result<CustomerResponse>> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);
        Task<Result<CustomerResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<CustomerResponse>> GetByCPFAsync(string cpf, CancellationToken cancellationToken = default);
    }
}
