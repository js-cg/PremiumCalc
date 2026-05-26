using System;
using System.Collections.Generic;
using FluentAssertions;
using TALPremiumCalculator;
using Xunit;

namespace TALPremiumCalculator.Tests
{
    public class PremiumModelTests
    {
        #region Valid Calculations (Happy Paths)

        [Theory]
        [InlineData("Doctor", 30, 500000, 937.50)]     // Professional factor: 1.5
        [InlineData("Author", 40, 200000, 1500.00)]    // White Collar factor: 2.25
        [InlineData("Cleaner", 25, 100000, 2395.83)]   // Light Manual factor: 11.50
        [InlineData("Farmer", 50, 300000, 39687.50)]   // Heavy Manual factor: 31.75
        public void CalculateMonthlyPremium_WithValidInputs_ReturnsExpectedPremium(
            string occupation, int age, double sumInsured, double expectedPremium)
        {
            // Arrange
            var model = new PremiumModel
            {
                Name = "John Doe",
                Age = age,
                SumInsured = sumInsured,
                SelectedOccupation = occupation
            };

            // Act
            double actualPremium = model.CalculateMonthlyPremium();

            // Assert
            // Using a precision margin of 2 decimal places due to floating-point division
            actualPremium.Should().BeApproximately(expectedPremium, 0.01);
        }

        #endregion

        #region Edge Cases & Boundary Values

        [Fact]
        public void CalculateMonthlyPremium_WithMaximumValidAge_CalculatesCorrectly()
        {
            // Arrange
            var model = new PremiumModel
            {
                Age = 120, // Max boundary
                SumInsured = 100000,
                SelectedOccupation = "Doctor"
            };

            // Act
            double result = model.CalculateMonthlyPremium();

            // Assert
            result.Should().Be(1500.0);
        }

        [Fact]
        public void CalculateMonthlyPremium_WithMinimumValidAge_CalculatesCorrectly()
        {
            // Arrange
            var model = new PremiumModel
            {
                Age = 1, // Min boundary
                SumInsured = 100000,
                SelectedOccupation = "Doctor"
            };

            // Act
            double result = model.CalculateMonthlyPremium();

            // Assert
            result.Should().Be(12.5);
        }

        #endregion

        #region Exception Testing (Sad Paths)

        [Theory]
        [InlineData(0, 100000)]    // Age at lower bound exception
        [InlineData(-5, 100000)]   // Negative age
        [InlineData(121, 100000)]  // Age past upper bound
        [InlineData(30, 0)]        // Zero sum insured
        [InlineData(30, -50000)]   // Negative sum insured
        public void CalculateMonthlyPremium_WithInvalidAgeOrSumInsured_ThrowsArgumentOutOfRangeException(
            int age, double sumInsured)
        {
            // Arrange
            var model = new PremiumModel
            {
                Age = age,
                SumInsured = sumInsured,
                SelectedOccupation = "Doctor"
            };

            // Act
            Action action = () => model.CalculateMonthlyPremium();

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Age or Sum Insured falls outside realistic insurance risk limits.*");
        }

        [Theory]
        [InlineData("")]
        [InlineData("Engineer")]
        [InlineData("InvalidOccupation")]
        public void CalculateMonthlyPremium_WithUnrecognizedOccupation_ThrowsKeyNotFoundException(
            string invalidOccupation)
        {
            // Arrange
            var model = new PremiumModel
            {
                Age = 30,
                SumInsured = 100000,
                SelectedOccupation = invalidOccupation
            };

            // Act
            Action action = () => model.CalculateMonthlyPremium();

            // Assert
            action.Should().Throw<KeyNotFoundException>()
                .WithMessage($"The selected occupation '{invalidOccupation}' is unrecognized.");
        }

        #endregion
    }
}
