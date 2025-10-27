using FluentAssertions;
using PB.CustomerService.Domain.Entities;
using PB.CustomerService.Domain.Enums;
using PB.CustomerService.Domain.Exceptions;
using Xunit;

namespace PB.CustomerService.Tests.Domain;

public class CustomerTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateCustomer()
    {
        // Arrange
        var name = "João Silva";
        var cpf = "12345678901";
        var email = "joao@example.com";
        var dateOfBirth = new DateTime(1990, 1, 1);
        var income = 5000m;
        var phoneNumber = "11999999999";
        var address = CreateValidAddress();

        // Act
        var customer = new Customer(name, cpf, email, dateOfBirth, income, phoneNumber, address);

        // Assert
        customer.Should().NotBeNull();
        customer.Id.Should().NotBeEmpty();
        customer.Name.Should().Be(name);
        customer.CPF.Should().Be(cpf);
        customer.Status.Should().Be(CustomerStatus.Active);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("AB")]
    public void Constructor_WithInvalidName_ShouldThrowException(string invalidName)
    {
        // Act
        var act = () => new Customer(
            invalidName,
            "12345678901",
            "test@example.com",
            new DateTime(1990, 1, 1),
            5000m,
            "11999999999",
            CreateValidAddress()
        );

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void GetAge_ShouldCalculateCorrectAge()
    {
        // Arrange
        var dateOfBirth = DateTime.Today.AddYears(-30);
        var customer = new Customer(
            "Test",
            "12345678901",
            "test@example.com",
            dateOfBirth,
            5000m,
            "11999999999",
            CreateValidAddress()
        );

        // Act
        var age = customer.GetAge();

        // Assert
        age.Should().Be(30);
    }

    private static Address CreateValidAddress()
    {
        return new Address(
            "Rua Teste",
            "123",
            "Apto 10",
            "Centro",
            "São Paulo",
            "SP",
            "01234567"
        );
    }
}