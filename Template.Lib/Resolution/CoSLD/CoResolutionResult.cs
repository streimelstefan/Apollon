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

        private ResolutionBaseState state;

        public ResolutionBaseState State 
        {
            get
            {
                return this.state;
            }

            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(this.State));
                this.state = value;
            }
        }

        public CoResolutionResult(bool success, Substitution substitution, ResolutionBaseState state)
            : base(success, state.Chs, substitution)
        {
            State = state;
        }
    }
}
