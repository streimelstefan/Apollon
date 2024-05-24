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
    public class ResolutionStepState : ResolutionBaseState
    {
        public BodyPart CurrentGoal { get; private set; }

        public static ResolutionStepState CloneConstructor(ResolutionBaseState baseState, BodyPart currentGoal)
        {
            var obj = new ResolutionStepState(baseState, currentGoal);

            return (ResolutionStepState)obj.Clone();
        }

        public static ResolutionStepState CloneConstructor(ResolutionBaseState baseState, BodyPart currentGoal, Statement[] statements)
        {
            var obj = new ResolutionStepState(baseState, currentGoal, statements);

            return (ResolutionStepState)obj.Clone();
        }

        public ResolutionStepState(BodyPart currentGoal, Statement[] statements, Stack<Literal> callStack, CHS chs, Substitution substitution, ILogger logger)
            : base(statements, callStack, chs, substitution, logger)
        {
            CurrentGoal = currentGoal;
        }

        public ResolutionStepState(ResolutionBaseState baseState, BodyPart currentGoal)
            : this(currentGoal, baseState.Statements, baseState.CallStack, baseState.Chs, baseState.Substitution, baseState.Logger)
        {
        }

        public ResolutionStepState(ResolutionBaseState baseState, BodyPart currentGoal, Statement[] statements)
            : this(currentGoal, statements, baseState.CallStack, baseState.Chs, baseState.Substitution, baseState.Logger)
        {
        }
        
        public override object Clone()
        {
            var baseObj = (ResolutionBaseState)base.Clone();

            return new ResolutionStepState(baseObj, (BodyPart)CurrentGoal.Clone());
        }
    }
}
