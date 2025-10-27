using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PB.CreditCardService.Infrastructure.Data;

namespace PB.CreditCardService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController(
        CardDbContext context,
        ILogger<CardsController> logger) : ControllerBase
    {
        private readonly CardDbContext _context = context;
        private readonly ILogger<CardsController> _logger = logger;

        /// <summary>
        /// Consulta cartão por ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var card = await _context.CreditCards
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (card == null)
                return NotFound(new { error = "Cartão não encontrado" });

            return Ok(new
            {
                card.Id,
                card.ProposalId,
                card.CustomerId,
                CardNumber = MaskCardNumber(card.CardNumber),
                card.ExpirationDate,
                card.CreditLimit,
                card.AvailableLimit,
                Status = card.Status.ToString(),
                card.IssuedAt,
                card.ActivatedAt
            });
        }

        /// <summary>
        /// Lista cartões do cliente
        /// </summary>
        [HttpGet("customer/{customerId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCustomerId(Guid customerId, CancellationToken cancellationToken)
        {
            var cards = await _context.CreditCards
                .Where(c => c.CustomerId == customerId)
                .OrderByDescending(c => c.IssuedAt)
                .Select(c => new
                {
                    c.Id,
                    c.ProposalId,
                    c.CustomerId,
                    CardNumber = MaskCardNumber(c.CardNumber),
                    c.ExpirationDate,
                    c.CreditLimit,
                    c.AvailableLimit,
                    Status = c.Status.ToString(),
                    c.IssuedAt,
                    c.ActivatedAt
                })
                .ToListAsync(cancellationToken);

            return Ok(cards);
        }

        private string MaskCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 4)
                return cardNumber;

            return $"****-****-****-{cardNumber.Substring(cardNumber.Length - 4)}";
        }
    }
}
