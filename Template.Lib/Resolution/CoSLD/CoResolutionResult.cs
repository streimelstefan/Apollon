using Apollon.Lib.Resolution.CallStackAndCHS;
using Apollon.Lib.Resolution.CoSLD.States;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.CoSLD
{
    public class CoResolutionResult : ResolutionResult
    {

        public ResolutionBaseState State { get; private set; }

        public CoResolutionResult(bool success, Substitution substitution, ResolutionBaseState state)
            : base(success, state.Chs, substitution)
        {
            State = state;
        }


        public CoResolutionResult() : base()
        {
        }
    }
}
