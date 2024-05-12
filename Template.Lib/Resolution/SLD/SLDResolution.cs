using Apollon.Lib.Resolution.CallStackAndCHS;
using Apollon.Lib.Rules;
using Apollon.Lib.Rules.Operations;
using Apollon.Lib.Unification;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.SLD
{
    public class SLDResolution : IResolution
    {
        public ResolutionResult Resolute(Statement[] statements, BodyPart[] goals)
        {
            return RecResolution(statements, goals);
        }

        private ResolutionResult RecResolution(Statement[] statements, BodyPart[] goals)
        {
            var chs = new CHS();
            var substitution = new Substitution();

            foreach (var currentGoal in goals)
            {
                ResolutionResult newChs;
                if (currentGoal.Literal != null)
                {
                    newChs = ResoluteLiteral(statements, currentGoal.Literal);
                }
                else if (currentGoal.Operation != null)
                {
                    if (!ResoluteOperation(currentGoal.Operation))
                    {
                        return new ResolutionResult();
                    }
                    continue;
                }
                else
                {
                    throw new InvalidOperationException("TODO: Implement Forall");
                }

                if (newChs.CHS.IsEmpty) // if new goals is empty. The current goals was a fact and can be added to the chs.
                {
                    return new ResolutionResult();
                }

                chs.SafeUnion(newChs.CHS);
                foreach (var mapping in newChs.Substitution.Mappings)
                {
                    substitution.Add(mapping.Variable, mapping.MapsTo);
                }
            }

            return new ResolutionResult(chs, substitution);
        }

        private ResolutionResult ResoluteLiteral(Statement[] statements, Literal currentGoal)
        {
            IUnifier unifier = new Unifier();
            var chs = new CHS();

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
                var unificationRes = unifier.Unify(statement.Head, currentGoal);

                if (unificationRes.Value == null)
                {
                    continue;
                }

                var substituted = unificationRes.Value.Apply(statement);

                // check all body parts for recursivly for unification 
                var recChs = RecResolution(statements, substituted.Body);

                if (recChs.CHS.IsEmpty && substituted.Body.Length != 0) // if new goals is empty. The current goals was a fact and can be added to the chs.
                {
                    continue;
                }

                chs.SafeUnion(recChs.CHS);

                // if body is fully unifiable add substituted head to chs
                chs.Add(recChs.Substitution.Apply(substituted).Head);

                return new ResolutionResult(chs, unificationRes.Value);
            }

            return new ResolutionResult();
        }

        private bool ResoluteOperation(Operation operation)
        {
            if (operation.Operator == Operator.Equals)
            {
                return operation.Condition.Equals(operation.Variable.Literal);
            }
            else
            {
                return !operation.Condition.Equals(operation.Variable.Literal);
            }
        }
    }
}
