using System;
using System.Collections.Generic;

namespace TALPremiumCalculator;

public class PremiumModel
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public double SumInsured { get; set; }
    public string SelectedOccupation { get; set; } = string.Empty;

    private static readonly Dictionary<string, string> OccupationToRating = new()
    {
        { "Cleaner", "Light Manual" }, { "Doctor", "Professional" }, { "Author", "White Collar" },
        { "Farmer", "Heavy Manual" }, { "Mechanic", "Heavy Manual" }, { "Florist", "Light Manual" },
        { "Other", "Heavy Manual" }
    };

    private static readonly Dictionary<string, double> RatingToFactor = new()
    {
        { "Professional", 1.5 }, { "White Collar", 2.25 }, { "Light Manual", 11.50 }, { "Heavy Manual", 31.75 }
    };

    /// <summary>
    /// Executes the core premium math safely with data boundaries.
    /// </summary>
    public double CalculateMonthlyPremium()
    {
        // Defensive checks against impossible financial boundary values
        if (Age <= 0 || Age > 120 || SumInsured <= 0)
        {
            throw new ArgumentOutOfRangeException("Age or Sum Insured falls outside realistic insurance risk limits.");
        }

        if (!OccupationToRating.TryGetValue(SelectedOccupation, out string? rating))
        {
            throw new KeyNotFoundException($"The selected occupation '{SelectedOccupation}' is unrecognized.");
        }

        if (!RatingToFactor.TryGetValue(rating, out double ratingFactor))
        {
            throw new KeyNotFoundException($"A mathematical factor could not be found for rating tier '{rating}'.");
        }

        // Apply Formula safely inside checked math context
        try
        {
            double premium = (SumInsured * ratingFactor * Age) / (1000 * 12);
            
            if (double.IsInfinity(premium) || double.IsNaN(premium))
                throw new OverflowException("The premium calculation resulted in an overflow or an undefined numeric state.");

            return premium;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred during calculation computation arithmetic.", ex);
        }
    }
}
