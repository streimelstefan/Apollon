using Apollon.Lib.Atoms;
using Apollon.Lib.Graph;
using Apollon.Lib.Linker;
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
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.CoSLD
{
    public class CoSLDResolution : IResolution
    {
        private IUnifier unifier = new ConstructiveUnifier();

        private ICoinductiveCHSChecker chsChecker = new CHSChecker();
        private ICallStackChecker callStackChecker = new CallStackChecker();

        private Statement[] allStatements = new Statement[0];

        private int variableIndex = 1;

        private VariableExtractor variableExtractor = new VariableExtractor();

        private OperationResolver operationResolver = new OperationResolver();

        private VariableLinker linker = new VariableLinker();

        private IEqualizer<Literal> preSelector = new LiteralParamCountEqualizer();

        private SubstitutionGroups substitutionGroups = new SubstitutionGroups();

        public IEnumerable<ResolutionResult> Resolute(Statement[] statements, BodyPart[] goals, ILogger logger)
        {
            // Initialize the call stack and CHS (coinductive hypothesis set)
            this.allStatements = statements.Where(s => s.Head != null).ToArray();
            var callStack = new Stack<Literal>();
            // -1 because the resolve all goals will increase it at the start resulting in 0.
            logger.RecursionDepth = -1;
            variableIndex = 1;

            // Start the resolution process
            var results = ResolveAllGoals(new ResolutionRecursionState(goals, allStatements, callStack, new CHS(), new Substitution(), logger));

            foreach (var res in results)
            {
                if (res.Success)
                {
                    yield return new ResolutionResult(res.CHS, res.Substitution);
                } else
                {
                    yield return new ResolutionResult();
                    yield break;
                }
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
            state.LogState();

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
                    yield return new CoResolutionResult(false, state.Substitution, state);
                    continue;
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
                    stateCopy.Logger.Silly($"SubTree: {this.substitutionGroups}");
                    var recurisveResults = ResolveAllGoalsPart(ResolutionRecursionState.CloneConstructor(stateCopy, nextGoals));
                    foreach (var recRes in recurisveResults)
                    {
                        if (!recRes.Success)
                        {
                            yield return new CoResolutionResult(false, state.Substitution, state);
                        }
                        else
                        {
                            yield return recRes;
                        }
                    }
                }
                else
                {
                    stateCopy.Logger.Silly($"GoalPart {goal} succeeded. No more next goal parts. Rule succeeded as a whole.");
                    stateCopy.LogState();
                    stateCopy.Logger.Silly($"SubTree: {this.substitutionGroups}");
                    yield return new CoResolutionResult(true, stateCopy.Substitution, stateCopy);
                }
            }
        }

        private IEnumerable<CoResolutionResult> ResolveForAllGoal(ResolutionStepState state)
        {
            state.LogState();
            // extract variable of each forall part.

            // for each part try to solve the goal with the variable
            // if the variable is bound (Has substitution with a non variable assigned) ignore result. And 
            // take the next if there is one. If there is none fail.
            // if the variable is negativly constraint (has values in the pvl). Go trough each value of the pvl
            // and try it with a substitution of that. If that succeed add the goal with the substition without the pvl.
            // if the variable is unbound add the goal to the chs.
            var subbedGoal = state.Substitution.Apply(new Statement(null, state.CurrentGoal)).Body[0];
            var results = this.ResolveForAllGoalPart(state, subbedGoal);

            foreach (var res in results)
            {
                if (res.Success && res.RealGoal != null)
                {
                    var stateCopy = (ResolutionStepState)state.Clone();
                    stateCopy.Chs = res.CHS;
                    res.Substitution.RemovePVls();
                    stateCopy.Substitution.BackPropagate(res.Substitution);
                    stateCopy.Substitution.Contract();

                    yield return new CoResolutionResult(true, stateCopy.Substitution, stateCopy);
                }
                else
                {
                    yield return res;
                }
            }
        }

        private IEnumerable<CoResolutionForAllResult> ResolveForAllGoalPart(ResolutionStepState state, BodyPart goal)
        {
            ArgumentNullException.ThrowIfNull(goal.ForAll, nameof(goal.ForAll));

            var variable = (Term)goal.ForAll.Clone();
            variable.ProhibitedValues.Clear();
            state.KeepUnbound.Add(variable);

            ResolutionStepState stateCopy = (ResolutionStepState)state.Clone();
            var realGoal = goal.Literal;

            // if child of the forall goal is a literal 
            // we need to see if the literal resolves. And apply the forall rules accordingly.

            state.Logger.Silly($"FORALL: Trying to resolve forall variable {variable}");
            IEnumerable<CoResolutionForAllResult> results = new CoResolutionForAllResult[0];
            if (goal.Child == null && goal.Literal != null)
            {
                state.Logger.Info($"Current Goal is {goal.Literal}");
                var intimResults = this.ResolveLiteralGoal(ResolutionLiteralState.CloneConstructor(state, goal.Literal));
                results = intimResults.Select(r => new CoResolutionForAllResult(r.Success, r.Substitution, r.State, realGoal));
            }
            else if (goal.Child != null && goal.Child.ForAll != null)
            {
                stateCopy = (ResolutionStepState)state.Clone();
                results = this.ResolveForAllGoalPart(stateCopy, goal.Child);
            }

            foreach (var res in results)
            {
                // if the resolution was not successful we can abort.
                if (!res.Success)
                {
                    state.Logger.Silly($"Forall Variable Part {variable} of {state.CurrentGoal} failed.");
                    state.LogState();
                    state.Logger.Silly($"SubTree: {this.substitutionGroups}");
                    continue;
                }

                state.Chs = res.CHS;
                var variableMapping = res.Substitution.Mappings.Where(m => m.Variable.Value == variable.Value).First();

                stateCopy = (ResolutionStepState)state.Clone();
                stateCopy.Substitution.BackPropagate(res.Substitution);
                stateCopy.Chs = res.CHS;
                stateCopy.KeepUnbound.Remove(variable);

                // if variable is unbound return success and is not negativly constraint.
                if (!variableMapping.MapsTo.Term.IsNegativelyConstrained())
                {
                    stateCopy.Logger.Trace($"Forall Variable Part {variable} of {state.CurrentGoal} succeeded because variable is not bound or negativly constraint.");
                    stateCopy.LogState();
                    stateCopy.Logger.Silly($"SubTree: {this.substitutionGroups}");
                    yield return new CoResolutionForAllResult(true, stateCopy.Substitution, stateCopy, res.RealGoal);
                    yield break;
                }

                // no variable is negativly constraint, so add all new values of the gotten pvl to the values to trie
                state.Logger.Trace($"Forall Variable Part {variable} of {state.CurrentGoal} failed because variable is negativly constraint.");
                state.LogState();
                state.Logger.Silly($"SubTree: {this.substitutionGroups}");
                var valuesToTry = variableMapping.Variable.ProhibitedValues.GetValues().ToList();
                valuesToTry.Sort((x, y) => x.ToString().CompareTo(y.ToString()));
                var constRes = this.ResolveForAllConstraint(stateCopy, goal, valuesToTry);
                foreach (var constraintResult in constRes)
                {
                    yield return constraintResult;
                }
            }

            yield return new CoResolutionForAllResult(false, stateCopy.Substitution, stateCopy, realGoal);
        }

        private IEnumerable<CoResolutionForAllResult> ResolveForAllConstraint(ResolutionStepState state, BodyPart goal, List<AtomParam> valuesToTry)
        {
            ArgumentNullException.ThrowIfNull(goal.ForAll, nameof(goal.ForAll));

            var variable = (Term)goal.ForAll.Clone();
            variable.ProhibitedValues.Clear();

            ResolutionStepState stateCopy = (ResolutionStepState)state.Clone();
            var realGoal = goal.Literal;

            // if child of the forall goal is a literal 
            // we need to see if the literal resolves. And apply the forall rules accordingly.
            for (int i = 0; i < valuesToTry.Count(); i++)
            {
                var subToTry = new Substitution();
                subToTry.Add(variable, valuesToTry[i]);

                state.Logger.Silly(
                    $"FORALL: Variable to try for {variable}: {valuesToTry[i]} - [{string.Join(", ", valuesToTry.Skip(i).Select(a => a.ToString()))}] ");
                IEnumerable<CoResolutionForAllResult> results = new CoResolutionForAllResult[0];
                if (goal.Child == null && goal.Literal != null)
                {
                    var subbedRealGoal = subToTry.Apply(goal.Literal);
                    state.Logger.Info($"Current Goal is {subbedRealGoal}");
                    var intimResults = this.ResolveLiteralGoal(ResolutionLiteralState.CloneConstructor(state, subbedRealGoal));
                    results = intimResults.Select(r => new CoResolutionForAllResult(r.Success, r.Substitution, r.State, realGoal));
                }
                else if (goal.Child != null && goal.Child.ForAll != null)
                {
                    stateCopy = (ResolutionStepState)state.Clone();
                    stateCopy.Substitution.BackPropagate(subToTry);
                    results = this.ResolveForAllGoalPart(stateCopy, goal.Child);
                }

                foreach (var res in results)
                {
                    // if the resolution was not successful we can abort.
                    if (!res.Success)
                    {
                        state.Logger.Silly($"Forall Variable Part {variable} of {state.CurrentGoal} failed.");
                        state.LogState();
                        state.Logger.Silly($"SubTree: {this.substitutionGroups}");
                        continue;
                    }

                    state.Chs = res.CHS;
                    subToTry.BackPropagate(res.Substitution);
                    subToTry.Contract();
                    var variableMapping = subToTry.Mappings.Where(m => m.Variable.Value == variable.Value).First();

                    stateCopy = (ResolutionStepState)state.Clone();
                    stateCopy.Substitution.BackPropagate(subToTry);
                    stateCopy.Substitution.BackPropagate(res.Substitution);
                    stateCopy.Chs = res.CHS;
                    stateCopy.KeepUnbound.Remove(variable);

                    // if variable is unbound return success and is not negativly constraint.
                    if (variableMapping.MapsTo.Term.IsNegativelyConstrained())
                    {
                        stateCopy.Logger.Error("Uncertain how to handle case where constrainted varaible returns constraint again. Failing for now.");
                        stateCopy.LogState();
                        stateCopy.Logger.Silly($"SubTree: {this.substitutionGroups}");
                        yield return new CoResolutionForAllResult(false, stateCopy.Substitution, stateCopy, realGoal);
                        yield break;
                    }
                }
            }

            yield return new CoResolutionForAllResult(true, stateCopy.Substitution, stateCopy, realGoal);
        }

        // Recursively resolves a goal
        private IEnumerable<CoResolutionResult> ResolveLiteralGoal(ResolutionLiteralState state)
        {
            state.LogState();
            //var baseSub = PreprocessLiteralGoal(state.CurrentGoal);
            var checkRes = CheckCHSAndCallStack(state);
            if (checkRes == CheckerResult.Succeed)
            {
                yield return new CoResolutionResult(true, new Substitution(), state);
                yield break;
            }
            if (checkRes == CheckerResult.Fail)
            {
                yield return new CoResolutionResult(false, state.Substitution, state);
                yield break;
            }

            state.Logger.Debug($"CallStack adding goal {state.CurrentGoal}");
            state.CallStack.Push(state.CurrentGoal);
            var expansionResults = this.ResolveLiteralGoalByExpansion(state);

            foreach (var expandsionRes in expansionResults)
            {
                state.Logger.Debug($"CallStack removing goal {state.CurrentGoal}");
                if (!expandsionRes.Success)
                {
                    yield return expandsionRes;
                    yield break;
                }
                var stateClone = (ResolutionLiteralState)expandsionRes.State.Clone();
                var subClone = state.Substitution.Clone();
                subClone.BackPropagate(expandsionRes.Substitution);
                subClone.Contract();

                yield return new CoResolutionResult(expandsionRes.Success, subClone, stateClone);
            }
        }

        private IEnumerable<CoResolutionResult> ResolveLiteralGoalByExpansion(ResolutionLiteralState state)
        {
            bool hasYielded = false;
            var variablesInGoal = variableExtractor.ExtractVariablesFrom(state.CurrentGoal).Select(t => t.Value).ToHashSet();

            var preselectedStatements = state.Statements
                .Where(s => this.preSelector.AreEqual(state.CurrentGoal, s.Head))
                .Select(s => this.LinkAndRenameVariablesIn(s))
                .ToArray();

            state.Logger
                .Silly($"Preselected {preselectedStatements.Length} rules: [{string.Join(" | ", preselectedStatements.Select(s => s.ToString()))}]");

            foreach (var statement in preselectedStatements)
            {
                if (statement.Head == null)
                {
                    continue;
                }

                var unificationRes = this.unifier.Unify(statement.Head, state.CurrentGoal);
                if (unificationRes.Value == null)
                {
                    continue;
                }

                // if a variable that should be kept unbound gets bound asume a unifictaion fail.
                foreach (var bound in unificationRes.Value.BoundMappings)
                {
                    if (state.KeepUnbound.Where(v => v.Equals(bound.Variable)).Any())
                    {
                        // this literal would result in a binding of a variable that should be kept unbound.
                        // so we need to fail.
                        continue;
                    }
                }

                this.substitutionGroups.AddAllOf(unificationRes.Value);

                state.Logger.Info($"Unified goal {state.CurrentGoal} with {statement} resulting in {unificationRes.Value}");
                // we expand the goal with this statment if it succeeds the goal gets added to the chs.
                var results = this.ResolveAllGoals(
                    ResolutionRecursionState.CloneConstructor(statement.Body, this.allStatements, state.CallStack, state.Chs, unificationRes.Value, state.KeepUnbound, state.Logger));

                foreach (var result in results)
                {
                    // this rule did not succeed try to find another one.
                    if (!result.Success)
                    {
                        state.Chs.SafeUnion(result.CHS, this.substitutionGroups);
                        continue;
                    }

                    var stateClone = (ResolutionLiteralState)state.Clone();
                    var reverseUnification = this.unifier.Unify(stateClone.CurrentGoal, statement.Head);

                    reverseUnification.Value.BackPropagate(result.Substitution);
                    reverseUnification.Value.Contract();
                    reverseUnification.Value.Intersect(variablesInGoal);

                    var goalToAdd = reverseUnification.Value.Apply(stateClone.CurrentGoal);
                    stateClone.Chs = result.CHS;
                    stateClone.Chs.Add(goalToAdd, this.substitutionGroups);
                    stateClone.Substitution = reverseUnification.Value;
                    stateClone.Logger.Debug($"CHS added goal {goalToAdd} resulting in {stateClone.Chs}");

                    state.Logger.Silly($"Literal Goal {state.CurrentGoal} succeeded with {statement}.");
                    stateClone.LogState();
                    stateClone.Logger.Silly($"SubTree: {this.substitutionGroups}");

                    yield return new CoResolutionResult(true, reverseUnification.Value, stateClone);
                    hasYielded = true;
                }
            }

            if (!hasYielded)
            {
                // no statments where found that succeed for this statment. 
                state.Logger.Silly($"Literal Goal {state.CurrentGoal} failed.");
                yield return new CoResolutionResult(false, state.Substitution, state);
            }
        }

        public CheckerResult CheckCHSAndCallStack(ResolutionLiteralState state)
        {
            var chsCheck = chsChecker.CheckCHSFor(state.CurrentGoal, state.Chs);
            state.Logger.Trace($"CHS marked goal {state.CurrentGoal} as {chsCheck} using {state.Chs}");

            if (chsCheck != CheckerResult.Continue) return chsCheck;

            var callStackCheck = callStackChecker.CheckCallStackFor(state.CurrentGoal, state.CallStack);

            state.Logger.Trace($"CallStack marked goal {state.CurrentGoal} as {callStackCheck} using ({string.Join(", ", state.CallStack)})");
            return callStackCheck;
        }



        private IEnumerable<CoResolutionResult> ResolveOperation(Operation operation, ResolutionBaseState state)
        {
            var res = this.operationResolver.ResolveOperation(operation, state);

            // this.substitutionTree.AddAllOf(res.Substitution);
            foreach (var bound in res.Substitution.BoundMappings)
            {
                if (state.KeepUnbound.Where(v => v.Equals(bound.Variable)).Any())
                {
                    // this literal would result in a binding of a variable that should be kept unbound.
                    // so we need to fail.
                    state.Logger.Info($"Goal {operation} failed since it binds a not boundable variable.");
                    yield return new CoResolutionResult(false, new Substitution(), state);
                    yield break;
                }
            }

            yield return res;
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

        private Statement LinkAndRenameVariablesIn(Statement statement)
        {
            var linkedStatement = this.linker.LinkVariables(statement);
            var variables = this.variableExtractor.ExtractVariablesFrom(linkedStatement);
            foreach (var variable in variables)
            {
                var newName = $"RV/{this.variableIndex}";

                variable.Value = newName;
                this.variableIndex++;
            }

            return linkedStatement;
        }
    }
}
