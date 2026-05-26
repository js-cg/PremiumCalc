using System;

namespace TALPremiumCalculator;

public class FormSubmissionController
{
    private readonly Form1 _view;
    private readonly PremiumModel _model;

    public FormSubmissionController(Form1 view, PremiumModel model)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _model = model ?? throw new ArgumentNullException(nameof(model));
    }

    /// <summary>
    /// Executes validation checks and submits the verified form data.
    /// </summary>
    public bool HandleFinalSubmission()
    {
        try
        {
            _view.ClearErrors();
            bool isValid = true;

            // 1. Validate Name Fields
            if (string.IsNullOrWhiteSpace(_view.CustomerName))
            {
                _view.SetErrorOnName("Name is a mandatory field.");
                isValid = false;
            }

            // 2. Validate Age Constraints
            if (!int.TryParse(_view.AgeText, out int age) || age <= 0 || age > 120)
            {
                _view.SetErrorOnAge("Please enter a valid age between 1 and 120.");
                isValid = false;
            }

            // 3. Validate Occupation List
            if (string.IsNullOrEmpty(_view.SelectedOccupation))
            {
                _view.SetErrorOnOccupation("Please select your Usual Occupation.");
                isValid = false;
            }

            // 4. Validate Sum Insured Bounds
            if (!double.TryParse(_view.SumInsuredText, out double sum) || sum <= 0)
            {
                _view.SetErrorOnSumInsured("Please enter a valid Sum Insured amount greater than 0.");
                isValid = false;
            }

            if (!isValid)
            {
                _view.ShowSystemWarning("Please correct the form fields highlighted in red.");
                return false;
            }

            // Commit views values to the model state layer
            _model.Name = _view.CustomerName;
            _model.Age = age;
            _model.SelectedOccupation = _view.SelectedOccupation;
            _model.SumInsured = sum;

            // Final check transaction logic verification
            double finalPremium = _model.CalculateMonthlyPremium();

            _view.ShowSuccessMessage($"Validation Success!\nFinal Monthly Premium: {finalPremium.ToString("C2")}");
            return true;
        }
        catch (Exception ex)
        {
            _view.ShowSystemError($"Submission Failure: {ex.Message}");
            return false;
        }
    }
}
