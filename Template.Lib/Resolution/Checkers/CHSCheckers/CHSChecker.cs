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
        private IUnifier unifer = new ExactUnifier();
        private IUnifier constructiveUnifier = new ConstructiveUnifier();
        private VariableExtractor extractor = new VariableExtractor();
        private VariableLinker linker = new VariableLinker();

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
            var goalCopy = (Literal)goal.Clone();
            goalCopy.IsNAF = !goalCopy.IsNAF;

            this.linker.LinkVariables(new Statement(goal));
            var goalVariables = this.extractor.ExtractVariablesFrom(goal);

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
            var copy = (Literal)literal.Clone();
            copy.IsNAF = !copy.IsNAF;

            return chs.Literals.Where(l => this.unifer.Unify(l, copy).IsSuccess).Any();
        }
    }
}
