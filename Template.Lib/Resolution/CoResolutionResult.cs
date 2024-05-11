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
        public CHS Result { get; set; }

        public CoResolutionResult(CHS chs, ISubstitution substitution, CHS result) : base(chs, substitution)
        {
            Result = result;
        }

        public CoResolutionResult() : this(new CHS(), new Substitution(), new CHS())
        {
        }
    }
}
