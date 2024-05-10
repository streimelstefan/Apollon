using Apollon.Lib.Atoms;
using Apollon.Lib.Resolution.CallStackAndCHS;
using Apollon.Lib.Rules;
using Apollon.Lib.Rules.Operations;
using Apollon.Lib.Unification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution
{
    public class CoSLDResolution : IResolution
    {
        private IUnifier _unifier = new ConstructiveUnifier();

        // this should work with a normal unifier. 
        private ICoinductiveCHSChecker _coinductiveCHSChecker = new CoinductiveCHSChecker(new Unifier());

        public ResolutionResult Resolute(Statement[] statements, BodyPart[] goals)
        {
            //return RecResolution(statements, goals, new Substitution(), new CHS());
            return BetterResolute(statements, goals);
        }

        /// <summary>
        /// TODO: Preprocess goals so the samly named variables have the same term object, so they share the same PVL.
        /// </summary>
        /// <param name="statements"></param>
        /// <param name="goals"></param>
        /// <returns></returns>
        private ResolutionResult BetterResolute(Statement[] statements, BodyPart[] goals)
        {
            // 1. Peek last element of call stack with suff in appling rules
            // 2. If Goal Literal
            // 2.1. CHS Check with current Goal
            // 2.2.1. If Succeed -> clear current Stacks appling rules
            // 2.2.2. If Fail -> Pop call stack
            // 2.3. Unify gal with first appling rule.
            // 2.4. Take first element of first appling rule. If rule gets empty by duing so remove it.
            // 2.5. Take first element of unified rule and add it as a new call stack item.
            PreprocessOriginalGoals(goals);
            var callStack = new CallStack();
            // add all goals in reverse order
            callStack.Items.AddRange(goals.Reverse().Select(g => new CallStack.CallStackItem(g, GetApplingRulesFor(g, statements), new Substitution())));
            CCHSResult lastCallRes = CCHSResult.Continue;

            while (!callStack.NoMoreRulesToCheck() && !callStack.IsEmpty)
            {
                CallStack.CallStackItem currentStack;
                currentStack = callStack.Peek();

                if (currentStack.CurrentGoal.Literal != null)
                {
                    var currentGoal = currentStack.ApplingSubstitution.Apply(currentStack.CurrentGoal.Literal);
                    var chsCheck = _coinductiveCHSChecker.CheckCHSFor(
                        currentGoal, 
                        lastCallRes == CCHSResult.Continue ? callStack.ConvertToCHSWithoutLast() : callStack.ConverToCHS());
                    if (lastCallRes != CCHSResult.Fail && chsCheck == CCHSResult.Succeed)
                    {
                        currentStack.ApplingRules.Clear();
                        lastCallRes = CCHSResult.Succeed;
                        continue;
                    }
                    if (lastCallRes != CCHSResult.Fail && chsCheck == CCHSResult.Fail)
                    {
                        callStack.Pop();
                        lastCallRes = CCHSResult.Fail;
                        continue;
                    }

                    if (lastCallRes == CCHSResult.Fail)
                    {
                        currentStack.ApplingRules.TryDequeue(out _);
                        if (currentStack.ApplingRules.Count() == 0)
                        {
                            lastCallRes = CCHSResult.Fail;
                            callStack.Pop();
                            continue;
                        }
                    }

                    var nextRule = currentStack.ApplingRules.Peek();
                    var unificationRes = _unifier.Unify(nextRule.Head, currentGoal);
                    var substituted = unificationRes.Value.Apply(nextRule);

                    BodyPart nextGoal;
                    if (nextRule.Body.Length == 0)
                    {
                        lastCallRes = CCHSResult.Succeed;
                        continue;
                    } else
                    {
                        nextGoal = nextRule.Body[0];
                    }
                    nextRule.Body = nextRule.Body.Skip(1).ToArray();
                    callStack.Add(nextGoal, GetApplingRulesFor(substituted.Body[0], statements), unificationRes.Value);

                    lastCallRes = CCHSResult.Continue;
                } else if (currentStack.CurrentGoal.Operation != null)
                {
                    callStack.Pop();
                    if (!ResoluteOperation(currentStack.CurrentGoal.Operation, currentStack.ApplingSubstitution))
                    {
                        var caller = callStack.PeekFirstNonFinished();
                        lastCallRes = CCHSResult.Fail;
                    }
                }
            }

            return new ResolutionResult(callStack.ConverToCHS(), new Substitution());
        }

        private Queue<Statement> GetApplingRulesFor(BodyPart goal, Statement[] statements)
        {
            if (goal.Literal == null)
            {
                return new Queue<Statement>();
            }
            return new Queue<Statement>(statements
                .Where(s => s.Head != null && _unifier.Unify(goal.Literal, s.Head).IsSuccess)
                .Select(s => (Statement)s.Clone()));
        }

        private void PreprocessOriginalGoals(BodyPart[] goals)
        {
            var variableIndex = 0;
            foreach (var param in goals[0].Literal.Atom.ParamList)
            {
                if (param.Term != null && param.Term.IsVariable)
                {
                    param.Term.Value = $"RV/{variableIndex}";
                    variableIndex++;
                }
            }
        }

        private CoResolutionResult RecResolution(Statement[] statements, BodyPart[] goals, ISubstitution substitution, CHS chs)
        {

            foreach (var currentGoal in goals)
            {
                CoResolutionResult newChs;
                if (currentGoal.Literal != null)
                {
                    newChs = ResoluteLiteral(statements, substitution.Apply(currentGoal.Literal), chs);
                }
                else if (currentGoal.Operation != null)
                {
                    if (!ResoluteOperation(currentGoal.Operation, substitution))
                    {
                        return new CoResolutionResult();
                    }
                    continue;
                }
                else
                {
                    throw new InvalidOperationException("TODO: Implement Forall");
                }

                if (newChs.CHS.IsEmpty) // if new goals is empty. The current goals was a fact and can be added to the chs.
                {
                    return new CoResolutionResult();
                }

                chs.SafeUnion(newChs.CHS);
                foreach (var mapping in newChs.Substitution.Mappings)
                {
                    substitution.Add(mapping.Variable, mapping.MapsTo);
                }
            }

            return new CoResolutionResult(chs, substitution);
        }

        private CoResolutionResult ResoluteLiteral(Statement[] statements, Literal currentGoal, CHS chs)
        {
            foreach (var statement in statements)
            {
                if (statement.Head == null)
                {
                    throw new InvalidOperationException("TODO implement constraints");
                }

                // see if head unifies with goal
                var unificationRes = _unifier.Unify(statement.Head, currentGoal);

                if (unificationRes.Value == null)
                {
                    continue;
                }

                var substituted = unificationRes.Value.Apply(statement);

                // check all body parts for recursivly for unification 
                var recChs = RecResolution(statements, statement.Body, unificationRes.Value, chs);

                if (recChs.CHS.IsEmpty && substituted.Body.Length != 0) // if new goals is empty. The current goals was a fact and can be added to the chs.
                {
                    //chs.Pop(); // remove current head again since this call did not succeed.
                    continue;
                }

                chs.SafeUnion(recChs.CHS);

                return new CoResolutionResult(chs, unificationRes.Value);
            }

            chs.Pop();
            return new CoResolutionResult();
        }

        private bool ResoluteOperation(Operation operation, ISubstitution substitution)
        {
            if (operation.Operator == Operator.Equals)
            {
                var op = substitution.Apply(operation);
                return op.Condition.Equals(op.Variable.Literal);
            }
            else
            {
                if (operation.Variable.Term == null || !operation.Variable.Term.IsVariable)
                {
                    throw new InvalidOperationException("operation variable is not variable...");
                }
                operation.Variable.Term.ProhibitedValues.AddValue(new AtomParam(operation.Condition));
                var op = substitution.Apply(operation);
                return !op.Condition.Equals(op.Variable.Literal);
            }
        }
    }
}
