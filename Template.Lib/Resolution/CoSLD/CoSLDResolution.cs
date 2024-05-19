using Apollon.Lib.Atoms;
using Apollon.Lib.Resolution.CallStackAndCHS;
using Apollon.Lib.Resolution.Checkers;
using Apollon.Lib.Resolution.Checkers.CallStack;
using Apollon.Lib.Resolution.Checkers.CHSCheckers;
using Apollon.Lib.Rules;
using Apollon.Lib.Rules.Operations;
using Apollon.Lib.Unification;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public ResolutionResult Resolute(Statement[] statements, BodyPart[] goals)
        {
            // Initialize the call stack and CHS (coinductive hypothesis set)
            _allStatements = statements;
            var callStack = new Stack<Literal>();
            CHS chs = new CHS();

            // Start the resolution process
            var res = ResolveAllGoals(statements, goals, callStack, chs, new Substitution());

            if (!res.Success)
            {
                return new ResolutionResult();
            }

            return new ResolutionResult(chs, res.Substitution);
        }

        public CoResolutionResult ResolveAllGoals(Statement[] _statements, BodyPart[] goals, Stack<Literal> callStack, CHS chs, ISubstitution substitution)
        {
            ISubstitution sub = substitution.Clone();
            var statements = _statements.Select(s => (Statement)s.Clone()).ToArray();
            foreach (var goal in goals)
            {
                CoResolutionResult res = new CoResolutionResult();

                if (goal.ForAll != null)
                {
                    res = ResolveForAllGoal(statements, goal, callStack, chs, sub);
                } else if (goal.Literal != null)
                {
                    var substituted = sub.Apply(goal.Literal);
                    res = ResolveLiteralGoal(statements, substituted, callStack, chs);
                } else if (goal.Operation != null)
                {
                    res = ResolveOperation(goal.Operation, substitution);
                }

                if (!res.Success)
                {
                    return new CoResolutionResult();
                }

                sub.BackPropagate(res.Substitution);
                sub.Contract();
            }

            return new CoResolutionResult(true, sub);
        }

        private CoResolutionResult ResolveForAllGoal(Statement[] statements, BodyPart goal, Stack<Literal> callStack, CHS chs, ISubstitution substitution)
        {
            var forallRuleHead = GetForAllRule(goal);
            var forallRules = statements
                .Where(s => _unifier.Unify(s.Head, forallRuleHead).IsSuccess)
                .Select(s => (Statement)s.Clone())
                .Select(s => new Statement[] { s });

            var currentGoal = new BodyPart(forallRuleHead, null);
            var sub = substitution.Clone(); 

            foreach (var forallRule in forallRules)
            {
                var subbedGoal = new BodyPart[] { sub.Apply(new Statement(null, currentGoal)).Body[0] };
                var res = ResolveAllGoals(forallRule, subbedGoal, callStack, chs, new Substitution());

                if (!res.Success)
                {
                    return new CoResolutionResult();
                }
            }

            return new CoResolutionResult(true, new Substitution());
        }

        // Recursively resolves a goal
        private CoResolutionResult ResolveLiteralGoal(Statement[] statements, Literal goal, Stack<Literal> callStack, CHS chs)
        {
            var baseSub = PreprocessLiteralGoal(goal);
            var checkRes = CheckCHSAndCallStack(goal, callStack, chs);
            if (checkRes == CheckerResult.Succeed)
            {
                return new CoResolutionResult(true, new Substitution());
            }
            if (checkRes == CheckerResult.Fail)
            {
                return new CoResolutionResult();
            }

            callStack.Push(goal);
            var expansionRes = ResolveLiteralGoalByExpansion(statements, goal, callStack, chs);
            callStack.Pop();

            baseSub.BackPropagate(expansionRes.Substitution);
            baseSub.Contract();

            return new CoResolutionResult(expansionRes.Success, baseSub);
        }

        private CoResolutionResult ResolveLiteralGoalByExpansion(Statement[] statements, Literal goal, Stack<Literal> callStack, CHS chs)
        {
            foreach (var statement in statements)
            {
                if (statement.Head == null)
                {
                    throw new InvalidOperationException("TODO: Implement Constraints");
                }

                var unificationRes = _unifier.Unify(statement.Head, goal);
                if (unificationRes.Value == null) continue;

                // we expand the goal with this statment if it succeeds the goal gets added to the chs.
                var result = ResolveAllGoals(_allStatements, statement.Body, callStack, chs, unificationRes.Value);

                // this rule did not succeed try to find another one.
                if (!result.Success) continue;
                var reverseUnification = _unifier.Unify(goal, statement.Head);
                reverseUnification.Value.BackPropagate(result.Substitution);
                reverseUnification.Value.Contract();

                chs.Add(reverseUnification.Value.Apply(goal));
                return new CoResolutionResult(true, reverseUnification.Value);
            }

            // no statments where found that succeed for this statment. 
            return new CoResolutionResult();
        }

        public CheckerResult CheckCHSAndCallStack(Literal goal, Stack<Literal> callStack, CHS chs)
        {
            var chsCheck = _chsChecker.CheckCHSFor(goal, chs);

            if (chsCheck != CheckerResult.Continue) return chsCheck;

            return _callStackChecker.CheckCallStackFor(goal, callStack);
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
