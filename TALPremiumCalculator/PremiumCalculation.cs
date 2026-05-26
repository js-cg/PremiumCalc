namespace TALPremiumCalculator;

using System;
using System.Drawing;
using System.Windows.Forms;

public partial class Form1 : Form
{
    // Inject both specialized workflow controllers
    private PremiumCalculationController? _calcController;
    private FormSubmissionController? _submitController;

    #region Controls & Properties
    //Controls
    private Label lblName, lblAge, lblDob, lblOccupation, lblSumInsured;
    private TextBox txtName, txtAge, txtSumInsured;
    private DateTimePicker dtpDob;
    private ComboBox cmbOccupation;
    private Button btnSubmit;
    private ErrorProvider errorProvider;

    //Properties
    public string CustomerName => txtName?.Text ?? string.Empty;
    public string AgeText => txtAge?.Text ?? string.Empty;
    public string SumInsuredText => txtSumInsured?.Text ?? string.Empty;
    public string SelectedOccupation => cmbOccupation?.SelectedItem?.ToString() ?? string.Empty;
    #endregion

    public Form1() 
    {
        this.Text = "TAL Code Test";
        this.Size = new Size(460, 420);
        this.StartPosition = FormStartPosition.CenterScreen;
        errorProvider = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        InitializeControls();
    }

    // Pass dependencies through independent setters
    public void LinkControllers(PremiumCalculationController calcCtrl, FormSubmissionController submitCtrl)
    {
        _calcController = calcCtrl;
        _submitController = submitCtrl;
    }

    private void InitializeControls()
    {
        TableLayoutPanel tableLayout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(20), ColumnCount = 2, RowCount = 7 };
        tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        for (int i = 0; i < tableLayout.RowCount; i++)
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));

        lblName = new Label { Text = "Name:", Anchor = AnchorStyles.Left, AutoSize = true };
        txtName = new TextBox { Dock = DockStyle.Fill };
        txtName.KeyPress += TxtName_KeyPress; 

        lblAge = new Label { Text = "Age Next Birthday:", Anchor = AnchorStyles.Left, AutoSize = true };
        txtAge = new TextBox { Dock = DockStyle.Fill };
        txtAge.KeyPress += TxtInt_KeyPress;

        lblDob = new Label { Text = "Date of Birth:", Anchor = AnchorStyles.Left, AutoSize = true };
        dtpDob = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Custom, CustomFormat = "MM/yyyy" };

        lblOccupation = new Label { Text = "Usual Occupation:", Anchor = AnchorStyles.Left, AutoSize = true };
        cmbOccupation = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
        cmbOccupation.Items.AddRange(new string[] { "Cleaner", "Doctor", "Author", "Farmer", "Mechanic", "Florist", "Other" });
        cmbOccupation.SelectedIndexChanged += CalculatePremium; 

        lblSumInsured = new Label { Text = "Death - Sum Insured:", Anchor = AnchorStyles.Left, AutoSize = true };
        txtSumInsured = new TextBox { Dock = DockStyle.Fill, Text = "0" };
        txtSumInsured.KeyPress += TxtInt_KeyPress;

        btnSubmit = new Button { Text = "Submit Details", Height = 35, Width = 150, Anchor = AnchorStyles.Right };
        btnSubmit.Click += BtnSubmit_Click; 

        tableLayout.Controls.Add(lblName, 0, 0);       tableLayout.Controls.Add(txtName, 1, 0);
        tableLayout.Controls.Add(lblAge, 0, 1);        tableLayout.Controls.Add(txtAge, 1, 1);
        tableLayout.Controls.Add(lblDob, 0, 2);        tableLayout.Controls.Add(dtpDob, 1, 2);
        tableLayout.Controls.Add(lblOccupation, 0, 3); tableLayout.Controls.Add(cmbOccupation, 1, 3);
        tableLayout.Controls.Add(lblSumInsured, 0, 4); tableLayout.Controls.Add(txtSumInsured, 1, 4);
        tableLayout.Controls.Add(btnSubmit, 1, 5);

        this.Controls.Add(tableLayout);
    }

    private void CalculatePremium(object sender, EventArgs e)
{
    if (_calcController != null)
    {
        _calcController.HandleLiveCalculation();
    }
}

    private void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (_submitController != null && _submitController.HandleFinalSubmission())
        {
            this.Close();
        }
    }

    #region View Presentation Methods
    public void ShowPremiumResult(double premium) => MessageBox.Show($"Calculated Monthly Premium: {premium.ToString("C2")}", "Live Calculation Result");
    public void ShowSuccessMessage(string text) => MessageBox.Show(text, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    public void ShowSystemWarning(string text) => MessageBox.Show(text, "Validation Issue", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    public void ShowSystemError(string message) => MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

    public void ClearErrors() => errorProvider?.Clear();
    public void SetErrorOnName(string error) => errorProvider?.SetError(txtName, error);
    public void SetErrorOnAge(string error) => errorProvider?.SetError(txtAge, error);
    public void SetErrorOnOccupation(string error) => errorProvider?.SetError(cmbOccupation, error);
    public void SetErrorOnSumInsured(string error) => errorProvider?.SetError(txtSumInsured, error);
    #endregion

    #region Filter Keypresses
    private void TxtName_KeyPress(object sender, KeyPressEventArgs e) { if (!char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true; }
    private void TxtInt_KeyPress(object sender, KeyPressEventArgs e) { if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true; }
    #endregion
}

