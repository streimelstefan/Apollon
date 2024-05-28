using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Atoms
{
    /// <summary>
    /// A parameter of an atom. It can be either a literal or a term.
    /// </summary>
    public class AtomParam : IEquatable<AtomParam>, ICloneable
    {
        /// <summary>
        /// Gets the literal representation of the parameter.
        /// Is only set if the parameter is a literal.
        /// </summary>
        public Literal? Literal { get; private set; }

        /// <summary>
        /// Gets the term representation of the parameter.
        /// Is only set if the parameter is a term.
        /// </summary>
        public Term? Term { get; set; }

        /// <summary>
        /// Gets whether the parameter is a literal.
        /// </summary>
        public bool IsLiteral { get {  return Literal != null; } }

        /// <summary>
        /// Gets whether the parameter is a term.
        /// </summary>
        public bool IsTerm { get { return Term != null; } }

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

            Literal = literal;
            Term = term;
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

            Literal = literal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomParam"/> class.
        /// </summary>
        /// <param name="term">The term representing the parameter.</param>
        public AtomParam(Term term)
        {
            Term = term;
        }

        /// <summary>
        /// Returns whether the current instance is equal to the other parameter.
        /// </summary>
        /// <param name="other">The other parameter to compare with.</param>
        /// <returns>Whether the current isntance is equal to the other parameter.</returns>
        public bool Equals(AtomParam? other)
        {
            if (other  == null) return false;

            if (this == other) return true;

            if (other.IsTerm && !IsTerm) return false;
            if (other.IsLiteral && !IsLiteral) return false;
        
            if (Literal != null)
            {
                return Literal.Equals(other.Literal);
            }

            if (Term != null)
            {
                return Term.Equals(other.Term);
            }

            return false;
        }

        /// <summary>
        /// Returns the string representation of the parameter.
        /// </summary>
        /// <returns>The string representation of the parameter.</returns>
        public override string ToString()
        {
            if (Literal != null)
            {
                return Literal.ToString();
            } else if (Term != null)
            {
                return Term.ToString();
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
            if (Literal != null)
            {
                return new AtomParam((Literal)Literal.Clone(), null);
            }
            if (Term != null)
            {
                return new AtomParam(null, (Term)Term.Clone());
            }

            throw new Exception("Unable to clone param that is neither an atom nor an term.");
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
