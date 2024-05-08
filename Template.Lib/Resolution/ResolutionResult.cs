using Apollon.Lib.Unification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution
{
    public class ResolutionResult
    {

        public CHS CHS { get; private set; }
        public ISubstitution Substitution { get; private set; }

        public ResolutionResult(CHS chs, ISubstitution substitution) 
        {
            CHS = chs;
            Substitution = substitution;
        }

        public ResolutionResult() : this(new CHS(), new Substitution())
        {
        }
    }
}
