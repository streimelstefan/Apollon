//-----------------------------------------------------------------------
// <copyright file="IResolution.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Resolution
{
    using Apollon.Lib.Logging;
    using Apollon.Lib.Rules;

    /// <summary>
    /// The interface for Resolution.
    /// </summary>
    public interface IResolution
    {
        /// <summary>
        /// The function that should be used when trying to resolve a set of Statements and Goals.
        /// </summary>
        /// <param name="statements">The statements that should be resolved.</param>
        /// <param name="goals">The goals for the resolution.</param>
        /// <param name="logger">The logger that should be used for the logging process.</param>
        /// <returns>The Result of the Resolution.</returns>
        IEnumerable<ResolutionResult> Resolute(Statement[] statements, BodyPart[] goals, ILogger logger);
    }
}
