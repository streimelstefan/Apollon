using Apollon.Lib.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Unification.DisagreementFinders
{
    public class Disagreement
    {

        public AtomParam? First { get; private set; }
        public AtomParam? Second { get; private set; }

        public bool IsEmpty
        {
            get
            {
                return First == null && Second == null;
            }
        }

        public Disagreement(AtomParam first, AtomParam second)
        {
            First = first;
            Second = second;
        }

        public Disagreement() { }

    }
}
