using System.Globalization;

namespace Calculator2;

public partial class MainPage : ContentPage
{
    private void OnMenuClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    double firstNumber = 0;       
    string mathOperator = "";  
    bool isOperatorClicked = false; 
    public MainPage()
    {
        InitializeComponent();
    }


    private async Task AnimateButton(Button button)
    {
        await button.FadeTo(0.5, 100); 
        await button.FadeTo(1.0, 100); 
    }

    private double GetNumberFromScreen()
    {
        if (double.TryParse(ResultLabel.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
        {
            return result;
        }
        return 0;
    }

    private void OnSecretCloseClicked(object sender, EventArgs e)
    {
        SecretOverlay.IsVisible = false;
    }

    private void SetNumberToScreen(double number)
    {
        ResultLabel.Text = number.ToString(CultureInfo.InvariantCulture);
        if (number == 67)
        {
            SecretOverlay.IsVisible = true; 
        }
    }

    private async void OnNumberClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await AnimateButton(button);

        string pressedNumber = button.Text;

        if (isOperatorClicked)
        {
            ResultLabel.Text = "0";
            isOperatorClicked = false;
        }

    
        if (ResultLabel.Text == "0" && pressedNumber != ".")
        {
           
            ResultLabel.Text = pressedNumber;
        }
        else
        {
           
            if (pressedNumber == "." && ResultLabel.Text.Contains("."))
            {
                return;
            }

            ResultLabel.Text += pressedNumber;
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

       
        double number = GetNumberFromScreen();
        number = number * -1;
        SetNumberToScreen(number);
    }

   
    private async void OnCalculateClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await AnimateButton(button);

  
        if (string.IsNullOrEmpty(mathOperator))
            return;

        double secondNumber = GetNumberFromScreen();
        double result = 0;

        switch (mathOperator)
        {
            case "+":
                result = firstNumber + secondNumber;
                break;
            case "-":
                result = firstNumber - secondNumber;
                break;
            case "X":
                result = firstNumber * secondNumber;
                break;
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
        Button button = (Button)sender;
        await AnimateButton(button);

        ResultLabel.Text = "0";
        firstNumber = 0;
        mathOperator = "";
        isOperatorClicked = false;
    }

    private async void OnPercentClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await AnimateButton(button);

        double number = GetNumberFromScreen();

    

        if (!string.IsNullOrEmpty(mathOperator) && firstNumber != 0)
        {
           
            number = firstNumber * (number / 100);
        }
        else
        {
            
            number = number / 100;
        }

        SetNumberToScreen(number);
    }
}