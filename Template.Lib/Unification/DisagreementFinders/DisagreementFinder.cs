//-----------------------------------------------------------------------
// <copyright file="DisagreementFinder.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Unification.DisagreementFinders
{
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Rules;

    /// <summary>
    /// The DisagreementFinder is used to find disagreements between two Statements.
    /// </summary>
    public class DisagreementFinder : IDisagreementFinder
    {
        /// <summary>
        /// Finds the disagreement between two Statements.
        /// </summary>
        /// <param name="s1">First Statement.</param>
        /// <param name="s2">Second Statement.</param>
        /// <returns>Returns a Result of the Disagreement.</returns>
        public DisagreementResult FindDisagreement(Statement s1, Statement s2)
        {
            DisagreementResult headRes = this.FindDisagreement(s1.Head, s2.Head);

            if (headRes.IsError)
            {
                return headRes;
            }

            // if there is no disagreement in the head continue to the body.
            // else return the disagreement.
            if (headRes.Value != null && !headRes.Value.IsEmpty)
            {
                return headRes;
            }

            if (s1.Body.Length != s2.Body.Length)
            {
                return new DisagreementResult($"Non fixable disagreement. Body Length. {s1} != {s2}");
            }

            for (int i = 0; i < s1.Body.Length; i++)
            {
                DisagreementResult res = this.FindDisagreement(s1.Body[i], s2.Body[i]);

                if (res.IsError)
                {
                    return res;
                }

                if (res.Value != null && !res.Value.IsEmpty)
                {
                    return res;
                }
            }

            return new DisagreementResult(new Disagreement());
        }

        private DisagreementResult FindDisagreement(Literal? lit1, Literal? lit2)
        {
            return lit1 == null && lit2 == null
                ? new DisagreementResult(new Disagreement())
                : !(lit1 != null && lit2 != null)
                ? new DisagreementResult($"Non fixable disagreement. Missing Literal. {lit1} != {lit2}")
                : lit1.IsNAF != lit2.IsNAF
                ? new DisagreementResult($"Non fixable disagreement. NAF. {lit1} != {lit2}")
                : lit1.IsNegative != lit2.IsNegative
                ? new DisagreementResult($"Non fixable disagreement. Negative. {lit1} != {lit2}")
                : this.FindDisagreement(lit1.Atom, lit2.Atom);
        }

        private DisagreementResult FindDisagreement(Atom atom1, Atom atom2)
        {
            if (atom1.Name != atom2.Name)
            {
                return new DisagreementResult($"Non fixable disagreement. Name. {atom1} != {atom2}");
            }

            if (atom1.ParamList.Length != atom2.ParamList.Length)
            {
                return new DisagreementResult($"Non fixable disagreement. Param length. {atom1} != {atom2}");
            }

            for (int i = 0; i < atom1.ParamList.Length; i++)
            {
                DisagreementResult res = this.FindDisagreement(atom1.ParamList[i], atom2.ParamList[i]);

                // a disagreement was found.
                if (res.Value != null && !res.Value.IsEmpty)
                {
                    return res;
                }

                if (res.IsError)
                {
                    return res;
                }
            }

            return new DisagreementResult(new Disagreement());
        }

        private DisagreementResult FindDisagreement(AtomParam param1, AtomParam param2)
        {
            return param1.Equals(param2)
                ? new DisagreementResult(new Disagreement())
                : (param1.Term != null && param1.Term.IsVariable) || (param2.Term != null && param2.Term.IsVariable)
                ? new DisagreementResult(new Disagreement(param1, param2))
                : param1.IsTerm != param2.IsTerm
                ? new DisagreementResult($"Non fixable disagreement. Uncompatable type. {param1} != {param2}")
                : param1.IsLiteral != param2.IsLiteral
                ? new DisagreementResult($"Non fixable disagreement. Uncompatable type. {param1} != {param2}")
                : param1.Literal != null && param2.Literal != null
                ? this.FindDisagreement(param1.Literal, param2.Literal)
                : param1.Term != null && param2.Term != null
                ? param1.Term.Value != param2.Term.Value
                    ? new DisagreementResult($"Non fixable disagreement. Different term. {param1} != {param2}")
                    : new DisagreementResult(new Disagreement())
                : throw new InvalidOperationException("This part should never be reached.");
        }

        private DisagreementResult FindDisagreement(BodyPart part1, BodyPart part2)
        {
            return part1.IsLiteral != part2.IsLiteral
                ? new DisagreementResult($"Non fixable disagreement. Uncompatable type. {part1} != {part2}")
                : part1.IsOperation != part2.IsOperation
                ? new DisagreementResult($"Non fixable disagreement. Uncompatable type. {part1} != {part2}")
                : part1.Literal != null && part2.Literal != null
                ? this.FindDisagreement(part1.Literal, part2.Literal)
                : throw new InvalidOperationException("TODO: Implement Forall and Operations");
        }
    }
}
