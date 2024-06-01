//-----------------------------------------------------------------------
// <copyright file="AtomParam.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Atoms
{
    /// <summary>
    /// A parameter of an atom. It can be either a literal or a term.
    /// </summary>
    public class AtomParam : IEquatable<AtomParam>, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomParam"/> class.
        /// Only literal or term are allowed to be set. At least one mus be set.
        /// </summary>
        /// <param name="literal">The literal that represents the parameter.</param>
        /// <param name="term">The term that represents the parameter.</param>
        /// <exception cref="ArgumentException">Is thrown if literal and term are set or when both are not set.</exception>
        public AtomParam(Literal? literal, Term? term)
        {
            if (literal == null && term == null)
            {
                throw new ArgumentException("Literal and Term are not allowed to be null at the same time.");
            }

            if (literal != null && term != null)
            {
                throw new ArgumentException("Literal and Term are not allowed to be set at the same time.");
            }

            if (literal != null && literal.IsNAF)
            {
                throw new ArgumentException("Literal is not allowed to be NAF.");
            }

            this.Literal = literal;
            this.Term = term;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomParam"/> class.
        /// </summary>
        /// <param name="literal">The literal that represents the parameter.</param>
        /// <exception cref="ArgumentException">Is thrown if the literal is naf.</exception>
        public AtomParam(Literal literal)
        {
            if (literal != null && literal.IsNAF)
            {
                throw new ArgumentException("Literal is not allowed to be NAF.");
            }

            this.Literal = literal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomParam"/> class.
        /// </summary>
        /// <param name="term">The term representing the parameter.</param>
        public AtomParam(Term term)
        {
            this.Term = term;
        }

        /// <summary>
        /// Gets the literal representation of the parameter.
        /// Is only set if the parameter is a literal.
        /// </summary>
        public Literal? Literal { get; private set; }

        /// <summary>
        /// Gets or sets the term representation of the parameter.
        /// Is only set if the parameter is a term.
        /// </summary>
        public Term? Term { get; set; }

        /// <summary>
        /// Gets a value indicating whether gets whether the parameter is a literal.
        /// </summary>
        public bool IsLiteral => this.Literal != null;

        /// <summary>
        /// Gets a value indicating whether gets whether the parameter is a term.
        /// </summary>
        public bool IsTerm => this.Term != null;

        /// <summary>
        /// Returns whether the current instance is equal to the other parameter.
        /// </summary>
        /// <param name="other">The other parameter to compare with.</param>
        /// <returns>Whether the current isntance is equal to the other parameter.</returns>
        public bool Equals(AtomParam? other)
        {
            return other != null
&& (this == other
|| ((!other.IsTerm || this.IsTerm)
&& (!other.IsLiteral || this.IsLiteral)
&& (this.Literal != null ? this.Literal.Equals(other.Literal) : this.Term != null && this.Term.Equals(other.Term))));
        }

        /// <summary>
        /// Returns the string representation of the parameter.
        /// </summary>
        /// <returns>The string representation of the parameter.</returns>
        public override string ToString()
        {
            if (this.Literal != null)
            {
                return this.Literal.ToString();
            }
            else if (this.Term != null)
            {
                return this.Term.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Retunrs a clone of the current instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        /// <exception cref="Exception">Is thrown if the parameter is neither a term nor a literal.</exception>
        public object Clone()
        {
            return this.Literal != null
                ? new AtomParam((Literal)this.Literal.Clone(), null)
                : this.Term != null
                ? (object)new AtomParam(null, (Term)this.Term.Clone())
                : throw new Exception("Unable to clone param that is neither an atom nor an term.");
        }

        /// <summary>
        /// Converts the parameter to a term if possible.
        /// </summary>
        public void ConvertToTermIfPossible()
        {
            if (this.Literal == null || this.Literal.Atom.ParamList.Count() != 0 || this.Literal.IsNAF || this.Literal.IsNegative)
            {
                return;
            }

            this.Term = new Term(this.Literal.Atom.Name);
            this.Literal = null;
        }
    }
}
