//-----------------------------------------------------------------------
// <copyright file="ICoinductiveCHSChecker.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Resolution.Checkers.CHSCheckers
{
    using Apollon.Lib.Resolution.CallStackAndCHS;
    using Apollon.Lib.Resolution.CoSLD.States;
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// The CHS Checker checks the CHS for loops.
    /// </summary>
    public interface ICoinductiveCHSChecker
    {
        /// <summary>
        /// Checks the CHS for instances where the Co SLD resolution can succeed or fail early.
        /// </summary>
        /// <param name="literal">The literal to check.</param>
        /// <param name="chs">The CHS to check/.</param>
        /// <returns>What the Co SLD resolution should do.</returns>
        CheckerResult CheckCHSFor(Literal literal, CHS chs, ResolutionLiteralState state);
    }
}
