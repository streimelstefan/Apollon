using Apollon.Lib.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Unification
{
    public class Mapping
    {

        public Term Variable { get; private set; }

        public AtomParam MapsTo { get; private set; }

        public Mapping(Term variable, AtomParam mapsTo) 
        { 
            Variable = variable;
            MapsTo = mapsTo;
        }

        public override string ToString()
        {
            return $"{Variable} -> {MapsTo}";
        }
    }
}
