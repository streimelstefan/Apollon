//-----------------------------------------------------------------------
// <copyright file="Literal.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib
{
    using Apollon.Lib.Atoms;

    /// <summary>
    /// The Literal class represents a literal in asp.
    /// </summary>
    public class Literal : IEquatable<Literal>, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Literal"/> class.
        /// </summary>
        /// <param name="atom">The atom the literal consists of.</param>
        /// <param name="isNAF">Whether or not the Literal is NAF negated.</param>
        /// <param name="isNegative">Whether or not the Literal is negated.</param>
        public Literal(Atom atom, bool isNAF, bool isNegative)
        {
            this.Atom = atom;
            this.IsNAF = isNAF;
            this.IsNegative = isNegative;
        }

        /// <summary>
        /// Gets or sets the Atom of the Literal.
        /// </summary>
        public Atom Atom { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Literal is NAF negated.
        /// </summary>
        public bool IsNAF { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Literal is negative.
        /// </summary>
        public bool IsNegative { get; set; }

        /// <summary>
        /// Converts the Literal to a String.
        /// </summary>
        /// <returns>A string consisting of the Literal.</returns>
        public override string ToString()
        {
            string nafPrefix = this.IsNAF ? "not " : string.Empty;
            string negativePrefix = this.IsNegative ? "-" : string.Empty;
            return $"{nafPrefix}{negativePrefix}{this.Atom}";
        }

        /// <summary>
        /// Checks whether or not the given Literal is equal to the current Literal.
        /// </summary>
        /// <param name="other">The Literal equality should be checked for.</param>
        /// <returns>A value indicating whether or not the given Literal is equal to the current Literal.</returns>
        public bool Equals(Literal? other)
        {
            return other != null && this.IsNAF == other.IsNAF && this.IsNegative == other.IsNegative && this.Atom.Equals(other.Atom);
        }

        /// <summary>
        /// Clones the Literal.
        /// </summary>
        /// <returns>A object consisting of the cloned literal.</returns>
        public object Clone()
        {
            return new Literal((Atom)this.Atom.Clone(), this.IsNAF, this.IsNegative);
        }
    }
}
