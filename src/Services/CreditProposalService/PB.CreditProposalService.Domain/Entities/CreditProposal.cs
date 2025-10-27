using PB.CreditProposalService.Domain.Enums;

namespace PB.CreditProposalService.Domain.Entities
{
    public class CreditProposal
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public int Score { get; private set; }
        public ProposalStatus Status { get; private set; }
        public int ApprovedCards { get; private set; }
        public decimal CreditLimitPerCard { get; private set; }
        public string RejectionReason { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ProcessedAt { get; private set; }

        private CreditProposal() { }

        public CreditProposal(Guid customerId, int score)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            Score = score;
            Status = ProposalStatus.Pending;
            CreatedAt = DateTime.UtcNow;

            ProcessProposal();
        }

        private void ProcessProposal()
        {
            if (Score <= 100)
            {
                Status = ProposalStatus.Rejected;
                RejectionReason = "Score insuficiente para liberação de crédito";
                ApprovedCards = 0;
                CreditLimitPerCard = 0;
            }
            else if (Score <= 500)
            {
                Status = ProposalStatus.Approved;
                ApprovedCards = 1;
                CreditLimitPerCard = 1000;
            }
            else // Score > 500
            {
                Status = ProposalStatus.Approved;
                ApprovedCards = 2;
                CreditLimitPerCard = 5000;
            }

            ProcessedAt = DateTime.UtcNow;
        }

        public void MarkAsProcessed()
        {
            Status = ProposalStatus.Processed;
        }
    }
}
