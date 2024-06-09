//-----------------------------------------------------------------------
// <copyright file="CHSChecker.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Resolution.Checkers.CHSCheckers
{
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Graph;
    using Apollon.Lib.Linker;
    using Apollon.Lib.Resolution.CallStackAndCHS;
    using Apollon.Lib.Resolution.CoSLD.States;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Unification;
    using Apollon.Lib.Unification.Substitutioners;
    using System.Reflection.Metadata;

    /// <summary>
    /// Checks the CHS for conditions that allow to Co SLD resolution to succeed or fail early.
    /// </summary>
    public class CHSChecker : ICoinductiveCHSChecker
    {
        private readonly IUnifier unifer = new ExactUnifier();
        private readonly IUnifier constructiveUnifier = new ConstructiveUnifier();
        private readonly VariableExtractor extractor = new();
        private readonly VariableLinker linker = new();
        private readonly IEqualizer<Literal> equalizer = new LiteralParamCountEqualizer();

        /// <summary>
        /// Checks the CHS for loops.
        /// </summary>
        /// <param name="literal">The Literal that the loop should be checked for.</param>
        /// <param name="chs">The stack of all Literals that should be checked.</param>
        /// <returns>Returns an Enumerable containing the Result of the Check.</returns>
        public CheckerResult CheckCHSFor(Literal literal, CHS chs, ResolutionLiteralState state)
        {
            var original = (Literal)literal.Clone();
            state.Substitution.ApplyInline(literal);

            if (this.IsPresentWithNAFSwitch(literal, chs))
            {
                // see if there is a literal that unifies with the original
                var boundConstraint = this.CreateConstraintOfBound(original, chs, literal, state);

                if (boundConstraint != null)
                {
                    state.CurrentGoal = boundConstraint;
                    return CheckerResult.CheckChangedValues;
                }

                return CheckerResult.Fail;
            }

            if (chs.Literals.Where(l => this.unifer.Unify(literal, l).IsSuccess).Any())
            {
                return CheckerResult.Succeed;
            }

            // if the chs contains the literal
            this.ConstraintAgainstCHS(chs, literal);
            // if (this.IsPresentWithNAFSwitch(literal, chs))
            // {
            //     return CheckerResult.Fail;
            // }
            // 
            if (chs.Literals.Where(l => this.unifer.Unify(literal, l).IsSuccess).Any())
            {
                return CheckerResult.Succeed;
            }

            return CheckerResult.Continue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="chs"></param>
        /// <param name="goal"></param>
        private void ConstraintAgainstCHS(CHS chs, Literal goal, bool subOnly = false)
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
                                    var tmpSub = new Substitution();
                                    var comparer = new StringComparer();
                                    tmpSub.Add(goalVariable, mappedVariableCopy.Term.ProhibitedValues.GetValues().OrderBy(t => t.ToString(), comparer).First());
                                    tmpSub.ApplyInline(goal);
                                    return;
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

        private Literal? CreateConstraintOfBound(Literal original, CHS chs, Literal challenge, ResolutionLiteralState state)
        {
            var orgCopy = (Literal)original.Clone();
            orgCopy.IsNAF = !orgCopy.IsNAF;

            _ = this.linker.LinkVariables(new Statement(orgCopy));
            HashSet<Term> goalVariables = this.extractor.ExtractVariablesFrom(orgCopy);

            var variablesToCheck = new List<Term>();
            foreach (var variable in goalVariables)
            {
                if (state.BodyOnlyLiteralAndVars.ContainsKey(variable.Value))
                {
                    variablesToCheck.Add(variable);
                } else
                {
                    var subCopy = state.Substitution.Clone();
                    foreach (var mappin in subCopy.Mappings.ToArray())
                    {
                        if (mappin.Variable.Value != variable.Value)
                        {
                            subCopy.Remove(mappin.Variable);
                        }
                    }

                    subCopy.ApplyInline(orgCopy);
                }
            }

            if (variablesToCheck.Count == 0)
            {
                return null;
            }

            return CreateConstraintOfBoundRec(orgCopy, chs, challenge, state, variablesToCheck);
        }

        private Literal? CreateConstraintOfBoundRec(Literal original, CHS chs, Literal challenge, ResolutionLiteralState state, List<Term> vars)
        {
            var orgCopy = (Literal)original.Clone();

            if (vars.Count() == 0)
            {
                orgCopy.IsNAF = !orgCopy.IsNAF;
                return orgCopy;
            }

            var myVar = vars.First();

            foreach (var literal in chs.Literals)
            {
                UnificationResult res = this.constructiveUnifier.Unify(orgCopy, literal);

                if (res.Value != null)
                {
                    foreach (Mapping mapping in res.Value.Mappings)
                    {
                        if (mapping.Variable.Value == myVar.Value)
                        {
                            AtomParam mappedVariableCopy = (AtomParam)mapping.MapsTo.Clone();
                            if (mappedVariableCopy.Term != null && mappedVariableCopy.Term.IsNegativelyConstrained())
                            {
                                var comparer = new StringComparer();

                                foreach (var value in mappedVariableCopy.Term.ProhibitedValues.GetValues().OrderBy(t => t.ToString(), comparer))
                                {
                                    var copy2 = (Literal)orgCopy.Clone();
                                    var tmpSub = new Substitution();
                                    tmpSub.Add(myVar, value);
                                    tmpSub.ApplyInline(copy2);

                                    var recRes = this.CreateConstraintOfBoundRec(copy2, chs, challenge, state, vars.Skip(1).ToList());

                                    if (recRes == null)
                                    {
                                        return null;
                                    }

                                    if (this.unifer.Unify(recRes, challenge).IsSuccess)
                                    {
                                        continue;
                                    }

                                    state.Logger.Info($"Current Goal is: {recRes}");

                                    var recResCopy = (Literal)recRes.Clone();
                                    recResCopy.IsNAF = !recResCopy.IsNAF;
                                    var isInChsAsNot = false;
                                    foreach (var lit in chs.Literals)
                                    {
                                        if (this.unifer.Unify(lit, recResCopy).IsSuccess)
                                        {
                                            isInChsAsNot = true;
                                            break;
                                        }
                                    }

                                    if (isInChsAsNot)
                                    {
                                        continue;
                                    }

                                    return recRes;
                                }
                            }
                        }
                    }
                }
            }
            return null;
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

        // private bool Constraint(Literal goal, Literal literal)
        // {
        //     if (!this.equalizer.AreEqual(goal, literal)) { return false; }
        // 
        //     return this.Constraint(goal.Atom, literal.Atom);
        // }
        // 
        // private bool Constraint(Atom goal, Atom atom)
        // {
        //     for (int i = 0; i < goal.ParamList.Length; i++)
        //     {
        //         if (goal.ParamList[i].Term != null && goal.ParamList[i].Term.IsVariable)
        //         {
        //             if (atom.ParamList[i].Term != null && atom.ParamList[i].Term.IsVariable)
        //             {
        // 
        //             }
        //         }
        // 
        //         if (goal.ParamList[i].IsLiteral && atom.ParamList[i].IsLiteral)
        //         {
        //             var res = this.Constraint(goal.ParamList[i].Literal, atom.ParamList[i].Literal);
        //             if (!res) return res;
        //         }
        //     }
        // }
    }
}
