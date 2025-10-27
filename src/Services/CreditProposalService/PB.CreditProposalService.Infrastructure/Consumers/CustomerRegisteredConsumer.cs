using MassTransit;
using Microsoft.Extensions.Logging;
using PB.CreditProposalService.Application.DTOs;
using PB.CreditProposalService.Application.Services;
using PB.CreditProposalService.Domain.Entities;
using PB.CreditProposalService.Domain.Enums;
using PB.CreditProposalService.Infrastructure.Data;
using PB.Shared.Messaging.Events;

namespace PB.CreditProposalService.Infrastructure.Consumers
{
    public class CustomerRegisteredConsumer : IConsumer<CustomerRegisteredEvent>
    {
        private readonly ProposalDbContext _context;
        private readonly IScoreCalculator _scoreCalculator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<CustomerRegisteredConsumer> _logger;

        public CustomerRegisteredConsumer(
            ProposalDbContext context,
            IScoreCalculator scoreCalculator,
            IPublishEndpoint publishEndpoint,
            ILogger<CustomerRegisteredConsumer> logger)
        {
            _context = context;
            _scoreCalculator = scoreCalculator;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CustomerRegisteredEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation(
                "Processando evento CustomerRegistered para o cliente {CustomerId}",
                message.CustomerId);

            try
            {
                // Calcular score
                var customerData = new CustomerData
                {
                    CustomerId = message.CustomerId,
                    MonthlyIncome = message.Income,
                    BirthDate = message.DateOfBirth
                };

                var score = _scoreCalculator.CalculateScore(customerData);

                _logger.LogInformation(
                    "Score calculado para cliente {CustomerId}: {Score}",
                    message.CustomerId,
                    score);

                // Criar proposta
                var proposal = new CreditProposal(message.CustomerId, score);

                // Salvar proposta
                await _context.CreditProposals.AddAsync(proposal);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Proposta {ProposalId} criada com status {Status}",
                    proposal.Id,
                    proposal.Status);

                // Publicar evento apropriado
                if (proposal.Status == ProposalStatus.Approved)
                {
                    var approvedEvent = new ProposalApprovedEvent
                    {
                        ProposalId = proposal.Id,
                        CustomerId = proposal.CustomerId,
                        Score = proposal.Score,
                        ApprovedCards = proposal.ApprovedCards,
                        CreditLimitPerCard = proposal.CreditLimitPerCard,
                        ApprovedAt = proposal.ProcessedAt.Value
                    };

                    await _publishEndpoint.Publish(approvedEvent);

                    _logger.LogInformation(
                        "Evento ProposalApproved publicado para proposta {ProposalId}",
                        proposal.Id);
                }
                else
                {
                    var rejectedEvent = new ProposalRejectedEvent
                    {
                        ProposalId = proposal.Id,
                        CustomerId = proposal.CustomerId,
                        Score = proposal.Score,
                        RejectionReason = proposal.RejectionReason,
                        RejectedAt = proposal.ProcessedAt.Value
                    };

                    await _publishEndpoint.Publish(rejectedEvent);

                    _logger.LogInformation(
                        "Evento ProposalRejected publicado para proposta {ProposalId}",
                        proposal.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao processar evento CustomerRegistered para cliente {CustomerId}",
                    message.CustomerId);
                throw;
            }
        }
    }
}
