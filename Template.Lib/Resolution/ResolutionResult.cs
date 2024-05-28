namespace Apollon.Lib.Resolution
{
    using Apollon.Lib.Resolution.CallStackAndCHS;
    using Apollon.Lib.Unification;
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// The Result of a Resolution process.
    /// </summary>
    public class ResolutionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionResult"/> class.
        /// </summary>
        /// <param name="chs">The CHS that was the result of the Resolution.</param>
        /// <param name="substitution">The Substitution that was used.</param>
        public ResolutionResult(CHS chs, Substitution substitution)
            : this(!chs.IsEmpty, chs, substitution)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionResult"/> class.
        /// </summary>
        /// <param name="success">Whether or not the Resolution was a success.</param>
        /// <param name="chs">The CHS that was the result of the Resolution.</param>
        /// <param name="substitution">The Substitution that was used.</param>
        public ResolutionResult(bool success, CHS chs, Substitution substitution)
        {
            ArgumentNullException.ThrowIfNull(chs);
            ArgumentNullException.ThrowIfNull(substitution);
            this.Success = success;
            this.CHS = chs;
            this.Substitution = substitution;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolutionResult"/> class.
        /// </summary>
        public ResolutionResult()
            : this(new CHS(), new Substitution())
        {
        }

        /// <summary>
        /// Gets a value indicating whether or not the Resolution was a success.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Gets the CHS that was the result of the Resolution.
        /// </summary>
        public CHS CHS { get; private set; }

        /// <summary>
        /// Gets the Substitution that was used.
        /// </summary>
        public Substitution Substitution { get; private set; }
    }
}
