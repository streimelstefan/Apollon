//-----------------------------------------------------------------------
// <copyright file="CHSChecker.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Resolution.Checkers.CHSCheckers
{
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Linker;
    using Apollon.Lib.Resolution.CallStackAndCHS;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Unification;

    /// <summary>
    /// Checks the CHS for conditions that allow to Co SLD resolution to succeed or fail early.
    /// </summary>
    public class CHSChecker : ICoinductiveCHSChecker
    {
        private readonly IUnifier unifer = new ExactUnifier();
        private readonly IUnifier constructiveUnifier = new ConstructiveUnifier();
        private readonly VariableExtractor extractor = new();
        private readonly VariableLinker linker = new();

        /// <summary>
        /// Checks the CHS for loops.
        /// </summary>
        /// <param name="literal">The Literal that the loop should be checked for.</param>
        /// <param name="chs">The stack of all Literals that should be checked.</param>
        /// <returns>Returns an Enumerable containing the Result of the Check.</returns>
        public CheckerResult CheckCHSFor(Literal literal, CHS chs)
        {
            if (this.IsPresentWithNAFSwitch(literal, chs))
            {
                return CheckerResult.Fail;
            }

            // if the chs contains the literal
            if (chs.Literals.Where(l => this.unifer.Unify(literal, l).IsSuccess).Any())
            {
                return CheckerResult.Succeed;
            }

            this.ConstraintAgainstCHS(chs, literal);

            return CheckerResult.Continue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="chs"></param>
        /// <param name="goal"></param>
        private void ConstraintAgainstCHS(CHS chs, Literal goal)
        {
            Literal goalCopy = (Literal)goal.Clone();
            goalCopy.IsNAF = !goalCopy.IsNAF;

            _ = this.linker.LinkVariables(new Statement(goal));
            HashSet<Term> goalVariables = this.extractor.ExtractVariablesFrom(goal);

            if (goalVariables.Count() == 0)
            {
                return;
            }

            foreach (Literal literal in chs.Literals)
            {
                UnificationResult res = this.constructiveUnifier.Unify(goalCopy, literal);

                if (res.Value != null)
                {
                    foreach (Term goalVariable in goalVariables)
                    {
                        foreach (Mapping mapping in res.Value.Mappings)
                        {
                            if (mapping.Variable.Value == goalVariable.Value)
                            {
                                AtomParam mappedVariableCopy = (AtomParam)mapping.MapsTo.Clone();
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
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="literal"></param>
        /// <param name="chs"></param>
        /// <returns></returns>
        private bool IsPresentWithNAFSwitch(Literal literal, CHS chs)
        {
            Literal copy = (Literal)literal.Clone();
            copy.IsNAF = !copy.IsNAF;

            return chs.Literals.Where(l => this.unifer.Unify(l, copy).IsSuccess).Any();
        }
    }
}
