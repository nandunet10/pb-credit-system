using FluentAssertions;
using global::PB.CreditProposalService.Domain.Entities;
using global::PB.CreditProposalService.Domain.Enums;
using Xunit;

namespace PB.CreditProposalService.Tests.Domain;

public class CreditProposalTests
{
    [Theory]
    [InlineData(50, ProposalStatus.Rejected, 0, 0)]
    [InlineData(100, ProposalStatus.Rejected, 0, 0)]
    public void Constructor_WithLowScore_ShouldRejectProposal(
        int score,
        ProposalStatus expectedStatus,
        int expectedCards,
        decimal expectedLimit)
    {
        // Act
        var proposal = new CreditProposal(Guid.NewGuid(), score);

        // Assert
        proposal.Status.Should().Be(expectedStatus);
        proposal.ApprovedCards.Should().Be(expectedCards);
        proposal.CreditLimitPerCard.Should().Be(expectedLimit);
        proposal.RejectionReason.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData(150, ProposalStatus.Approved, 1, 1000)]
    [InlineData(500, ProposalStatus.Approved, 1, 1000)]
    public void Constructor_WithMediumScore_ShouldApproveOneCard(
        int score,
        ProposalStatus expectedStatus,
        int expectedCards,
        decimal expectedLimit)
    {
        // Act
        var proposal = new CreditProposal(Guid.NewGuid(), score);

        // Assert
        proposal.Status.Should().Be(expectedStatus);
        proposal.ApprovedCards.Should().Be(expectedCards);
        proposal.CreditLimitPerCard.Should().Be(expectedLimit);
    }

    [Theory]
    [InlineData(501, ProposalStatus.Approved, 2, 5000)]
    [InlineData(1000, ProposalStatus.Approved, 2, 5000)]
    public void Constructor_WithHighScore_ShouldApproveTwoCards(
        int score,
        ProposalStatus expectedStatus,
        int expectedCards,
        decimal expectedLimit)
    {
        // Act
        var proposal = new CreditProposal(Guid.NewGuid(), score);

        // Assert
        proposal.Status.Should().Be(expectedStatus);
        proposal.ApprovedCards.Should().Be(expectedCards);
        proposal.CreditLimitPerCard.Should().Be(expectedLimit);
    }
}

