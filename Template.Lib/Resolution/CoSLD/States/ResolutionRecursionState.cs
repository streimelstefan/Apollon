using Apollon.Lib.Logging;
using Apollon.Lib.Resolution.CallStackAndCHS;
using Apollon.Lib.Rules;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.CoSLD.States
{
    public class ResolutionRecursionState : ResolutionBaseState
    {
        public BodyPart[] Goals { get; private set; }

        public ResolutionRecursionState(BodyPart[] goals, Statement[] statements, Stack<Literal> callStack, CHS chs, ISubstitution substitution, ILogger logger)
            : base(statements, callStack, chs, substitution, logger)
        {
            Goals = goals;
        }

        public ResolutionRecursionState(ResolutionBaseState baseState, BodyPart[] goals)
            : this(goals, baseState.Statements, baseState.CallStack, baseState.Chs, baseState.Substitution, baseState.Logger)
        {
        }
    }
}
