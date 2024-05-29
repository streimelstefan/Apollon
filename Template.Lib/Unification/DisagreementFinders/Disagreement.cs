namespace Apollon.Lib.Unification.DisagreementFinders
{
    using Apollon.Lib.Atoms;

    /// <summary>
    /// Represents a disagreement between two AtomParams.
    /// </summary>
    public class Disagreement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Disagreement"/> class.
        /// </summary>
        /// <param name="first">The first AtomParam.</param>
        /// <param name="second">The second AtomParam.</param>
        public Disagreement(AtomParam first, AtomParam second)
        {
            this.First = first;
            this.Second = second;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Disagreement"/> class.
        /// </summary>
        public Disagreement()
        {
        }

        /// <summary>
        /// Gets the first AtomParam.
        /// </summary>
        public AtomParam? First { get; private set; }

        /// <summary>
        /// Gets the second AtomParam.
        /// </summary>
        public AtomParam? Second { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the Disagreement is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.First == null && this.Second == null;
            }
        }
    }
}
