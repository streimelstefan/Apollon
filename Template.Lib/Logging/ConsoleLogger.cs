using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Logging
{
    public class ConsoleLogger : ILogger
    {
        public LogLevel Level { get; set; } = LogLevel.NoLogging;
        public int RecursionDepth { get; set; }

        public ILogger CreateChild()
        {
            var newLogger = new ConsoleLogger();
            newLogger.Level = Level;
            newLogger.RecursionDepth = RecursionDepth;

            return newLogger;
        }

        public void Debug(string message)
        {
            if (Level > LogLevel.Debug)
            {
                return;
            }
            Console.WriteLine($"[DEBUG]:{new string(' ', RecursionDepth * 2)} {message}");
        }

        public void Error(string message)
        {
            if (Level > LogLevel.Error)
            {
                return;
            }
            Console.WriteLine($"[ERROR]:{new string(' ', RecursionDepth * 2)} {message}");
        }

        public void Fatal(string message)
        {
            if (Level > LogLevel.Fatal)
            {
                return;
            }
            Console.WriteLine($"[FATAL]:{new string(' ', RecursionDepth * 2)} {message}");
        }

        public void Info(string message)
        {
            if (Level > LogLevel.Info)
            {
                return;
            }
            Console.WriteLine($"[INFO ]:{new string(' ', RecursionDepth * 2)} {message}");
        }

        public void Trace(string message)
        {
            if (Level > LogLevel.Trace)
            {
                return;
            }
            Console.WriteLine($"[TRACE]:{new string(' ', RecursionDepth * 2)} {message}");
        }

        public void Warn(string message)
        {
            if (Level > LogLevel.Warn)
            {
                return;
            }
            Console.WriteLine($"[WARN ]:{new string(' ', RecursionDepth * 2)} {message}");
        }
    }
}
