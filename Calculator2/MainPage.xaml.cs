using System.Globalization; // Обязательно для работы с точкой

namespace Calculator2;

public partial class MainPage : ContentPage
{
    private void OnMenuClicked(object sender, EventArgs e)
    {
        // Эта команда открывает боковое меню программно
        Shell.Current.FlyoutIsPresented = true;
    }

    // Переменные состояния
    double firstNumber = 0;       // Первое число
    string mathOperator = "";     // Знак (+, -, *, /)
    bool isOperatorClicked = false; // Флаг: было ли нажато действие

    public MainPage()
    {
        InitializeComponent();
    }

    // --- ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ---

    // 1. Анимация нажатия (мигание)
    private async Task AnimateButton(Button button)
    {
        await button.FadeTo(0.5, 100); // Полупрозрачность
        await button.FadeTo(1.0, 100); // Возврат к норме
    }

    // 2. Получить число с экрана (безопасно, через точку)
    private double GetNumberFromScreen()
    {
        // CultureInfo.InvariantCulture заставляет программу ждать точку, а не запятую
        if (double.TryParse(ResultLabel.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
        {
            return result;
        }
        return 0;
    }

    private void OnSecretCloseClicked(object sender, EventArgs e)
    {
        // Скрываем слой с гифкой
        // Убедитесь, что в XAML у Grid стоит x:Name="SecretOverlay"
        SecretOverlay.IsVisible = false;
    }

    // 3. Вывести число на экран (через точку)
    private void SetNumberToScreen(double number)
    {
        ResultLabel.Text = number.ToString(CultureInfo.InvariantCulture);
        if (number == 67)
        {
            SecretOverlay.IsVisible = true; 
        }
    }

    // -----------------------------


    // НАЖАТИЕ НА ЦИФРУ (0-9 и точка)
    private async void OnNumberClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await AnimateButton(button); // Анимация

        string pressedNumber = button.Text;

        // Если до этого нажали (+, -, /), очищаем экран для ввода второго числа
        if (isOperatorClicked)
        {
            ResultLabel.Text = "0";
            isOperatorClicked = false;
        }

        // Логика ввода
        if (ResultLabel.Text == "0" && pressedNumber != ".")
        {
            // Если на экране только 0 и нажали цифру -> заменяем 0
            ResultLabel.Text = pressedNumber;
        }
        else
        {
            // Защита от двух точек (5.5.5)
            if (pressedNumber == "." && ResultLabel.Text.Contains("."))
            {
                return;
            }

            ResultLabel.Text += pressedNumber;
        }
    }

    // НАЖАТИЕ НА ОПЕРАТОР (+, -, X, /, %)
    private async void OnOperatorClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await AnimateButton(button);

        isOperatorClicked = true;
        mathOperator = button.Text;

        // Запоминаем число, которое сейчас на экране
        firstNumber = GetNumberFromScreen();
    }

    // НАЖАТИЕ НА +/- (Смена знака)
    private async void OnNegativeClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await AnimateButton(button);

        // Если нажали +/- сразу после выбора действия (например "5 + +/-")
        // Начинаем ввод второго числа сразу с минуса
        if (isOperatorClicked)
        {
            ResultLabel.Text = "-0";
            isOperatorClicked = false;
            return;
        }

        // Обычный сценарий: берем число, умножаем на -1, возвращаем
        double number = GetNumberFromScreen();
        number = number * -1;
        SetNumberToScreen(number);
    }

    // НАЖАТИЕ НА РАВНО (=)
    private async void OnCalculateClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await AnimateButton(button);

        // Если оператор пустой, ничего не считаем
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
                    ResultLabel.Text = "Error"; // Деление на 0
                    return;
                }
                break;
        }

        // Выводим результат
        SetNumberToScreen(result);

        // Сохраняем результат как первое число для следующих действий
        firstNumber = result;

        // Сбрасываем оператор, чтобы повторное нажатие "=" не ломало логику
        mathOperator = "";
    }

    // НАЖАТИЕ НА C (Очистка)
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

        // Логика: если мы вводим второе число (например: 100 + 10%), 
        // то 10% должно считаться от первого числа (от 100).
        // Если просто ввели число и нажали %, то просто делим на 100.

        if (!string.IsNullOrEmpty(mathOperator) && firstNumber != 0)
        {
            // Сложный процент (скидка/налог)
            // Пример: 100 + 10% -> 10% от 100 это 10.
            // Число на экране (10) превращаем в (100 * 0.1 = 10)
            number = firstNumber * (number / 100);
        }
        else
        {
            // Простой процент (перевод в дробь)
            // Пример: 50% -> 0.5
            number = number / 100;
        }

        SetNumberToScreen(number);
    }
}