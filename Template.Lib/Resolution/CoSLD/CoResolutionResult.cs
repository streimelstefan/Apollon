//-----------------------------------------------------------------------
// <copyright file="CoResolutionResult.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Resolution.CoSLD
{
    using Apollon.Lib.Logging;
    using Apollon.Lib.Resolution.CallStackAndCHS;
    using Apollon.Lib.Resolution.CoSLD.States;
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// The CoResolution Result.
    /// </summary>
    public class CoResolutionResult : ResolutionResult
    {
        private ResolutionBaseState state;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoResolutionResult"/> class.
        /// </summary>
        /// <param name="success">Whether or not the Resolution was a success.</param>
        /// <param name="substitution">The substitution that was used.</param>
        /// <param name="state">The base State.</param>
        public CoResolutionResult(bool success, Substitution substitution, ResolutionBaseState state)
            : base(success, state.Chs, substitution)
        {
            // the warning in this constructor is just false. Idk where it gets that form...
            // State gets set and checked for null...
            this.State = state;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoResolutionResult"/> class.
        /// </summary>
        public CoResolutionResult()
            : base(false, new CHS(), new Substitution())
        {
            // the warning in this constructor is just false. Idk where it gets that form...
            // State gets set and checked for null...
            this.State = new ResolutionBaseState(new Rules.Statement[0], new Stack<Literal>(), new CHS(), new Substitution(), new VoidLogger());
        }

        /// <summary>
        /// Gets the Base State.
        /// </summary>
        public ResolutionBaseState State
        {
            get => this.state;

            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(this.State));
                this.state = value;
            }
        }
    }
}
