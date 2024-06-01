//-----------------------------------------------------------------------
// <copyright file="CoSLDResolution.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Resolution.CoSLD
{
    using System.Data;
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

    /// <summary>
    /// This class is responsible for the CoSLD resolution.
    /// </summary>
    public class CoSLDResolution : IResolution
    {
        private readonly IUnifier unifier = new ConstructiveUnifier();

        private readonly ICoinductiveCHSChecker chsChecker = new CHSChecker();
        private readonly ICallStackChecker callStackChecker = new CallStackChecker();

        private readonly VariableExtractor variableExtractor = new();

        private readonly OperationResolver operationResolver = new();

        private readonly VariableLinker linker = new();

        private readonly IEqualizer<Literal> preSelector = new LiteralParamCountEqualizer();

        private readonly SubstitutionGroups substitutionGroups = new();

        private Statement[] allStatements = new Statement[0];

        private int variableIndex = 1;

        /// <summary>
        /// Executes the resolution.
        /// </summary>
        /// <param name="statements">An array of statements that should be resolved.</param>
        /// <param name="goals">An array of all goals.</param>
        /// <param name="logger">The logger used for logging.</param>
        /// <returns>The resolved goals piece by piece.</returns>
        public IEnumerable<ResolutionResult> Resolute(Statement[] statements, BodyPart[] goals, ILogger logger)
        {
            // Initialize the call stack and CHS (coinductive hypothesis set)
            this.allStatements = statements.Where(s => s.Head != null).ToArray();
            Stack<Literal> callStack = new();

            // -1 because the resolve all goals will increase it at the start resulting in 0.
            logger.RecursionDepth = -1;
            this.variableIndex = 1;

            // Start the resolution process
            IEnumerable<CoResolutionResult> results = this.ResolveAllGoals(new ResolutionRecursionState(goals, this.allStatements, callStack, new CHS(), new Substitution(), logger));

            foreach (CoResolutionResult res in results)
            {
                if (res.Success)
                {
                    yield return new ResolutionResult(res.CHS, res.Substitution);
                }
                else
                {
                    yield return new ResolutionResult();
                    yield break;
                }
            }
        }

        /// <summary>
        /// Executes the resolution.
        /// </summary>
        /// <param name="state">The state that should be used for the Resolution.</param>
        /// <returns>The resolved goals piece by piece.</returns>
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

            IEnumerable<CoResolutionResult> results = this.ResolveAllGoalsPart((ResolutionRecursionState)state.Clone());

            foreach (CoResolutionResult res in results)
            {
                yield return res;
            }
        }

        /// <summary>
        /// Checks the CHS and CallStack for the current goal.
        /// </summary>
        /// <param name="state">The state that should be checked.</param>
        /// <returns>The Result of the Check.</returns>
        public CheckerResult CheckCHSAndCallStack(ResolutionLiteralState state)
        {
            CheckerResult chsCheck = this.chsChecker.CheckCHSFor(state.CurrentGoal, state.Chs);
            state.Logger.Trace($"CHS marked goal {state.CurrentGoal} as {chsCheck} using {state.Chs}");

            if (chsCheck != CheckerResult.Continue)
            {
                return chsCheck;
            }

            CheckerResult callStackCheck = this.callStackChecker.CheckCallStackFor(state.CurrentGoal, state.CallStack);

            state.Logger.Trace($"CallStack marked goal {state.CurrentGoal} as {callStackCheck} using ({string.Join(", ", state.CallStack)})");
            return callStackCheck;
        }

        /// <summary>
        /// Resolves all goals part by part.
        /// </summary>
        /// <param name="state">The state that should be used for the Resolution.</param>
        /// <returns>The resolved goals piece by piece.</returns>
        private IEnumerable<CoResolutionResult> ResolveAllGoalsPart(ResolutionRecursionState state)
        {
            BodyPart goal = state.Goals.First();
            BodyPart[] nextGoals = state.Goals.Skip(1).ToArray();
            IEnumerable<CoResolutionResult> results = new CoResolutionResult[0];

            if (goal.ForAll != null)
            {
                state.Logger.Info($"Current goal is: {goal} | {state.Substitution}");
                results = this.ResolveForAllGoal(ResolutionStepState.CloneConstructor(state, goal, state.Statements));
            }
            else if (goal.Literal != null)
            {
                Literal substituted = state.Substitution.Apply(goal.Literal);
                state.Logger.Info($"Current goal is: {substituted}");

                ResolutionLiteralState nextState = ResolutionLiteralState.CloneConstructor(state, substituted, state.Statements);
                nextState.Substitution.Clear();

                results = this.ResolveLiteralGoal(nextState);
            }
            else if (goal.Operation != null)
            {
                state.Logger.Info($"Current goal is: {goal} | {state.Substitution}");
                results = this.ResolveOperation(goal.Operation, (ResolutionBaseState)state.Clone());
            }

            foreach (CoResolutionResult res in results)
            {
                if (!res.Success)
                {
                    state.Logger.Debug($"Recursive resolution of goal {goal} using {state.Substitution} failed");
                    yield return new CoResolutionResult(false, state.Substitution, state);
                    continue;
                }

                ResolutionRecursionState stateCopy = (ResolutionRecursionState)state.Clone();
                stateCopy.Chs = res.State.Chs;
                stateCopy.Substitution.BackPropagate(res.Substitution);
                stateCopy.Substitution.Contract();

                // if there are other goals that need to be checked.
                if (nextGoals.Length != 0)
                {
                    stateCopy.Logger.Silly($"GoalPart {goal} succeeded. Next goal parts are [{string.Join(", ", nextGoals.Select(g => g.ToString()))}]");
                    stateCopy.LogState();
                    stateCopy.Logger.Silly($"SubTree: {this.substitutionGroups}");
                    IEnumerable<CoResolutionResult> recurisveResults = this.ResolveAllGoalsPart(ResolutionRecursionState.CloneConstructor(stateCopy, nextGoals));
                    foreach (CoResolutionResult recRes in recurisveResults)
                    {
                        yield return !recRes.Success ? new CoResolutionResult(false, state.Substitution, state) : recRes;
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

        /// <summary>
        /// Resolves the forall part of a goal.
        /// </summary>
        /// <param name="state">The state that should be used for the Resolve process.</param>
        /// <returns>An Enumerable of all goals piece by piece.</returns>
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
            BodyPart subbedGoal = state.Substitution.Apply(new Statement(null, state.CurrentGoal)).Body[0];
            IEnumerable<CoResolutionForAllResult> results = this.ResolveForAllGoalPart(state, subbedGoal);

            foreach (CoResolutionForAllResult res in results)
            {
                if (res.Success && res.RealGoal != null)
                {
                    ResolutionStepState stateCopy = (ResolutionStepState)state.Clone();
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

            Term variable = (Term)goal.ForAll.Clone();
            variable.ProhibitedValues.Clear();
            state.KeepUnbound.Add(variable);

            ResolutionStepState stateCopy = (ResolutionStepState)state.Clone();
            Literal? realGoal = goal.Literal;

            // if child of the forall goal is a literal we need to see if the literal resolves. And apply the forall rules accordingly.
            state.Logger.Silly($"FORALL: Trying to resolve forall variable {variable}");
            IEnumerable<CoResolutionForAllResult> results = new CoResolutionForAllResult[0];
            if (goal.Child == null && goal.Literal != null)
            {
                state.Logger.Info($"Current Goal is {goal.Literal}");
                IEnumerable<CoResolutionResult> intimResults = this.ResolveLiteralGoal(ResolutionLiteralState.CloneConstructor(state, goal.Literal));
                results = intimResults.Select(r => new CoResolutionForAllResult(r.Success, r.Substitution, r.State, realGoal));
            }
            else if (goal.Child != null && goal.Child.ForAll != null)
            {
                stateCopy = (ResolutionStepState)state.Clone();
                results = this.ResolveForAllGoalPart(stateCopy, goal.Child);
            }

            foreach (CoResolutionForAllResult res in results)
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
                Mapping variableMapping = res.Substitution.Mappings.Where(m => m.Variable.Value == variable.Value).First();

                stateCopy = (ResolutionStepState)state.Clone();
                stateCopy.Substitution.BackPropagate(res.Substitution);
                stateCopy.Chs = res.CHS;
                _ = stateCopy.KeepUnbound.Remove(variable);

                // if variable is unbound return success and is not negativly constraint.
                if (!variableMapping.MapsTo.Term!.IsNegativelyConstrained())
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
                List<AtomParam> valuesToTry = variableMapping.Variable.ProhibitedValues.GetValues().ToList();
                valuesToTry.Sort((x, y) => x.ToString().CompareTo(y.ToString()));
                IEnumerable<CoResolutionForAllResult> constRes = this.ResolveForAllConstraint(stateCopy, goal, valuesToTry);
                foreach (CoResolutionForAllResult constraintResult in constRes)
                {
                    yield return constraintResult;
                }
            }

            yield return new CoResolutionForAllResult(false, stateCopy.Substitution, stateCopy, realGoal);
        }

        private IEnumerable<CoResolutionForAllResult> ResolveForAllConstraint(ResolutionStepState state, BodyPart goal, List<AtomParam> valuesToTry)
        {
            ArgumentNullException.ThrowIfNull(goal.ForAll, nameof(goal.ForAll));

            Term variable = (Term)goal.ForAll.Clone();
            variable.ProhibitedValues.Clear();

            ResolutionStepState stateCopy = (ResolutionStepState)state.Clone();
            Literal? realGoal = goal.Literal;
            AtomParam valueToTry = valuesToTry.First();
            List<AtomParam> nextValuesToTry = valuesToTry.Skip(1).ToList();

            // if child of the forall goal is a literal
            // we need to see if the literal resolves. And apply the forall rules accordingly.
            Substitution subToTry = new();
            subToTry.Add(variable, valueToTry);

            state.Logger.Info(
                $"FORALL: Variable to try for {variable}: {valueToTry} - [{string.Join(", ", nextValuesToTry.Select(a => a.ToString()))}] ");
            IEnumerable<CoResolutionForAllResult> results = new CoResolutionForAllResult[0];
            if (goal.Child == null && goal.Literal != null)
            {
                Literal subbedRealGoal = subToTry.Apply(goal.Literal);
                state.Logger.Info($"Current Goal is {subbedRealGoal}");
                IEnumerable<CoResolutionResult> intimResults = this.ResolveLiteralGoal(ResolutionLiteralState.CloneConstructor(state, subbedRealGoal));
                results = intimResults.Select(r => new CoResolutionForAllResult(r.Success, r.Substitution, r.State, realGoal));
            }
            else if (goal.Child != null && goal.Child.ForAll != null)
            {
                stateCopy = (ResolutionStepState)state.Clone();
                stateCopy.Substitution.BackPropagate(subToTry);
                results = this.ResolveForAllGoalPart(stateCopy, goal.Child);
            }

            foreach (CoResolutionForAllResult res in results)
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
                Mapping variableMapping = subToTry.Mappings.Where(m => m.Variable.Value == variable.Value).First();

                stateCopy = (ResolutionStepState)state.Clone();
                stateCopy.Substitution.BackPropagate(subToTry);
                stateCopy.Substitution.BackPropagate(res.Substitution);
                stateCopy.Chs = res.CHS;
                _ = stateCopy.KeepUnbound.Remove(variable);

                // if variable is unbound return success and is not negativly constraint.
                if (variableMapping.MapsTo.Term!.IsNegativelyConstrained())
                {
                    stateCopy.Logger.Error("Uncertain how to handle case where constrainted varaible returns constraint again. Failing for now.");
                    stateCopy.LogState();
                    stateCopy.Logger.Silly($"SubTree: {this.substitutionGroups}");
                    yield return new CoResolutionForAllResult(false, stateCopy.Substitution, stateCopy, realGoal);
                    yield break;
                }

                if (nextValuesToTry.Count > 0)
                {
                    IEnumerable<CoResolutionForAllResult> recResults = this.ResolveForAllConstraint(stateCopy, goal, nextValuesToTry);

                    foreach (CoResolutionForAllResult recRes in recResults)
                    {
                        yield return !recRes.Success ? new CoResolutionForAllResult(false, stateCopy.Substitution, stateCopy, realGoal) : recRes;
                    }
                }
                else
                {
                    yield return new CoResolutionForAllResult(true, stateCopy.Substitution, stateCopy, realGoal);
                }
            }

            // yield return new CoResolutionForAllResult(true, stateCopy.Substitution, stateCopy, realGoal);
        }

        // Recursively resolves a goal
        private IEnumerable<CoResolutionResult> ResolveLiteralGoal(ResolutionLiteralState state)
        {
            state.LogState();

            // var baseSub = PreprocessLiteralGoal(state.CurrentGoal);
            CheckerResult checkRes = this.CheckCHSAndCallStack(state);
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
            IEnumerable<CoResolutionResult> expansionResults = this.ResolveLiteralGoalByExpansion(state);

            foreach (CoResolutionResult expandsionRes in expansionResults)
            {
                state.Logger.Debug($"CallStack removing goal {state.CurrentGoal}");
                if (!expandsionRes.Success)
                {
                    yield return expandsionRes;
                    yield break;
                }

                ResolutionLiteralState stateClone = (ResolutionLiteralState)expandsionRes.State.Clone();
                Substitution subClone = state.Substitution.Clone();
                subClone.BackPropagate(expandsionRes.Substitution);
                subClone.Contract();

                yield return new CoResolutionResult(expandsionRes.Success, subClone, stateClone);
            }
        }

        private IEnumerable<CoResolutionResult> ResolveLiteralGoalByExpansion(ResolutionLiteralState state)
        {
            bool hasYielded = false;
            HashSet<string> variablesInGoal = this.variableExtractor.ExtractVariablesFrom(state.CurrentGoal).Select(t => t.Value).ToHashSet();

            Statement[] preselectedStatements = state.Statements
                .Where(s => this.preSelector.AreEqual(state.CurrentGoal, s.Head!))
                .Select(s => this.LinkAndRenameVariablesIn(s))
                .ToArray();

            state.Logger
                .Silly($"Preselected {preselectedStatements.Length} rules: [{string.Join(" | ", preselectedStatements.Select(s => s.ToString()))}]");

            foreach (Statement? statement in preselectedStatements)
            {
                if (statement.Head == null)
                {
                    continue;
                }

                UnificationResult unificationRes = this.unifier.Unify(statement.Head, state.CurrentGoal);
                if (unificationRes.Value == null)
                {
                    continue;
                }

                // if a variable that should be kept unbound gets bound asume a unifictaion fail.
                foreach (Mapping bound in unificationRes.Value.BoundMappings)
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
                IEnumerable<CoResolutionResult> results = this.ResolveAllGoals(
                    ResolutionRecursionState.CloneConstructor(statement.Body, this.allStatements, state.CallStack, state.Chs, unificationRes.Value, state.KeepUnbound, state.Logger));

                foreach (CoResolutionResult result in results)
                {
                    // this rule did not succeed try to find another one.
                    if (!result.Success)
                    {
                        state.Chs.SafeUnion(result.CHS, this.substitutionGroups);
                        continue;
                    }

                    ResolutionLiteralState stateClone = (ResolutionLiteralState)state.Clone();
                    UnificationResult reverseUnification = this.unifier.Unify(stateClone.CurrentGoal, statement.Head);

                    reverseUnification.Value!.BackPropagate(result.Substitution);
                    reverseUnification.Value.Contract();
                    reverseUnification.Value.Intersect(variablesInGoal);

                    Literal goalToAdd = reverseUnification.Value.Apply(stateClone.CurrentGoal);
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

        private IEnumerable<CoResolutionResult> ResolveOperation(Operation operation, ResolutionBaseState state)
        {
            CoResolutionResult res = this.operationResolver.ResolveOperation(operation, state);

            // this.substitutionTree.AddAllOf(res.Substitution);
            foreach (Mapping bound in res.Substitution.BoundMappings)
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
            foreach (AtomParam param in goal.Atom.ParamList)
            {
                if (param.Term != null && param.Term.IsVariable)
                {
                    string newVariableName = $"RV/{this.variableIndex}";
                    sub.Add(param.Term, new AtomParam(new Term(newVariableName)));
                    param.Term.Value = newVariableName;
                    this.variableIndex++;
                }
                else if (param.Literal != null)
                {
                    this.PreprocessLiteralGoal(param.Literal, sub);
                }
            }
        }

        private Literal GetForAllRule(BodyPart forall)
        {
            return forall.ForAll == null && (forall.Child == null || forall.Literal == null)
                ? throw new ArgumentException($"Body part needs to be a forall body part. Got ${forall}")
                : forall.Child != null
                ? this.GetForAllRule(forall.Child)
                : forall.Literal ?? throw new InvalidOperationException("Got body part that has neither a child or a literal, weird...");
        }

        private Statement LinkAndRenameVariablesIn(Statement statement)
        {
            Statement linkedStatement = this.linker.LinkVariables(statement);
            HashSet<Term> variables = this.variableExtractor.ExtractVariablesFrom(linkedStatement);
            foreach (Term variable in variables)
            {
                string newName = $"RV/{this.variableIndex}";

                variable.Value = newName;
                this.variableIndex++;
            }

            return linkedStatement;
        }
    }
}
