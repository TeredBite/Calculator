using System.Globalization;

namespace Calculator2;

public partial class EngineeringPage : ContentPage
{

    private void OnMenuClicked(object sender, EventArgs e)
    {
        // Эта команда открывает боковое меню программно
        Shell.Current.FlyoutIsPresented = true;
    }

    double firstNumber = 0;
    string mathOperator = "";
    bool isOperatorClicked = false;

    public EngineeringPage()
    {
        InitializeComponent();
    }

    // --- НАВИГАЦИЯ НАЗАД ---
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); // Возврат на главную
    }

    // --- ПОМОЩНИКИ ---
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
        // Округляем до 8 знаков, чтобы sin(pi) не давал 0.0000000000123
        double rounded = Math.Round(number, 8);
        ResultLabel.Text = rounded.ToString(CultureInfo.InvariantCulture);
    }

    // --- БАЗОВАЯ ЛОГИКА (Копия MainPage) ---
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

        if (ResultLabel.Text == "0" && pressed != ".")
            ResultLabel.Text = pressed;
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
            case "/": result = secondNumber != 0 ? firstNumber / secondNumber : 0; break;
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
        await AnimateButton((Button)sender);
        double val = GetNumberFromScreen();
        SetNumberToScreen(val * -1);
    }

    // --- НОВАЯ ИНЖЕНЕРНАЯ ЛОГИКА ---

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
                // Считаем в градусах (умножаем на PI/180)
                result = Math.Sin(currentVal * Math.PI / 180);
                break;
            case "cos":
                result = Math.Cos(currentVal * Math.PI / 180);
                break;
            case "tan":
                result = Math.Tan(currentVal * Math.PI / 180);
                break;
            case "log":
                result = Math.Log10(currentVal); // Десятичный логарифм
                break;
            case "ln":
                result = Math.Log(currentVal); // Натуральный логарифм
                break;
            case "√":
                if (currentVal >= 0)
                    result = Math.Sqrt(currentVal);
                else
                {
                    ResultLabel.Text = "Error"; // Корень из отрицательного нельзя
                    return;
                }
                break;
            case "x²":
                result = Math.Pow(currentVal, 2);
                break;
            case "%":
                result = currentVal / 100;
                break;
        }

        SetNumberToScreen(result);
        // Сразу сохраняем результат как начало следующего действия
        firstNumber = result;
        isOperatorClicked = true; // Чтобы следующая цифра начала новый ввод
    }

    // Кнопки ПИ и E
    private async void OnPiClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await AnimateButton(button);

        if (button.Text == "π")
            SetNumberToScreen(Math.PI);
        else // e
            SetNumberToScreen(Math.E);

        isOperatorClicked = false; // Это как ввод цифры
    }
}