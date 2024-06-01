//-----------------------------------------------------------------------
// <copyright file="CoResolutionForAllResult.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Resolution.CoSLD
{
    using Apollon.Lib.Resolution.CoSLD.States;
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// The CoResolution Result for the ForAll Quantifier.
    /// </summary>
    public class CoResolutionForAllResult : CoResolutionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoResolutionForAllResult"/> class.
        /// </summary>
        /// <param name="success">Whether or not the resolution was a success.</param>
        /// <param name="substitution">The substitution that was used.</param>
        /// <param name="state">The Resolution that was currently used.</param>
        /// <param name="realGoal">The real Goal that was identified.</param>
        /// <exception cref="ArgumentNullException">Is thrown if success and realGoal is null.</exception>
        public CoResolutionForAllResult(bool success, Substitution substitution, ResolutionBaseState state, Literal? realGoal)
            : base(success, substitution, state)
        {
            if (success && realGoal == null)
            {
                throw new ArgumentNullException(nameof(realGoal));
            }

            this.RealGoal = realGoal;
        }

        /// <summary>
        /// Gets the Literal representing the real Goal.
        /// </summary>
        public Literal? RealGoal { get; private set; }
    }
}
