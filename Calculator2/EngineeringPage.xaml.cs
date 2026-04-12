using System.Globalization;

namespace Calculator2;

public partial class EngineeringPage : ContentPage
{

    private void OnMenuClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    double firstNumber = 0;
    string mathOperator = "";
    bool isOperatorClicked = false;

    public EngineeringPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async Task AnimateButton(Button button)
    {
        await button.FadeTo(0.5, 100);
        await button.FadeTo(1.0, 100);
    }

    private double GetNumberFromScreen()
    {
        if (double.TryParse(ResultLabel.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            return result;
        return 0;
    }

    private void SetNumberToScreen(double number)
    {
        double rounded = Math.Round(number, 8);
        ResultLabel.Text = rounded.ToString(CultureInfo.InvariantCulture);
    }

    private async void OnNumberClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await AnimateButton(button);
        string pressed = button.Text;

        if (isOperatorClicked)
        {
            ResultLabel.Text = "0";
            isOperatorClicked = false;
        }

        if ((ResultLabel.Text == "0" || ResultLabel.Text == "-0") && pressed != ".")
        {
            if (ResultLabel.Text == "-0")
                ResultLabel.Text = "-" + pressed;
            else
                ResultLabel.Text = pressed;
        }
        else
        {
            if (pressed == "." && ResultLabel.Text.Contains(".")) return;
            ResultLabel.Text += pressed;
        }
    }

    private async void OnOperatorClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await AnimateButton(button);
        isOperatorClicked = true;
        mathOperator = button.Text;
        firstNumber = GetNumberFromScreen();
    }

    private async void OnCalculateClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await AnimateButton(button);

        if (string.IsNullOrEmpty(mathOperator)) return;

        double secondNumber = GetNumberFromScreen();
        double result = 0;

        switch (mathOperator)
        {
            case "+": result = firstNumber + secondNumber; break;
            case "-": result = firstNumber - secondNumber; break;
            case "X": result = firstNumber * secondNumber; break;
            case "/":
                if (secondNumber != 0)
                    result = firstNumber / secondNumber;
                else
                {
                    ResultLabel.Text = "Error";
                    return;
                }
                break;
        }

        SetNumberToScreen(result);
        firstNumber = result;
        mathOperator = "";
    }

    private async void OnClearClicked(object sender, EventArgs e)
    {
        await AnimateButton((Button)sender);
        ResultLabel.Text = "0";
        firstNumber = 0;
        mathOperator = "";
    }

    private async void OnNegativeClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await AnimateButton(button);

        if (isOperatorClicked)
        {
            ResultLabel.Text = "-0";
            isOperatorClicked = false;
            return;
        }

        double val = GetNumberFromScreen();
        SetNumberToScreen(val * -1);
    }


    private async void OnScientificClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await AnimateButton(button);

        string func = button.Text;
        double currentVal = GetNumberFromScreen();
        double result = 0;

        switch (func)
        {
            case "sin":
                result = Math.Sin(currentVal * Math.PI / 180);
                break;
            case "cos":
                result = Math.Cos(currentVal * Math.PI / 180);
                break;
            case "tan":
                result = Math.Tan(currentVal * Math.PI / 180);
                break;
            case "log":
                result = Math.Log10(currentVal); 
                break;
            case "ln":
                result = Math.Log(currentVal); 
                break;
            case "√":
                if (currentVal >= 0)
                    result = Math.Sqrt(currentVal);
                else
                {
                    ResultLabel.Text = "Error"; 
                    return;
                }
                break;
            case "x²":
                result = Math.Pow(currentVal, 2);
                break;
            case "%":
                if (!string.IsNullOrEmpty(mathOperator) && firstNumber != 0)
                    result = firstNumber * (currentVal / 100);
                else
                    result = currentVal / 100;
                break;
        }

        SetNumberToScreen(result);
   
        firstNumber = result;
        isOperatorClicked = true; 
    }

    private async void OnPiClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await AnimateButton(button);

        if (button.Text == "π")
            SetNumberToScreen(Math.PI);
        else 
            SetNumberToScreen(Math.E);

        isOperatorClicked = false; 
    }
}