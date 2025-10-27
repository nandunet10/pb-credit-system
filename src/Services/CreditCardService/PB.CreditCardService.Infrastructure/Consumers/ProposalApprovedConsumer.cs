using MassTransit;
using Microsoft.Extensions.Logging;
using PB.CreditCardService.Domain.Entities;
using PB.CreditCardService.Infrastructure.Data;
using PB.Shared.Messaging.Events;
using Polly;
using Polly.Retry;

namespace PB.CreditCardService.Infrastructure.Consumers
{
    public class ProposalApprovedConsumer : IConsumer<ProposalApprovedEvent>
    {
        private readonly CardDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<ProposalApprovedConsumer> _logger;
        private readonly AsyncRetryPolicy _retryPolicy;

        public ProposalApprovedConsumer(
            CardDbContext context,
            IPublishEndpoint publishEndpoint,
            ILogger<ProposalApprovedConsumer> logger)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
            _logger = logger;

            // Política de retry: 3 tentativas com backoff exponencial
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(
                            exception,
                            "Tentativa {RetryCount} falhou. Aguardando {TimeSpan}s antes de tentar novamente",
                            retryCount,
                            timeSpan.TotalSeconds);
                    });
        }

        public async Task Consume(ConsumeContext<ProposalApprovedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation(
                "Processando evento ProposalApproved para proposta {ProposalId} - {ApprovedCards} cartão(ões)",
                message.ProposalId,
                message.ApprovedCards);

            try
            {
                for (int i = 0; i < message.ApprovedCards; i++)
                {
                    var cardNumber = i + 1;

                    _logger.LogInformation(
                        "Tentando emitir cartão {CardNumber}/{TotalCards} para cliente {CustomerId}",
                        cardNumber,
                        message.ApprovedCards,
                        message.CustomerId);

                    try
                    {
                        await _retryPolicy.ExecuteAsync(async () =>
                        {
                            await IssueCardAsync(message, cardNumber);
                        });

                        _logger.LogInformation(
                            "Cartão {CardNumber}/{TotalCards} emitido com sucesso para cliente {CustomerId}",
                            cardNumber,
                            message.ApprovedCards,
                            message.CustomerId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Falha ao emitir cartão {CardNumber}/{TotalCards} após todas as tentativas. Proposta: {ProposalId}",
                            cardNumber,
                            message.ApprovedCards,
                            message.ProposalId);

                        // Publicar evento de falha
                        var failureEvent = new CardIssuanceFailedEvent
                        {
                            ProposalId = message.ProposalId,
                            CustomerId = message.CustomerId,
                            AttemptNumber = cardNumber,
                            ErrorMessage = ex.Message,
                            FailedAt = DateTime.UtcNow
                        };

                        await _publishEndpoint.Publish(failureEvent);

                        // Não lançar exceção para permitir processamento dos próximos cartões
                        // A mensagem será tratada pela Dead Letter Queue
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro crítico ao processar proposta {ProposalId}",
                    message.ProposalId);
                throw; // Lançar para reprocessamento
            }
        }

        private async Task IssueCardAsync(ProposalApprovedEvent message, int cardNumber)
        {
            // Simulação de possível falha (remover em produção)
            // if (new Random().Next(0, 10) < 2) throw new Exception("Simulação de falha na emissão");

            var card = new CreditCard(
                message.ProposalId,
                message.CustomerId,
                message.CreditLimitPerCard);

            await _context.CreditCards.AddAsync(card);
            await _context.SaveChangesAsync();

            // Publicar evento de cartão emitido
            var cardEvent = new CardIssuedEvent
            {
                CardId = card.Id,
                ProposalId = card.ProposalId,
                CustomerId = card.CustomerId,
                CardNumberMasked = MaskCardNumber(card.CardNumber),
                CreditLimit = card.CreditLimit,
                IssuedAt = card.IssuedAt
            };

            await _publishEndpoint.Publish(cardEvent);
        }

        private string MaskCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 4)
                return cardNumber;

            return $"****-****-****-{cardNumber.Substring(cardNumber.Length - 4)}";
        }
    }
}
