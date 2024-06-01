//-----------------------------------------------------------------------
// <copyright file="ConsoleLogger.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

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
            ConsoleLogger newLogger = new()
            {
                Level = this.Level,
                RecursionDepth = this.RecursionDepth,
            };

            return newLogger;
        }

        /// <summary>
        /// Logs a debug message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Debug(string message)
        {
            if (this.Level > LogLevel.Debug)
            {
                return;
            }

            Console.WriteLine($"[DEBUG]:{new string(' ', this.RecursionDepth * 2)} {message}");
        }

        /// <summary>
        /// Logs a error message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Error(string message)
        {
            if (this.Level > LogLevel.Error)
            {
                return;
            }

            Console.WriteLine($"[ERROR]:{new string(' ', this.RecursionDepth * 2)} {message}");
        }

        /// <summary>
        /// Logs a fatal message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Fatal(string message)
        {
            if (this.Level > LogLevel.Fatal)
            {
                return;
            }

            Console.WriteLine($"[FATAL]:{new string(' ', this.RecursionDepth * 2)} {message}");
        }

        /// <summary>
        /// Logs a info message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Info(string message)
        {
            if (this.Level > LogLevel.Info)
            {
                return;
            }

            Console.WriteLine($"[INFO ]:{new string(' ', this.RecursionDepth * 2)} {message}");
        }

        /// <summary>
        /// Logs a silly message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Silly(string message)
        {
            if (this.Level > LogLevel.Silly)
            {
                return;
            }

            Console.WriteLine($"[SILLY]:{new string(' ', this.RecursionDepth * 2)} |+| {message}");
        }

        /// <summary>
        /// Logs a trace message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Trace(string message)
        {
            if (this.Level > LogLevel.Trace)
            {
                return;
            }

            Console.WriteLine($"[TRACE]:{new string(' ', this.RecursionDepth * 2)} {message}");
        }

        /// <summary>
        /// Logs a warning message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Warn(string message)
        {
            if (this.Level > LogLevel.Warn)
            {
                return;
            }

            Console.WriteLine($"[WARN ]:{new string(' ', this.RecursionDepth * 2)} {message}");
        }
    }
}
