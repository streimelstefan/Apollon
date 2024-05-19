using Apollon.Lib.Resolution.CallStackAndCHS;
using Apollon.Lib.Unification;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution
{
    public class ResolutionResult
    {

        public bool Success { get; private set; }

        public CHS CHS { get; private set; }
        public ISubstitution Substitution { get; private set; }

        public ResolutionResult(CHS chs, ISubstitution substitution) : this(!chs.IsEmpty, chs, substitution)
        {
        }

        public ResolutionResult(bool success, CHS chs, ISubstitution substitution)
        {
            Success = success;
            CHS = chs;
            Substitution = substitution;
        }


        public ResolutionResult() : this(new CHS(), new Substitution())
        {
        }
    }
}
