//-----------------------------------------------------------------------
// <copyright file="UnificationResult.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Unification
{
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// The Result of a Unification process.
    /// </summary>
    public class UnificationResult : Maybe<Substitution, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnificationResult"/> class.
        /// </summary>
        /// <param name="value">The Substitution that is used.</param>
        public UnificationResult(Substitution value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnificationResult"/> class.
        /// </summary>
        /// <param name="error">A string representing the error message.</param>
        public UnificationResult(string error)
            : base(error)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnificationResult"/> class.
        /// </summary>
        /// <param name="value">The Substitution that is used.</param>
        /// <param name="error">A string representing the error message.</param>
        public UnificationResult(Substitution? value, string? error)
            : base(value, error)
        {
        }
    }
}
