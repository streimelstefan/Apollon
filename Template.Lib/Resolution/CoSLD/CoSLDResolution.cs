using Apollon.Lib.Atoms;
using Apollon.Lib.Logging;
using Apollon.Lib.Resolution.CallStackAndCHS;
using Apollon.Lib.Resolution.Checkers;
using Apollon.Lib.Resolution.Checkers.CallStack;
using Apollon.Lib.Resolution.Checkers.CHSCheckers;
using Apollon.Lib.Resolution.CoSLD.States;
using Apollon.Lib.Rules;
using Apollon.Lib.Rules.Operations;
using Apollon.Lib.Unification;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.CoSLD
{
    public class CoSLDResolution : IResolution
    {
        private IUnifier _unifier = new ConstructiveUnifier();

        private ICoinductiveCHSChecker _chsChecker = new CHSChecker();
        private ICallStackChecker _callStackChecker = new CallStackChecker();

        private Statement[] _allStatements = new Statement[0];

        public ResolutionResult Resolute(Statement[] statements, BodyPart[] goals, ILogger logger)
        {
            // Initialize the call stack and CHS (coinductive hypothesis set)
            _allStatements = statements;
            var callStack = new Stack<Literal>();
            CHS chs = new CHS();
            // -1 because the resolve all goals will increase it at the start resulting in 0.
            logger.RecursionDepth = -1;

            // Start the resolution process
            var res = ResolveAllGoals(new ResolutionRecursionState(goals, statements, callStack, chs, new Substitution(), logger));

            if (!res.Success)
            {
                return new ResolutionResult();
            }

            return new ResolutionResult(chs, res.Substitution);
        }

        public CoResolutionResult ResolveAllGoals(ResolutionRecursionState state)
        {
            state.Logger.RecursionDepth++;
            ISubstitution sub = state.Substitution.Clone();
            var statements = state.Statements.Select(s => (Statement)s.Clone()).ToArray();
            foreach (var goal in state.Goals)
            {
                CoResolutionResult res = new CoResolutionResult();

                if (goal.ForAll != null)
                {
                    state.Logger.Info($"Current goal is: {goal}");
                    res = ResolveForAllGoal(new ResolutionStepState(state, goal, statements));
                } else if (goal.Literal != null)
                {
                    var substituted = sub.Apply(goal.Literal);
                    state.Logger.Info($"Current goal is: {substituted}");
                    res = ResolveLiteralGoal(new ResolutionLiteralState(state, substituted, statements));
                } else if (goal.Operation != null)
                {
                    state.Logger.Info($"Current goal is: {goal}");
                    res = ResolveOperation(goal.Operation, state.Substitution);
                }

                if (!res.Success)
                {
                    state.Logger.Debug($"Recursive resolution of goal {goal} using {sub} failed");
                    state.Logger.RecursionDepth--;
                    return new CoResolutionResult();
                }

                sub.BackPropagate(res.Substitution);
                sub.Contract();
            }

            state.Logger.RecursionDepth--;
            return new CoResolutionResult(true, sub);
        }

        private CoResolutionResult ResolveForAllGoal(ResolutionStepState state)
        {
            var forallRuleHead = GetForAllRule(state.CurrentGoal);
            var forallRules = state.Statements
                .Where(s => _unifier.Unify(s.Head, forallRuleHead).IsSuccess)
                .Select(s => (Statement)s.Clone())
                .Select(s => new Statement[] { s });

            var currentGoal = new BodyPart(forallRuleHead, null);
            var sub = state.Substitution.Clone(); 

            foreach (var forallRule in forallRules)
            {
                var subbedGoal = new BodyPart[] { sub.Apply(new Statement(null, currentGoal)).Body[0] };
                var res = ResolveAllGoals(new ResolutionRecursionState(subbedGoal, forallRule, state.CallStack, state.Chs, new Substitution(), state.Logger));

                if (!res.Success)
                {
                    return new CoResolutionResult();
                }
            }

            return new CoResolutionResult(true, new Substitution());
        }

        // Recursively resolves a goal
        private CoResolutionResult ResolveLiteralGoal(ResolutionLiteralState state)
        {
            var baseSub = PreprocessLiteralGoal(state.CurrentGoal);
            var checkRes = CheckCHSAndCallStack(state);
            if (checkRes == CheckerResult.Succeed)
            {
                return new CoResolutionResult(true, new Substitution());
            }
            if (checkRes == CheckerResult.Fail)
            {
                return new CoResolutionResult();
            }

            state.Logger.Debug($"CallStack adding goal {state.CurrentGoal}");
            state.CallStack.Push(state.CurrentGoal);
            var expansionRes = ResolveLiteralGoalByExpansion(state);
            state.CallStack.Pop();
            state.Logger.Debug($"CallStack removing goal {state.CurrentGoal}");

            baseSub.BackPropagate(expansionRes.Substitution);
            baseSub.Contract();

            return new CoResolutionResult(expansionRes.Success, baseSub);
        }

        private CoResolutionResult ResolveLiteralGoalByExpansion(ResolutionLiteralState state)
        {
            foreach (var statement in state.Statements)
            {
                if (statement.Head == null)
                {
                    throw new InvalidOperationException("TODO: Implement Constraints");
                }

                var unificationRes = _unifier.Unify(statement.Head, state.CurrentGoal);
                if (unificationRes.Value == null) continue;

                state.Logger.Info($"Unified goal {state.CurrentGoal} with {statement} resulting in {unificationRes.Value}");
                // we expand the goal with this statment if it succeeds the goal gets added to the chs.
                var result = ResolveAllGoals(
                    new ResolutionRecursionState(statement.Body, _allStatements, state.CallStack, state.Chs, unificationRes.Value, state.Logger));

                // this rule did not succeed try to find another one.
                if (!result.Success) continue;
                var reverseUnification = _unifier.Unify(state.CurrentGoal, statement.Head);
                reverseUnification.Value.BackPropagate(result.Substitution);
                reverseUnification.Value.Contract();

                var goalToAdd = reverseUnification.Value.Apply(state.CurrentGoal);
                state.Chs.Add(goalToAdd);
                state.Logger.Debug($"CHS added goal {goalToAdd} resulting in {state.Chs}");
                return new CoResolutionResult(true, reverseUnification.Value);
            }

            // no statments where found that succeed for this statment. 
            return new CoResolutionResult();
        }

        public CheckerResult CheckCHSAndCallStack(ResolutionLiteralState state)
        {
            var chsCheck = _chsChecker.CheckCHSFor(state.CurrentGoal, state.Chs);
            state.Logger.Trace($"CHS marked goal {state.CurrentGoal} as {chsCheck} using {state.Chs}");

            if (chsCheck != CheckerResult.Continue) return chsCheck;

            var callStackCheck = _callStackChecker.CheckCallStackFor(state.CurrentGoal, state.CallStack);

            state.Logger.Trace($"CallStack marked goal {state.CurrentGoal} as {callStackCheck} using ({string.Join(", ", state.CallStack)})");
            return callStackCheck;
        }



        private CoResolutionResult ResolveOperation(Operation operation, ISubstitution substitution)
        {
            if (operation.Operator == Operator.Equals)
            {
                var op = substitution.Apply(operation);
                var unificationRes = _unifier.Unify(op.Condition, op.Variable.Literal);
                return new CoResolutionResult(unificationRes.IsSuccess, unificationRes.Value ?? new Substitution());
            }
            else
            {
                if (operation.Variable.Term == null || !operation.Variable.Term.IsVariable)
                {
                    throw new InvalidOperationException("operation variable is not variable...");
                }
                operation.Variable.Term.ProhibitedValues.AddValue(new AtomParam(operation.Condition));
                var op = substitution.Apply(operation);
                return new CoResolutionResult(!op.Condition.Equals(op.Variable.Literal), substitution);
            }
        }

        /// <summary>
        /// Renames all the variables in the goal to RV/{index} so they dont get substitued with body variables.
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="paramIndex"></param>
        /// <returns></returns>
        private ISubstitution PreprocessLiteralGoal(Literal goal)
        {
            var sub = new Substitution();
            var paramIndex = 0;
            foreach (var param in goal.Atom.ParamList)
            {
                if (param.Term != null && param.Term.IsVariable)
                {
                    var newVariableName = $"RV/{paramIndex}";
                    sub.Add(param.Term, new AtomParam(new Term(newVariableName)));
                    param.Term.Value = newVariableName;
                    paramIndex++;
                }
                else if (param.Literal != null)
                {
                    paramIndex = PreprocessLiteralGoal(param.Literal, sub, paramIndex);
                }
            }

            return sub;
        }

        private int PreprocessLiteralGoal(Literal goal, ISubstitution sub, int paramIndex = 0)
        {
            foreach (var param in goal.Atom.ParamList)
            {
                if (param.Term != null && param.Term.IsVariable)
                {
                    var newVariableName = $"RV/{paramIndex}";
                    sub.Add(param.Term, new AtomParam(new Term(newVariableName)));
                    param.Term.Value = newVariableName;
                    paramIndex++;
                }
                else if (param.Literal != null)
                {
                    paramIndex = PreprocessLiteralGoal(param.Literal, sub, paramIndex);
                }
            }

            return paramIndex;
        }

        private Literal GetForAllRule(BodyPart forall)
        {
            if (forall.ForAll == null && (forall.Child == null || forall.Literal == null))
                throw new ArgumentException($"Body part needs to be a forall body part. Got ${forall}");

            if (forall.Child != null) 
                return GetForAllRule(forall.Child);
            if (forall.Literal != null)
                return forall.Literal;

            throw new InvalidOperationException("Got body part that has neither a child or a literal, weird...");
        }
    }
}
