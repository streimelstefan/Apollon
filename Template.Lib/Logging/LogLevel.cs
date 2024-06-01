//-----------------------------------------------------------------------
// <copyright file="LogLevel.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Logging
{
    /// <summary>
    /// Represents the level of logging.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Logs all messages.
        /// </summary>
        Silly,

        /// <summary>
        /// Includes all messages from the previous level and adds trace messages.
        /// </summary>
        Trace,

        /// <summary>
        /// Includes all messages from the previous level and adds debug messages.
        /// </summary>
        Debug,

        /// <summary>
        /// Includes all messages from the previous level and adds info messages.
        /// </summary>
        Info,

        /// <summary>
        /// Includes all messages from the previous level and adds warning messages.
        /// </summary>
        Warn,

        /// <summary>
        /// Includes all messages from the previous level and adds error messages.
        /// </summary>
        Error,

        /// <summary>
        /// Only logs fatal logs.
        /// </summary>
        Fatal,

        /// <summary>
        /// No messages are logged.
        /// </summary>
        NoLogging,
    }
}
