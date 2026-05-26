namespace TALPremiumCalculator;

using System;
using System.Windows.Forms;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        Form1 view = new Form1();
        PremiumModel model = new PremiumModel();

        // Instantiate both specialized controllers independently
        PremiumCalculationController calcController = new PremiumCalculationController(view, model);
        FormSubmissionController submitController = new FormSubmissionController(view, model);

        // Inject dependencies into the view object
        view.LinkControllers(calcController, submitController);

        Application.Run(view);
    }
}
