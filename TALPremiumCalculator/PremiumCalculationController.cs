using System;

namespace TALPremiumCalculator;

public class PremiumCalculationController
{
    private readonly Form1 _view;
    private readonly PremiumModel _model;

    public PremiumCalculationController(Form1 view, PremiumModel model)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _model = model ?? throw new ArgumentNullException(nameof(model));
    }

    /// <summary>
    /// Core functional operation to execute live calculations from view inputs.
    /// </summary>
    public void HandleLiveCalculation()
    {
        try
        {
            string ageText = _view.AgeText;
            string occupationText = _view.SelectedOccupation;
            string sumInsuredText = _view.SumInsuredText;

            // Silently abort if input parameters are incomplete or negative
            if (string.IsNullOrEmpty(occupationText) || 
                !int.TryParse(ageText, out int age) || 
                !double.TryParse(sumInsuredText, out double sumInsured) ||
                age <= 0 || sumInsured <= 0)
            {
                return; 
            }

            // Sync structural parameters down to the Model
            _model.Age = age;
            _model.SelectedOccupation = occupationText;
            _model.SumInsured = sumInsured;

            // Process calculations through the Model layer
            double monthlyPremium = _model.CalculateMonthlyPremium();
            _view.ShowPremiumResult(monthlyPremium);
        }
        catch (Exception ex)
        {
            _view.ShowSystemError($"Live Calculation Error: {ex.Message}");
        }
    }
}
