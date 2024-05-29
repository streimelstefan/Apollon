namespace Apollon.Lib.Unification
{
    using Apollon.Lib.Atoms;

    /// <summary>
    /// 
    /// </summary>
    public class Mapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mapping"/> class.
        /// </summary>
        /// <param name="variable">The variable that should be mapped.</param>
        /// <param name="mapsTo">The AtomParam the variable should be mapped to.</param>
        public Mapping(Term variable, AtomParam mapsTo)
        {
            this.Variable = variable;
            this.MapsTo = mapsTo;
        }

        /// <summary>
        /// Gets the Varible that is mapped.
        /// </summary>
        public Term Variable { get; private set; }

        /// <summary>
        /// Gets the AtomParam that the Variable is mapped to.
        /// </summary>
        public AtomParam MapsTo { get; private set; }

        /// <summary>
        /// Converts the Mapping to a string.
        /// </summary>
        /// <returns>A string containing the mapping.</returns>
        public override string ToString()
        {
            return $"{this.Variable} -> {this.MapsTo}";
        }
    }
}
