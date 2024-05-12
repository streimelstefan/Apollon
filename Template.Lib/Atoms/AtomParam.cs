using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Atoms
{
    public class AtomParam : IEquatable<AtomParam>, ICloneable
    {
        public Literal? Literal { get; private set; }
        public Term? Term { get; set; }

        public bool IsLiteral { get {  return Literal != null; } }

        public bool IsTerm { get { return Term != null; } }

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

        public AtomParam(Literal literal)
        {
            if (literal != null && literal.IsNAF)
            {
                throw new ArgumentException("Literal is not allowed to be NAF.");
            }

            Literal = literal;
        }

        public AtomParam(Term term)
        {
            Term = term;
        }

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
    }
}
