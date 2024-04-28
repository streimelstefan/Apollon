using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Atoms
{
    public class AtomParam : IEquatable<AtomParam>, ICloneable
    {
        public Atom? Atom { get; private set; }
        public Term? Term { get; private set; }

        public bool IsAtom { get {  return Atom != null; } }

        public bool IsTerm { get { return Term != null; } }

        public AtomParam(Atom? atom, Term? term) 
        { 
            if (atom == null && term == null)
            {
                throw new ArgumentException("Atom and Term are not allowed to be null at the same time.");
            }
            if (atom != null && term != null)
            {
                throw new ArgumentException("Atom and Term are not allowed to be set at the same time.");
            }

            Atom = atom;
            Term = term;
        }

        public bool Equals(AtomParam? other)
        {
            if (other  == null) return false;

            if (this == other) return true;

            if (other.IsTerm && !IsTerm) return false;
            if (other.IsAtom && !IsAtom) return false;
        
            if (Atom != null)
            {
                return Atom.Equals(other.Atom);
            }

            if (Term != null)
            {
                return Term.Equals(other.Term);
            }

            return false;
        }

        public override string ToString()
        {
            if (Atom != null)
            {
                return Atom.ToString();
            } else if (Term != null)
            {
                return Term.ToString();
            }

            return string.Empty;
        }

        public object Clone()
        {
            if (Atom != null)
            {
                return new AtomParam((Atom)Atom.Clone(), null);
            }
            if (Term != null)
            {
                return new AtomParam(null, (Term)Term.Clone());
            }

            throw new Exception("Unable to clone param that is neither an atom nor an term.");
        }
    }
}
