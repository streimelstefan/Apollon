using Apollon.Lib.Unification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution
{
    public class CoResolutionResult : ResolutionResult
    {
        public CoResolutionResult(CHS chs, ISubstitution substitution) : base(chs, substitution)
        {
        }

        public CoResolutionResult() : this(new CHS(), new Substitution())
        {
        }
    }
}
