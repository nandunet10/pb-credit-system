using MassTransit;
using Microsoft.Extensions.Logging;
using PB.CustomerService.Application.DTOs;
using PB.CustomerService.Domain.Entities;
using PB.Shared.Messaging.Events;

namespace PB.CustomerService.Application.Services
{
    public class CustomerService(
        ICustomerRepository repository,
        IPublishEndpoint publishEndpoint,
        ILogger<CustomerService> logger) : ICustomerService
    {
        private readonly ICustomerRepository _repository = repository;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly ILogger<CustomerService> _logger = logger;

        public async Task<Result<CustomerResponse>> CreateCustomerAsync(
            CreateCustomerRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Verificar se CPF já existe
                var existingCustomer = await _repository.GetByCPFAsync(request.CPF, cancellationToken);
                if (existingCustomer != null)
                {
                    return Result.Failure<CustomerResponse>("Cliente com este CPF já existe");
                }

                // Criar entidade de domínio
                var address = new Address(
                    request.Address.Street,
                    request.Address.Number,
                    request.Address.Complement,
                    request.Address.Neighborhood,
                    request.Address.City,
                    request.Address.State,
                    request.Address.ZipCode
                );

                var customer = new Customer(
                    request.Name,
                    request.CPF,
                    request.Email,
                    request.DateOfBirth,
                    request.Income,
                    request.PhoneNumber,
                    address
                );

                // Salvar no banco
                await _repository.AddAsync(customer, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Cliente {CustomerId} criado com sucesso", customer.Id);

                // Publicar evento
                var customerEvent = new CustomerRegisteredEvent
                {
                    CustomerId = customer.Id,
                    Name = customer.Name,
                    CPF = customer.CPF,
                    Email = customer.Email,
                    DateOfBirth = customer.DateOfBirth,
                    Income = customer.Income,
                    RegisteredAt = customer.CreatedAt
                };

                await _publishEndpoint.Publish(customerEvent, cancellationToken);

                _logger.LogInformation("Evento CustomerRegistered publicado para o cliente {CustomerId}", customer.Id);

                // Retornar resposta
                var response = new CustomerResponse
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    CPF = customer.CPF,
                    Email = customer.Email,
                    DateOfBirth = customer.DateOfBirth,
                    Income = customer.Income,
                    PhoneNumber = customer.PhoneNumber,
                    Address = request.Address,
                    Status = customer.Status.ToString(),
                    CreatedAt = customer.CreatedAt
                };

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar cliente");
                return Result.Failure<CustomerResponse>($"Erro ao criar cliente: {ex.Message}");
            }
        }

        public async Task<Result<CustomerResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var customer = await _repository.GetByIdAsync(id, cancellationToken);

            if (customer == null)
                return Result.Failure<CustomerResponse>("Cliente não encontrado");

            var response = MapToResponse(customer);
            return Result.Success(response);
        }

        public async Task<Result<CustomerResponse>> GetByCPFAsync(string cpf, CancellationToken cancellationToken = default)
        {
            var customer = await _repository.GetByCPFAsync(cpf, cancellationToken);

            if (customer == null)
                return Result.Failure<CustomerResponse>("Cliente não encontrado");

            var response = MapToResponse(customer);
            return Result.Success(response);
        }

        private CustomerResponse MapToResponse(Customer customer)
        {
            return new CustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                CPF = customer.CPF,
                Email = customer.Email,
                DateOfBirth = customer.DateOfBirth,
                Income = customer.Income,
                PhoneNumber = customer.PhoneNumber,
                Address = new AddressDto
                {
                    Street = customer.Address.Street,
                    Number = customer.Address.Number,
                    Complement = customer.Address.Complement,
                    Neighborhood = customer.Address.Neighborhood,
                    City = customer.Address.City,
                    State = customer.Address.State,
                    ZipCode = customer.Address.ZipCode
                },
                Status = customer.Status.ToString(),
                CreatedAt = customer.CreatedAt
            };
        }
    }
}
