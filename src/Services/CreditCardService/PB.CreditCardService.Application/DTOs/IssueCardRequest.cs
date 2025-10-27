namespace PB.CreditCardService.Application.DTOs
{
    public class IssueCardRequest
    {
        public Guid ProposalId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal CreditLimit { get; set; }
    }
}