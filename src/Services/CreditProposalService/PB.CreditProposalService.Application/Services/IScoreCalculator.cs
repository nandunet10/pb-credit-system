using PB.CreditProposalService.Application.DTOs;

namespace PB.CreditProposalService.Application.Services
{
    public interface IScoreCalculator
    {
        int CalculateScore(CustomerData customerData);
    }
}