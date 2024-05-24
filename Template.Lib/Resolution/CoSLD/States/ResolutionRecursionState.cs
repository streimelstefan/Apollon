using Apollon.Lib.Logging;
using Apollon.Lib.Resolution.CallStackAndCHS;
using Apollon.Lib.Rules;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.CoSLD.States
{
    public class ResolutionRecursionState : ResolutionBaseState
    {
        public BodyPart[] Goals { get; private set; }

        public static ResolutionRecursionState CloneConstructor(BodyPart[] goals, Statement[] statements, Stack<Literal> callStack, CHS chs, Substitution substitution, ILogger logger)
        {
            var obj = new ResolutionRecursionState(goals, statements, callStack, chs, substitution, logger);

            return (ResolutionRecursionState)obj.Clone();
        }

        public static ResolutionRecursionState CloneConstructor(ResolutionBaseState baseState, BodyPart[] goals)
        {
            var obj = new ResolutionRecursionState(baseState, goals);

            return (ResolutionRecursionState)obj.Clone();
        }

        public ResolutionRecursionState(BodyPart[] goals, Statement[] statements, Stack<Literal> callStack, CHS chs, Substitution substitution, ILogger logger)
            : base(statements, callStack, chs, substitution, logger)
        {
            Goals = goals;
        }

        public ResolutionRecursionState(ResolutionBaseState baseState, BodyPart[] goals)
            : this(goals, baseState.Statements, baseState.CallStack, baseState.Chs, baseState.Substitution, baseState.Logger)
        {
        }

        public override object Clone()
        {
            var baseObj = (ResolutionBaseState)base.Clone();

            return new ResolutionRecursionState(baseObj, Goals.Select(g => (BodyPart)g.Clone()).ToArray());
        }
    }
}
