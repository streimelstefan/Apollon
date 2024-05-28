using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Logging
{
    /// <summary>
    /// A logger that does not log anything.
    /// </summary>
    public class VoidLogger : ILogger
    {
        /// <summary>
        /// Gets or sets the recursion depth of the logger.
        /// </summary>
        public int RecursionDepth 
        { 
            get
            {
                return 0;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the loglevel of the logger.
        /// </summary>
        public LogLevel Level { 
            get 
            {
                return LogLevel.Trace;
            } 

            set 
            { 
            } 
        }


        /// <summary>
        /// Creates a child of the current logger.
        /// </summary>
        /// <returns>The child of the current logger.</returns>
        public ILogger CreateChild()
        {
            return new VoidLogger();
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Debug(string message)
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Error(string message)
        {
        }


        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Fatal(string message)
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Info(string message)
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Silly(string message)
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Trace(string message)
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Warn(string message)
        {
        }
    }
}
