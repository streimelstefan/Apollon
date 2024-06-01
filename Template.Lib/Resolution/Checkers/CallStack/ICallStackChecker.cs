//-----------------------------------------------------------------------
// <copyright file="ICallStackChecker.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Resolution.Checkers.CallStack
{
    /// <summary>
    /// The CallStackChecker checks the CallStack for loops.
    /// </summary>
    public interface ICallStackChecker
    {
        /// <summary>
        /// Checks the CallStack for loops.
        /// </summary>
        /// <param name="literal">The Literal that should be checked.</param>
        /// <param name="stack">The Stack of Literals that should be checked.</param>
        /// <returns>Returns an Enumerable containing the Result of the Check.</returns>
        CheckerResult CheckCallStackFor(Literal literal, Stack<Literal> stack);
    }
}
