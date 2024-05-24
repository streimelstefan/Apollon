﻿using Apollon.Lib.Atoms;
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

        private int variableIndex = 1;

        private VariableExtractor variableExtractor = new VariableExtractor();

        private OperationResolver operationResolver = new OperationResolver();

        public IEnumerable<ResolutionResult> Resolute(Statement[] statements, BodyPart[] goals, ILogger logger)
        {
            // Initialize the call stack and CHS (coinductive hypothesis set)
            _allStatements = statements.Where(s => s.Head != null).ToArray();
            var callStack = new Stack<Literal>();
            // -1 because the resolve all goals will increase it at the start resulting in 0.
            logger.RecursionDepth = -1;
            variableIndex = 1;

            // Start the resolution process
            var results = ResolveAllGoals(new ResolutionRecursionState(goals, _allStatements, callStack, new CHS(), new Substitution(), logger));

            foreach (var res in results)
            {
                if (!res.Success)
                {
                    yield return new ResolutionResult();
                }

                yield return new ResolutionResult(res.CHS, res.Substitution);
            }
        }

        public IEnumerable<CoResolutionResult> ResolveAllGoals(ResolutionRecursionState state)
        {
            // if there are no goals it means we were called by an atom and we can succeed
            if (state.Goals.Length == 0)
            {
                yield return new CoResolutionResult(true, new Substitution(), state);
                yield break;
            }
            state.Logger.RecursionDepth++;

            var results = ResolveAllGoalsPart((ResolutionRecursionState)state.Clone());

            foreach (var res in results)
            {
                yield return res;
            }
        }

        private IEnumerable<CoResolutionResult> ResolveAllGoalsPart(ResolutionRecursionState state)
        {

            var goal = state.Goals.First();
            var nextGoals = state.Goals.Skip(1).ToArray();
            IEnumerable<CoResolutionResult> results = new CoResolutionResult[0];

            if (goal.ForAll != null)
            {
                state.Logger.Info($"Current goal is: {goal} | {state.Substitution}");
                results = ResolveForAllGoal(ResolutionStepState.CloneConstructor(state, goal, state.Statements));
            }
            else if (goal.Literal != null)
            {
                var substituted = state.Substitution.Apply(goal.Literal);
                state.Logger.Info($"Current goal is: {substituted}");
                
                var nextState = ResolutionLiteralState.CloneConstructor(state, substituted, state.Statements);
                nextState.Substitution.Clear();

                results = ResolveLiteralGoal(nextState);
            }
            else if (goal.Operation != null)
            {
                state.Logger.Info($"Current goal is: {goal} | {state.Substitution}");
                results = ResolveOperation(goal.Operation, (ResolutionBaseState)state.Clone());
            }

            foreach (var res in results)
            {
                if (!res.Success)
                {
                    state.Logger.Debug($"Recursive resolution of goal {goal} using {state.Substitution} failed");
                    yield return new CoResolutionResult();
                    yield break;
                }

                var stateCopy = (ResolutionRecursionState)state.Clone();
                stateCopy.Chs = res.State.Chs;
                stateCopy.Substitution.BackPropagate(res.Substitution);
                stateCopy.Substitution.Contract();


                // if there are other goals that need to be checked.
                if (nextGoals.Length != 0)
                {
                    stateCopy.Logger.Silly($"GoalPart {goal} succeeded. Next gaol parts are [{string.Join(", ", nextGoals.Select(g => g.ToString()))}]");
                    stateCopy.LogState();
                    var recurisveResults = ResolveAllGoalsPart(ResolutionRecursionState.CloneConstructor(stateCopy, nextGoals));
                    foreach (var recRes in recurisveResults)
                    {
                        yield return recRes;
                    }
                }
                else
                {
                    stateCopy.Logger.Silly($"GoalPart {goal} succeeded. No more next goal parts. Rule succeeded as a whole.");
                    stateCopy.LogState();
                    yield return new CoResolutionResult(true, stateCopy.Substitution, stateCopy);
                }
            }
        }

        private IEnumerable<CoResolutionResult> ResolveForAllGoal(ResolutionStepState state)
        {
            var forallRuleHead = GetForAllRule(state.CurrentGoal);
            var forallRules = state.Statements
                .Where(s => _unifier.Unify(s.Head, forallRuleHead).IsSuccess)
                .Select(s => (Statement)s.Clone())
                .Select(s => new Statement[] { s })
                .ToList();
            var currentGoal = new BodyPart(forallRuleHead, null);

            var results = ResolveForAllGoalPart(forallRules, ResolutionStepState.CloneConstructor(state, currentGoal));

            foreach (var result in results)
            {
                state.Logger.Silly($"Forall goal {(result.Success ? "succeeded" : "failed")}.");
                result.State.LogState();
                yield return result;
            }
        }

        private IEnumerable<CoResolutionResult> ResolveForAllGoalPart(List<Statement[]> forAllRules, ResolutionStepState state)
        {
            var sub = state.Substitution.Clone();
            var forallRule = forAllRules.First();
            // list of all the goal parts without the one that is currently being handled.
            var forAllRulesClone = new List<Statement[]>(forAllRules.Skip(1)); 


            var subbedGoal = new BodyPart[] { sub.Apply(new Statement(null, state.CurrentGoal)).Body[0] };
            var results = ResolveAllGoals(ResolutionRecursionState.CloneConstructor(subbedGoal, forallRule, state.CallStack, state.Chs, new Substitution(), state.Logger));

            foreach (var result in results)
            {
                if (!result.Success)
                {
                    state.Logger.Silly($"Forall part {forallRule} failed.");
                    state.LogState();
                    yield return new CoResolutionResult();
                    yield break;
                }

                // if there are still forall rules that need to be checked.
                if (forAllRulesClone.Count() != 0)
                {
                    var stateCopy = (ResolutionStepState)state.Clone();
                    stateCopy.Chs = result.State.Chs;
                    stateCopy.Substitution.BackPropagate(result.Substitution);
                    stateCopy.Substitution.Contract();
                    var newSubbedGoal = stateCopy.Substitution.Apply(new Statement(null, state.CurrentGoal)).Body.First();

                    state.Logger.Silly($"Forall part {forallRule} succeeded. Rules to check [{string.Join(", ", forAllRulesClone.Select(l => l[0].ToString()))}]");
                    stateCopy.LogState();

                    var recursiveResults = ResolveForAllGoalPart(
                        forAllRulesClone,
                        ResolutionStepState.CloneConstructor(result.State, newSubbedGoal, forAllRulesClone.First()));

                    foreach (var res in recursiveResults)
                    {
                        yield return res;
                    }
                } 
                else // if we are in the last goal part. We can return our results after they have been merged with the current state.
                {
                    state.Logger.Silly($"Forall part {forallRule} succeeded. No more rules to check.");
                    result.State.LogState();
                    yield return new CoResolutionResult(true, result.Substitution, result.State);
                }
            }
        }

        // Recursively resolves a goal
        private IEnumerable<CoResolutionResult> ResolveLiteralGoal(ResolutionLiteralState state)
        {
            //var baseSub = PreprocessLiteralGoal(state.CurrentGoal);
            var checkRes = CheckCHSAndCallStack(state);
            if (checkRes == CheckerResult.Succeed)
            {
                yield return new CoResolutionResult(true, new Substitution(), state);
                yield break;
            }
            if (checkRes == CheckerResult.Fail)
            {
                yield return new CoResolutionResult();
                yield break;
            }

            state.Logger.Debug($"CallStack adding goal {state.CurrentGoal}");
            state.CallStack.Push(state.CurrentGoal);
            var expansionResults = ResolveLiteralGoalByExpansion(state);

            foreach (var expandsionRes in expansionResults)
            {
                state.Logger.Debug($"CallStack removing goal {state.CurrentGoal}");
                if (!expandsionRes.Success)
                {
                    yield return new CoResolutionResult();
                    yield break;
                }
                var subClone = state.Substitution.Clone();
                subClone.BackPropagate(expandsionRes.Substitution);
                subClone.Contract();

                yield return new CoResolutionResult(expandsionRes.Success, subClone, expandsionRes.State);
            }
        }

        private IEnumerable<CoResolutionResult> ResolveLiteralGoalByExpansion(ResolutionLiteralState state)
        {
            bool hasYielded = false;
            var variablesInGoal = variableExtractor.ExtractVariablesFrom(state.CurrentGoal).Select(t => t.Value).ToHashSet();
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
                var results = ResolveAllGoals(
                    ResolutionRecursionState.CloneConstructor(statement.Body, _allStatements, state.CallStack, state.Chs, unificationRes.Value, state.Logger));

                foreach (var result in results)
                {
                    // this rule did not succeed try to find another one.
                    if (!result.Success) continue;

                    var stateClone = (ResolutionLiteralState)state.Clone();                    
                    var reverseUnification = _unifier.Unify(stateClone.CurrentGoal, statement.Head);
                    
                    reverseUnification.Value.BackPropagate(result.Substitution);
                    reverseUnification.Value.Contract();
                    reverseUnification.Value.Intersect(variablesInGoal);

                    var goalToAdd = reverseUnification.Value.Apply(stateClone.CurrentGoal);
                    stateClone.Chs = result.CHS;
                    stateClone.Chs.Add(goalToAdd);
                    stateClone.Logger.Debug($"CHS added goal {goalToAdd} resulting in {stateClone.Chs}");
                    stateClone.Substitution = reverseUnification.Value;

                    state.Logger.Silly($"Literal Goal {state.CurrentGoal} succeeded with {statement}.");
                    stateClone.LogState();

                    yield return new CoResolutionResult(true, reverseUnification.Value, stateClone);
                    hasYielded = true;
                }
            }

            if (!hasYielded)
            {
                // no statments where found that succeed for this statment. 
                state.Logger.Silly($"Literal Goal {state.CurrentGoal} failed.");
                yield return new CoResolutionResult();
            }
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



        private IEnumerable<CoResolutionResult> ResolveOperation(Operation operation, ResolutionBaseState state)
        {
            yield return operationResolver.ResolveOperation(operation, state);
        }

        private void PreprocessLiteralGoal(Literal goal, Substitution sub)
        {
            foreach (var param in goal.Atom.ParamList)
            {
                if (param.Term != null && param.Term.IsVariable)
                {
                    var newVariableName = $"RV/{variableIndex}";
                    sub.Add(param.Term, new AtomParam(new Term(newVariableName)));
                    param.Term.Value = newVariableName;
                    variableIndex++;
                }
                else if (param.Literal != null)
                {
                    PreprocessLiteralGoal(param.Literal, sub);
                }
            }
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
