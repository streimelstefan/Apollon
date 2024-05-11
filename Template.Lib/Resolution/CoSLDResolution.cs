using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using Apollon.Lib.Rules.Operations;
using Apollon.Lib.Unification;
using System;
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
            var res = RecResolution(statements, goals, new Substitution(), new CHS());

            if (res.CHS.IsEmpty)
            {
                return new ResolutionResult();
            }

            return new ResolutionResult(res.Result, new Substitution());
        }

        private void PreprocessGoals(BodyPart[] goals)
        {
            var variables = new Dictionary<string, Term>();

        }

        private CoResolutionResult RecResolution(Statement[] statements, BodyPart[] goals, ISubstitution substitution, CHS chs)
        {
            var result = new CHS();
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
                result.SafeUnion(newChs.Result);
                foreach (var mapping in newChs.Substitution.Mappings)
                {
                    substitution.Add(mapping.Variable, mapping.MapsTo);
                }
            }

            return new CoResolutionResult(chs, substitution, result);
        }

        private CoResolutionResult ResoluteLiteral(Statement[] statements, Literal currentGoal, CHS chs)
        {
            // check for chs failure or success
            var chsCheck = _coinductiveCHSChecker.CheckCHSFor(currentGoal, chs);
            var result = new CHS();
            if (chsCheck == CCHSResult.Succeed)
            {
                // we dont need to add the current goal again since it is already present once in the chs
                // if we get here. 
                return new CoResolutionResult(chs, new Substitution(), result);
            }
            if (chsCheck == CCHSResult.Fail)
            {
                return new CoResolutionResult();
            }
            chs.Add(currentGoal);

            var variableIndex = 0;
            foreach (var param in currentGoal.Atom.ParamList)
            {
                if (param.Term != null && param.Term.IsVariable)
                {
                    param.Term.Value = $"RV/{variableIndex}";
                    variableIndex++;
                }
            }

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
                    continue;
                }
                result.SafeUnion(recChs.Result);
                result.Add(recChs.Substitution.Apply(currentGoal));

                return new CoResolutionResult(chs, unificationRes.Value, result);
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
