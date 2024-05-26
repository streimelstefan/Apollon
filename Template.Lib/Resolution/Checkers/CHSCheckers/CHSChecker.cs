using Apollon.Lib.Atoms;
using Apollon.Lib.Linker;
using Apollon.Lib.Resolution.CallStackAndCHS;
using Apollon.Lib.Rules;
using Apollon.Lib.Unification;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution.Checkers.CHSCheckers
{
    public class CHSChecker : ICoinductiveCHSChecker
    {
        private IUnifier _unifer = new ExactUnifier();
        private IUnifier constructiveUnifier = new ConstructiveUnifier();
        private VariableExtractor _extractor = new VariableExtractor();
        private VariableLinker _linker = new VariableLinker();  

        public CheckerResult CheckCHSFor(Literal literal, CHS chs)
        {
            if (IsPresentWithNAFSwitch(literal, chs))
                return CheckerResult.Fail;

            // if the chs contains the literal
            if (chs.Literals.Where(l => _unifer.Unify(literal, l).IsSuccess).Any()) return CheckerResult.Succeed;

            this.ConstraintAgainsCHS(chs, literal);

            return CheckerResult.Continue;
        }

        private void ConstraintAgainsCHS(CHS chs, Literal goal)
        {
            var goalCopy = (Literal)goal.Clone();
            goalCopy.IsNAF = !goalCopy.IsNAF;

            this._linker.LinkVariables(new Statement(goal));
            var goalVariables = this._extractor.ExtractVariablesFrom(goal);

            if (goalVariables.Count() == 0)
            {
                return;
            }

            foreach (var literal in chs.Literals)
            {
                var res = this.constructiveUnifier.Unify(goalCopy, literal);

                if (res.Value != null)
                {
                    foreach (var goalVariable in goalVariables)
                    {
                        foreach (var mapping in res.Value.Mappings)
                        {
                            if (mapping.Variable.Value == goalVariable.Value)
                            {
                                var mappedVariableCopy = (AtomParam)mapping.MapsTo.Clone();
                                if (mappedVariableCopy.Term != null && mappedVariableCopy.Term.IsNegativelyConstrained())
                                {
                                    mappedVariableCopy.Term.ProhibitedValues.Clear();
                                }

                                if (mapping.MapsTo.Term != null && mapping.MapsTo.Term.IsVariable)
                                {
                                    continue;
                                }

                                try
                                {
                                    goalVariable.ProhibitedValues.AddValue(mappedVariableCopy);
                                } catch (Exception)
                                {
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool IsPresentWithNAFSwitch(Literal literal, CHS chs)
        {
            var copy = (Literal)literal.Clone();
            copy.IsNAF = !copy.IsNAF;

            return chs.Literals.Where(l => _unifer.Unify(l, copy).IsSuccess).Any();
        }

    }
}
