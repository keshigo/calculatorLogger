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
            new FileLogger("file.txt")
            });
        ;
        Calculator calculator = new Calculator(compositeLogger);

        Console.WriteLine("* * " + new string('-', 25) + "* *");
        Console.WriteLine("Калькулятор \nОперации: \n+ \n- \n* \n/ \n% \n^ \n\nВвод число затем операция затем второе число");
        Console.WriteLine("* * " + new string('-', 25) + "* *");
        Console.WriteLine("help - справка \n\nexit выход");
        Console.WriteLine("* * " + new string('-', 25) + "* *");

        while (true)
        {
            string userInput = Console.ReadLine().Trim().ToLower();

            if (userInput == "exit")

                break;


            if (userInput == "help")
            {
                System.Console.WriteLine("Справка \nВвод число затем операция затем второе число \n\nОперации: \n+ \n- \n* \n/ \n% \n^ \n\nhelp - справка \n\nexit выход");
                continue;
            }

            try
            {

                /*
                System.Console.WriteLine("Вв а");
                string string1 = Console.ReadLine();
                System.Console.WriteLine("Вв б");
                string string2 = Console.ReadLine();
                System.Console.WriteLine("Вв оп");
                string string3 = Console.ReadLine();
                double a = double.Parse(string1);
                double b = double.Parse(string2);
                char operation = char.Parse(string3);
                */

                string[] strings = userInput.Split(' ');
                double a = double.Parse(strings[0]);
                char operation = char.Parse(strings[1]);
                double b = double.Parse(strings[2]);


                Console.WriteLine($"Результат операции: {calculator.Calculate(a, b, operation)}");
            }
            catch (Exception ex)
            {
                compositeLogger.Error(ex, "Ошибка ввода");
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }


    }

    class Calculator
    {
        private readonly ILogger _logger;
        private readonly Dictionary<char, Func<double, double, double>> _supportedOperations;
        //= new Dictionary<char, Func<double, double, double>>()
        public Calculator(ILogger logger)
        {
            _logger = logger;
            _supportedOperations = new Dictionary<char, Func<double, double, double>>()
            {

                { '+', (a, b) => a + b },
                { '-', (a, b) => a - b },
                { '*', (a, b) => a * b },
                { '/', Divide }, //(a, b) => a / b
                { '%', Module }, //(a, b) => a % b
                { '^', (a, b) => Math.Pow(a, b) }
            };
        }

        public double Calculate(double a, double b, char operation)
        {
            if (!_supportedOperations.ContainsKey(operation))
                throw new ArgumentException($"Неподдерживаемая операция: {operation}");

            return _supportedOperations[operation](a, b);
        }
        private double Divide(double a, double b)
        {
            if (b == 0) throw new DivideByZeroException("Деление на ноль запрещено");
            return a / b;
        }

        private double Module(double a, double b)
        {
            if (b == 0) throw new ArgumentException("Деление по модулю на ноль");
            return a % b;
        }


    }

}