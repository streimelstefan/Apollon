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
    public class ResolutionLiteralState : ResolutionBaseState
    {
        public Literal CurrentGoal { get; private set; }

        public static ResolutionLiteralState CloneConstructor(ResolutionBaseState baseState, Literal currentGoal)
        {
            var obj = new ResolutionLiteralState(baseState, currentGoal);
            return (ResolutionLiteralState)obj.Clone();
        }

        public static ResolutionLiteralState CloneConstructor(ResolutionBaseState baseState, Literal currentGoal, Statement[] statements)
        {
            var obj = new ResolutionLiteralState(baseState, currentGoal, statements);
            return (ResolutionLiteralState)obj.Clone();
        }

        public ResolutionLiteralState(Literal currentGoal, Statement[] statements, Stack<Literal> callStack, CHS chs, ISubstitution substitution, ILogger logger) 
            : base(statements, callStack, chs, substitution, logger)
        {
            CurrentGoal = currentGoal;
        }

        public ResolutionLiteralState(ResolutionBaseState baseState, Literal currentGoal)
            : this(currentGoal, baseState.Statements, baseState.CallStack, baseState.Chs, baseState.Substitution, baseState.Logger)
        {
        }

        public ResolutionLiteralState(ResolutionBaseState baseState, Literal currentGoal, Statement[] statements)
            : this(currentGoal, statements, baseState.CallStack, baseState.Chs, baseState.Substitution, baseState.Logger)
        {
        }

        public override object Clone()
        {
            var baseObj = (ResolutionBaseState)base.Clone();

            return new ResolutionLiteralState(baseObj, (Literal)CurrentGoal.Clone());
        }
    }
}
