﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Lib
{
    public class Term : IEquatable<Term>
    {
        public PVL ProhibitedValues { get; set; } // Is this the correct place to put this?

        public bool IsVariable {  
            get
            {
                return Char.IsUpper(Value[0]);
            } 
        }

        public string Value { get; set; }

        public Term(string value) 
        { 
            Value = value;
            ProhibitedValues = new PVL();
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

    }
}
