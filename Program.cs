using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using MyNewLogger;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;



class Program
{

    static void Main()
    {

        var compositeLogger = new CompositeLogger(new List<ILogger> {
            new ConsoleLogger(),
            new FileLogger("log.txt")
        });

        Calculator calculator = new Calculator(compositeLogger);

        PrintInstructions();

        while (true)
        {
            string userInput = Console.ReadLine().Trim().ToLower();

            if (userInput == "exit")
                break;

            if (userInput == "help")
            {
                PrintInstructions();
                continue;
            }

            try
            {
                string[] parts = userInput.Split(' ');
                if (parts.Length != 3)
                    throw new ArgumentException("Неверный формат ввода");

                double a = double.Parse(parts[0]);
                char operation = char.Parse(parts[1]);
                double b = double.Parse(parts[2]);

                double result = calculator.Calculate(a, b, operation);
                Console.WriteLine($"Результат: {result}");
            }
            catch (Exception ex)
            {
                compositeLogger.LogError(ex, "Ошибка ввода");
            }
        }
    }
    private static void PrintInstructions()
    {
        Console.WriteLine("* * " + new string('-', 25) + "* *");
        Console.WriteLine("Калькулятор \nОперации: \n+ \n- \n* \n/ \n% \n^ \n\nВвод: число операция число");
        Console.WriteLine("* * " + new string('-', 25) + "* *");
        Console.WriteLine("help - справка \nexit - выход");
        Console.WriteLine("* * " + new string('-', 25) + "* *");
    }
}

class Calculator
{
    private readonly ILogger _logger;
    private readonly Dictionary<char, Func<double, double, double>> _operations;
    //= new Dictionary<char, Func<double, double, double>>()
    public Calculator(ILogger logger)
    {
        _logger = logger;
        _operations = new Dictionary<char, Func<double, double, double>>()
        {
            { '+', Add },
            { '-', Subtract },
            { '*', Multiply },
            { '/', Divide },
            { '%', Modulus },
            { '^', Power }
        };
    }

    public double Calculate(double a, double b, char operation)
    {
        if (!_operations.ContainsKey(operation))
            throw new ArgumentException($"Неподдерживаемая операция: {operation}");

        double result = _operations[operation](a, b);
        _logger.LogInformation($"Выполнено: {a} {operation} {b} = {result}");

        return _operations[operation](a, b);
    }
    private double Add(double a, double b) => a + b;
    private double Subtract(double a, double b) => a - b;
    private double Multiply(double a, double b) => a * b;

    private double Divide(double a, double b)
    {
        if (b == 0)
            throw new DivideByZeroException("Деление на ноль запрещено");
        return a / b;
    }

    private double Modulus(double a, double b)
    {
        if (b == 0)
            throw new DivideByZeroException("Деление по модулю на ноль запрещено");
        return a % b;
    }

    private double Power(double a, double b) => Math.Pow(a, b);



}


