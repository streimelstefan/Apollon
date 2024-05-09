using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib
{
    public class Term : IEquatable<Term>, ICloneable
    {
        public PVL ProhibitedValues { get; set; } // Is this the correct place to put this?

        public bool IsVariable {  
            get
            {
                return Char.IsUpper(Value[0]);
            } 
        }

        public string Value { get; set; }

        public Term(string value) : this(value, new PVL())
        { 
        }

        public Term(string value, PVL pVL)
        {
            Value = value;
            ProhibitedValues = pVL;
        }

        public override string ToString()
        {
            return Value;
        }


        public bool Equals(Term? other)
        {
            if (other == null) return false;

            return Value == other.Value;
        }

        public bool IsNegativelyConstrained()
        {
            return !ProhibitedValues.Empty(); // It is negatively constrained if it has prohibited values
        }

        public object Clone()
        {
            return new Term((string)Value.Clone(), (PVL)ProhibitedValues.Clone());
        }
    }
}
