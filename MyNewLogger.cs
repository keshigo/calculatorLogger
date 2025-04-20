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
        void LogInformation(string message);
        void LogError(Exception ex, string additionalMessage = null);
    }

    public class ConsoleLogger : ILogger
    {
        public void LogInformation(string message)
        {
            Console.WriteLine($"Информация: {message}");
        }

        public void LogError(Exception ex, string additionalMessage = null)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
           // if (!string.IsNullOrEmpty(additionalMessage))
           //     Console.WriteLine($"Дополнительно: {additionalMessage}");
        }
    }

    public class FileLogger : ILogger
    {
        private readonly string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void LogInformation(string message)
        {
            using (StreamWriter writer = new StreamWriter(_filePath, true))
            {
                writer.WriteLine($"[INFO] {message}");
            }
        }

        public void LogError(Exception ex, string additionalMessage = null)
        {
            using (StreamWriter writer = new StreamWriter(_filePath, true))
            {
                writer.WriteLine($"[ERROR] {ex.Message}");
                if (!string.IsNullOrEmpty(additionalMessage))
                    writer.WriteLine($"Дополнительно: {additionalMessage}");
            }
        }
    }

    public class CompositeLogger : ILogger
    {
        private readonly List<ILogger> _loggers;

        public CompositeLogger(List<ILogger> loggers)
        {
            _loggers = loggers;
        }

        public void LogInformation(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.LogInformation(message);
            }
        }

        public void LogError(Exception ex, string additionalMessage = null)
        {
            foreach (var logger in _loggers)
            {
                logger.LogError(ex, additionalMessage);
            }
        }
    }
}
