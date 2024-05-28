using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Logging
{
    /// <summary>
    /// A logger that logs messages specifically designed for the Apollon project.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets or sets the level of the logger.
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the recursion depth of the logger.
        /// </summary>
        public int RecursionDepth { get; set; }

        /// <summary>
        /// Logs a silly message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Silly(string message);

        /// <summary>
        /// Logs a trace message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Trace(string message);

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Debug(string message);

        /// <summary>
        /// Logs a info message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Info(string message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Warn(string message);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Error(string message);

        /// <summary>
        /// Logs a fatal message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Fatal(string message);

        /// <summary>
        /// Creates a child of the current logger.
        /// </summary>
        /// <returns>The child of the current logger.</returns>
        public ILogger CreateChild();
    }
}
