using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace MyNewLogger
{

    public interface ILogger
    {
        void Info(string message);
        void Error(Exception ex, string additionalMessage = null);
    }
    public class ConsoleLogger : ILogger
    {
        public void Info(string message)
        {
            Console.WriteLine($"чипи чипи {message}");
        }
        public void Error(Exception ex, string additionalMessage = null)
        {
            Console.WriteLine($"ПиЗдАтО{ex.Message}");
        }


    }
    public class FileLogger : ILogger
    {
        private readonly string _leva;
        public FileLogger(string janDydka)
        {
            _leva = janDydka;
        }
        public void Info(string message)
        {
            using (StreamWriter writer = new StreamWriter(_leva, true))
            { writer.WriteLine($"{message}"); }
        }
        public void Error(Exception ex, string additionalMessage = null)
        {
            using (StreamWriter writer = new StreamWriter(_leva, true))
            {
                writer.WriteLine($"{ex.Message}");
                if (additionalMessage != null)
                {
                    writer.WriteLine(additionalMessage);
                }
            }

        }
    }
    public class CompositeLogger : ILogger 
    {
        private readonly List <ILogger> _loggers;
        public CompositeLogger(List<ILogger> loggers)
        {
            _loggers = loggers;
        }

        public void Info(string message)
        {
            foreach (var logger in _loggers)
            {
                try
                {
                    logger.Info(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }
        public void Error(Exception ex, string additionalMessage = null)
        {
            foreach (var logger in _loggers)
            {
                try
                {
                    logger.Error(ex, additionalMessage);
                }
                catch (Exception ex1)
                {
                    Console.WriteLine($"{ex1.Message}");
                }
            }



        }


    }
}