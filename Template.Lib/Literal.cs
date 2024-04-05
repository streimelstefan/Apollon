﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Lib
{
    public class Literal
    {
        public Atom Atom { get; set; }

        public bool IsNAF { get; set; }

        public bool IsNegative { get; set; }

        public Literal(Atom atom, bool isNAF, bool isNegative) 
        {
            Atom = atom;
            IsNAF = isNAF;
            IsNegative = isNegative;
        }

        public override string ToString()
        {
            var nafPrefix = IsNAF ? "not " : "";
            var negativePrefix = IsNegative ? "-" : "";
            return $"{nafPrefix}{negativePrefix}{Atom}";
        }


        public bool Equals(Literal other)
        {
            return IsNAF == other.IsNAF && IsNegative == other.IsNegative && Atom.Equals(other.Atom);
        }
    }
}