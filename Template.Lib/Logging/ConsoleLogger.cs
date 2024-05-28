using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Logging
{
    /// <summary>
    /// Logs messages to the console.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Gets or sets the log level of the logger.
        /// </summary>
        public LogLevel Level { get; set; } = LogLevel.NoLogging;

        /// <summary>
        /// Gets or sets the recursion depth of the logger.
        /// </summary>
        public int RecursionDepth { get; set; }

        /// <summary>
        /// Creates a child of the current logger.
        /// </summary>
        /// <returns>The child of the current logger.</returns>
        public ILogger CreateChild()
        {
            var newLogger = new ConsoleLogger();
            newLogger.Level = Level;
            newLogger.RecursionDepth = RecursionDepth;

            return newLogger;
        }

        /// <summary>
        /// Logs a debug message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Debug(string message)
        {
            if (Level > LogLevel.Debug)
            {
                return;
            }
            Console.WriteLine($"[DEBUG]:{new string(' ', RecursionDepth * 2)} {message}");
        }

        /// <summary>
        /// Logs a error message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Error(string message)
        {
            if (Level > LogLevel.Error)
            {
                return;
            }
            Console.WriteLine($"[ERROR]:{new string(' ', RecursionDepth * 2)} {message}");
        }

        /// <summary>
        /// Logs a fatal message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Fatal(string message)
        {
            if (Level > LogLevel.Fatal)
            {
                return;
            }
            Console.WriteLine($"[FATAL]:{new string(' ', RecursionDepth * 2)} {message}");
        }

        /// <summary>
        /// Logs a info message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Info(string message)
        {
            if (Level > LogLevel.Info)
            {
                return;
            }
            Console.WriteLine($"[INFO ]:{new string(' ', RecursionDepth * 2)} {message}");
        }

        /// <summary>
        /// Logs a silly message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Silly(string message)
        {
            if (Level > LogLevel.Silly)
            {
                return;
            }
            Console.WriteLine($"[SILLY]:{new string(' ', RecursionDepth * 2)} |+| {message}");
        }

        /// <summary>
        /// Logs a trace message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Trace(string message)
        {
            if (Level > LogLevel.Trace)
            {
                return;
            }
            Console.WriteLine($"[TRACE]:{new string(' ', RecursionDepth * 2)} {message}");
        }

        /// <summary>
        /// Logs a warning message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
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
