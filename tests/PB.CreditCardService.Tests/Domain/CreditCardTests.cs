using FluentAssertions;
using PB.CreditCardService.Domain.Entities;
using PB.CreditCardService.Domain.Enums;
using Xunit;

namespace PB.CreditCardService.Tests.Domain;

public class CreditCardTests
{
    [Fact]
    public void Constructor_ShouldCreateCardWithValidData()
    {
        // Arrange
        var proposalId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var creditLimit = 5000m;

        // Act
        var card = new CreditCard(proposalId, customerId, creditLimit);

        // Assert
        card.Id.Should().NotBeEmpty();
        card.ProposalId.Should().Be(proposalId);
        card.CustomerId.Should().Be(customerId);
        card.CardNumber.Should().HaveLength(16);
        card.CVV.Should().HaveLength(3);
        card.CreditLimit.Should().Be(creditLimit);
        card.AvailableLimit.Should().Be(creditLimit);
        card.Status.Should().Be(CardStatus.Issued);
    }

    [Fact]
    public void Activate_WhenCardIsIssued_ShouldChangeStatusToActive()
    {
        // Arrange
        var card = new CreditCard(Guid.NewGuid(), Guid.NewGuid(), 1000m);

        // Act
        card.Activate();

        // Assert
        card.Status.Should().Be(CardStatus.Active);
        card.ActivatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Block_ShouldChangeStatusToBlocked()
    {
        // Arrange
        var card = new CreditCard(Guid.NewGuid(), Guid.NewGuid(), 1000m);
        card.Activate();

        // Act
        card.Block("Cartão perdido");

        // Assert
        card.Status.Should().Be(CardStatus.Blocked);
    }
}