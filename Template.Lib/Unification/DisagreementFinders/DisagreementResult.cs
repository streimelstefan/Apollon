//-----------------------------------------------------------------------
// <copyright file="DisagreementResult.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Unification.DisagreementFinders
{
    /// <summary>
    /// The Result of a Disagreement.
    /// </summary>
    public class DisagreementResult : Maybe<Disagreement, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisagreementResult"/> class.
        /// </summary>
        /// <param name="value">The value of the disagreement.</param>
        public DisagreementResult(Disagreement value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisagreementResult"/> class.
        /// </summary>
        /// <param name="error">The error string when disagreement runs into an error.</param>
        public DisagreementResult(string error)
            : base(error)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisagreementResult"/> class.
        /// </summary>
        /// <param name="value">The value of the disagreement.</param>
        /// <param name="error">The error string when disagreement runs into an error.</param>
        public DisagreementResult(Disagreement? value, string? error)
            : base(value, error)
        {
        }
    }
}
