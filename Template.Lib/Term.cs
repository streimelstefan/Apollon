using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Lib
{
    public class Term
    {

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
        }

        public override string ToString()
        {
            return Value;
        }


        public bool Equals(Term other)
        {
            return Value == other.Value;
        }

    }
}
