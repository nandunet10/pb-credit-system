using FluentAssertions;
using PB.CreditProposalService.Application.DTOs;
using PB.CreditProposalService.Application.Services;
using Xunit;

namespace PB.CreditProposalService.Tests.Application
{
    public class ScoreCalculatorTests
    {
        private readonly IScoreCalculator _calculator;

        public ScoreCalculatorTests()
        {
            _calculator = new ScoreCalculator();
        }

        [Fact]
        public void CalculateScore_YoungLowIncome_ShouldReturnCorrectScore()
        {
            // Arrange
            var customerData = new CustomerData
            {
                BirthDate = new DateTime(2000, 1, 1),
                MonthlyIncome = 1500,
                InitialScore = 50
            };

            // Act
            var score = _calculator.CalculateScore(customerData);

            // Assert
            score.Should().BeGreaterThanOrEqualTo(150); // Correto: BeGreaterThanOrEqualTo
            score.Should().BeLessThanOrEqualTo(1000);   // Correto: BeLessThanOrEqualTo
        }

        [Fact]
        public void CalculateScore_MiddleAgeHighIncome_ShouldReturnCorrectScore()
        {
            // Arrange
            var customerData = new CustomerData
            {
                BirthDate = new DateTime(1980, 1, 1),
                MonthlyIncome = 15000,
                InitialScore = 600
            };

            // Act
            var score = _calculator.CalculateScore(customerData);

            // Assert
            score.Should().BeGreaterThanOrEqualTo(1000);
            score.Should().Be(1000);
        }

        [Fact]
        public void CalculateScore_SeniorMediumIncome_ShouldReturnCorrectScore()
        {
            // Arrange
            var customerData = new CustomerData
            {
                BirthDate = new DateTime(1950, 1, 1),
                MonthlyIncome = 7000,
                InitialScore = 300
            };

            // Act
            var score = _calculator.CalculateScore(customerData);

            // Assert
            score.Should().BeGreaterThanOrEqualTo(450);
            score.Should().BeLessThanOrEqualTo(1000);
        }
    }
}