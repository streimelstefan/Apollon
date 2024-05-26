using Apollon.Lib.Resolution.CoSLD.States;
using Apollon.Lib.Rules;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.CoSLD
{
    public class CoResolutionForAllResult : CoResolutionResult
    {
        public Literal? RealGoal { get; private set; }

        public CoResolutionForAllResult(bool success, Substitution substitution, ResolutionBaseState state, Literal? realGoal) 
            : base(success, substitution, state)
        {
            if (success && realGoal == null)
            {
                throw new ArgumentNullException(nameof(realGoal));
            }

            this.RealGoal = realGoal;
        }
    }
}
