using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PB.CreditProposalService.Infrastructure.Data;

namespace PB.CreditProposalService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProposalsController : ControllerBase
    {
        private readonly ProposalDbContext _context;
        private readonly ILogger<ProposalsController> _logger;

        public ProposalsController(
            ProposalDbContext context,
            ILogger<ProposalsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Consulta proposta por ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var proposal = await _context.CreditProposals
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (proposal == null)
                return NotFound(new { error = "Proposta não encontrada" });

            return Ok(proposal);
        }

        /// <summary>
        /// Consulta proposta por ID do cliente
        /// </summary>
        [HttpGet("customer/{customerId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCustomerId(Guid customerId, CancellationToken cancellationToken)
        {
            var proposals = await _context.CreditProposals
                .Where(p => p.CustomerId == customerId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync(cancellationToken);

            if (!proposals.Any())
                return NotFound(new { error = "Nenhuma proposta encontrada para este cliente" });

            return Ok(proposals);
        }
    }
}
