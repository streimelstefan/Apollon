//-----------------------------------------------------------------------
// <copyright file="IDisagreementFinder.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Unification.DisagreementFinders
{
    using Apollon.Lib.Rules;

    /// <summary>
    /// The interface for a DisagreementFinder.
    /// </summary>
    public interface IDisagreementFinder
    {
        /// <summary>
        /// Returns the first disagreement that was found between the two statments. If there is an disagreement that is not
        /// fixable the maybe will be error. A disagreement is not fixable if the disagreement is not part of an <see cref="Atoms.AtomParam"/>.
        /// </summary>
        /// <param name="s1">The first statement.</param>
        /// <param name="s2">The second statement.</param>
        /// <returns>A result with the result of the Disagreement.</returns>
        DisagreementResult FindDisagreement(Statement s1, Statement s2);
    }
}
