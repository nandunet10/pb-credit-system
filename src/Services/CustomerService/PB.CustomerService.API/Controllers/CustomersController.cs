using Microsoft.AspNetCore.Mvc;
using PB.CustomerService.Application.DTOs;
using PB.CustomerService.Application.Services;
using PB.CustomerService.Application.Validators;

namespace PB.CustomerService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(
        ICustomerService customerService,
        ILogger<CustomersController> logger) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;
        private readonly ILogger<CustomersController> _logger = logger;

        /// <summary>
        /// Cadastra um novo cliente
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomer(
            [FromBody] CreateCustomerRequest request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Recebida requisição para criar cliente: {CPF}", request.CPF);

            // Validação
            var validator = new CreateCustomerRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    errors = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        message = e.ErrorMessage
                    })
                });
            }

            var result = await _customerService.CreateCustomerAsync(request, cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogWarning("Falha ao criar cliente: {Error}", result.Error);
                return BadRequest(new { error = result.Error });
            }

            _logger.LogInformation("Cliente {CustomerId} criado com sucesso", result.Value.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// Consulta cliente por ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _customerService.GetByIdAsync(id, cancellationToken);

            if (result.IsFailure)
                return NotFound(new { error = result.Error });

            return Ok(result.Value);
        }

        /// <summary>
        /// Consulta cliente por CPF
        /// </summary>
        [HttpGet("cpf/{cpf}")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCPF(string cpf, CancellationToken cancellationToken)
        {
            var result = await _customerService.GetByCPFAsync(cpf, cancellationToken);

            if (result.IsFailure)
                return NotFound(new { error = result.Error });

            return Ok(result.Value);
        }
    }
}
