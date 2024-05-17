using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Unification.DisagreementFinders
{
    public class DisagreementFinder : IDisagreementFinder
    {
        public DisagreementResult FindDisagreement(Statement s1, Statement s2)
        {
            var headRes = FindDisagreement(s1.Head, s2.Head);

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
                var res = FindDisagreement(s1.Body[i], s2.Body[i]);

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
            if (lit1 == null && lit2 == null)
            {
                return new DisagreementResult(new Disagreement());
            }
            if (!(lit1 != null && lit2 != null))
            {
                return new DisagreementResult($"Non fixable disagreement. Missing Literal. {lit1} != {lit2}");
            }

            if (lit1.IsNAF != lit2.IsNAF)
            {
                return new DisagreementResult($"Non fixable disagreement. NAF. {lit1} != {lit2}");
            }
            if (lit1.IsNegative != lit2.IsNegative)
            {
                return new DisagreementResult($"Non fixable disagreement. Negative. {lit1} != {lit2}");
            }

            return FindDisagreement(lit1.Atom, lit2.Atom);
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

            for (var i = 0; i < atom1.ParamList.Length; i++)
            {
                var res = FindDisagreement(atom1.ParamList[i], atom2.ParamList[i]);

                if (res.Value != null && !res.Value.IsEmpty) // a disagreement was found.
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
            if (param1.Equals(param2))
            {
                return new DisagreementResult(new Disagreement());
            }

            if (param1.Term != null && param1.Term.IsVariable || param2.Term != null && param2.Term.IsVariable)
            {
                return new DisagreementResult(new Disagreement(param1, param2));
            }

            if (param1.IsTerm != param2.IsTerm)
            {
                return new DisagreementResult($"Non fixable disagreement. Uncompatable type. {param1} != {param2}");
            }
            if (param1.IsLiteral != param2.IsLiteral)
            {
                return new DisagreementResult($"Non fixable disagreement. Uncompatable type. {param1} != {param2}");
            }

            if (param1.Literal != null && param2.Literal != null)
            {
                return FindDisagreement(param1.Literal, param2.Literal);
            }

            if (param1.Term != null && param2.Term != null)
            {
                if (param1.Term.Value != param2.Term.Value)
                {
                    return new DisagreementResult($"Non fixable disagreement. Different term. {param1} != {param2}");
                }

                return new DisagreementResult(new Disagreement());
            }

            throw new InvalidOperationException("This part should never be reached.");
        }

        private DisagreementResult FindDisagreement(BodyPart part1, BodyPart part2)
        {
            if (part1.IsLiteral != part2.IsLiteral)
            {
                return new DisagreementResult($"Non fixable disagreement. Uncompatable type. {part1} != {part2}");
            }

            if (part1.IsOperation != part2.IsOperation)
            {
                return new DisagreementResult($"Non fixable disagreement. Uncompatable type. {part1} != {part2}");
            }

            if (part1.Literal != null && part2.Literal != null)
            {
                return FindDisagreement(part1.Literal, part2.Literal);
            }

            throw new InvalidOperationException("TODO: Implement Forall and Operations");
        }

    }
}
