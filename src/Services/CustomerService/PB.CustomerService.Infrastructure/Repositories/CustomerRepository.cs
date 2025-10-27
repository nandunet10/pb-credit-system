using Microsoft.EntityFrameworkCore;
using PB.CustomerService.Domain.Entities;
using PB.CustomerService.Infrastructure.Data;

namespace PB.CustomerService.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Customer> GetByCPFAsync(string cpf, CancellationToken cancellationToken = default)
        {
            var cleanCpf = cpf.Replace(".", "").Replace("-", "").Trim();
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.CPF == cleanCpf, cancellationToken);
        }

        public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            await _context.Customers.AddAsync(customer, cancellationToken);
        }

        public Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            _context.Customers.Update(customer);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
