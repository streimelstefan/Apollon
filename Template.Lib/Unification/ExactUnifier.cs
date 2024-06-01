//-----------------------------------------------------------------------
// <copyright file="ExactUnifier.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Unification
{
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Rules.Operations;
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// A unifier that uses the exact unification algorithm. Meaning that literals need to be exaclty the same
    /// with the sole exception beeing variable names.
    /// </summary>
    public class ExactUnifier : IUnifier
    {
        /// <summary>
        /// Unifies the two given literals.
        /// </summary>
        /// <param name="unifier">The literal that should be unified with.</param>
        /// <param name="against">The literal that should be unified against.</param>
        /// <param name="sigma">The sigma that should be used.</param>
        /// <returns>A UnificationResult.</returns>
        public UnificationResult Unify(Literal unifier, Literal against, Substitution sigma)
        {
            return this.Unify(unifier, against);
        }

        /// <summary>
        /// Unifies the two given BodyParts.
        /// </summary>
        /// <param name="unifier">The BodyPart that should be unified with.</param>
        /// <param name="against">The BodyPart that should be unified against.</param>
        /// <param name="sigma">The sigma that should be used.</param>
        /// <returns>A UnificationResult.</returns>
        public UnificationResult Unify(BodyPart unifier, BodyPart against, Substitution sigma)
        {
            return this.Unify(unifier, against);
        }

        /// <summary>
        /// Unifies the two given literals.
        /// </summary>
        /// <param name="unifier">The literal that should be unified with.</param>
        /// <param name="against">The literal that should be unified against.</param>
        /// <returns>A UnificationResult.</returns>
        public UnificationResult Unify(Literal unifier, Literal against)
        {
            UnificationResult res = this.Unify(unifier.Atom, against.Atom);
            return res.IsError
                ? res
                : unifier.IsNAF != against.IsNAF
                ? new UnificationResult($"NAF is not the same | {unifier} \\= {against}")
                : unifier.IsNegative != against.IsNegative ? new UnificationResult($"Negation is not the same | {unifier} \\= {against}") : res;
        }

        /// <summary>
        /// Unifies the two given BodyParts.
        /// </summary>
        /// <param name="unifier">The bodypart that should be unified with.</param>
        /// <param name="against">The bodypart that should be unified against.</param>
        /// <returns>A UnificationResult.</returns>
        public UnificationResult Unify(BodyPart unifier, BodyPart against)
        {
            if (unifier.IsLiteral != against.IsLiteral || unifier.IsForAll != against.IsForAll ||
                unifier.HasChild != against.HasChild || unifier.IsOperation != unifier.IsOperation)
            {
                return new UnificationResult($"Type does not match | {unifier} \\= {against}");
            }

            // here for all can be ignored since the forall term needs to be varaible anyway
            return unifier.Literal != null && against.Literal != null
                ? this.Unify(unifier.Literal, against.Literal)
                : unifier.Operation != null && against.Operation != null
                ? this.Unify(unifier.Operation, against.Operation)
                : unifier.Child != null && against.Child != null
                ? this.Unify(unifier.Child, against.Child)
                : throw new InvalidOperationException($"Unahndled Case within Exact Unification => {unifier} {against}");
        }

        /// <summary>
        /// Unifies the two given Atoms.
        /// </summary>
        /// <param name="atom1">The Atom that should be unified with.</param>
        /// <param name="atom2">The Atom that should be unified against.</param>
        /// <returns>A UnificationResult.</returns>
        public UnificationResult Unify(Atom atom1, Atom atom2)
        {
            if (atom1.Name != atom2.Name)
            {
                return new UnificationResult($"Name missmatch => {atom1} \\= {atom2}");
            }

            if (atom1.ParamList.Length != atom2.ParamList.Length)
            {
                return new UnificationResult($"ParamList length missmatch => {atom1} \\= {atom2}");
            }

            for (int i = 0; i < atom1.ParamList.Length; i++)
            {
                UnificationResult res = this.Unify(atom1.ParamList[i], atom2.ParamList[i]);
                if (res.IsError)
                {
                    return res;
                }
            }

            return new UnificationResult(new Substitution());
        }

        /// <summary>
        /// Unifies the two given AtomParams.
        /// </summary>
        /// <param name="param1">The AtomParam that should be unified with.</param>
        /// <param name="param2">The AtomParam that should be unified against.</param>
        /// <returns>A UnificationResult.</returns>
        public UnificationResult Unify(AtomParam param1, AtomParam param2)
        {
            return param1.IsTerm != param2.IsTerm || param2.IsLiteral != param1.IsLiteral
                ? new UnificationResult($"Type missmatch => {param1} \\= {param2}")
                : param1.Literal != null && param2.Literal != null
                ? this.Unify(param1.Literal, param2.Literal)
                : param1.Term != null && param2.Term != null
                ? this.Unify(param1.Term, param2.Term)
                : throw new InvalidOperationException($"Unhandled Case in Exact Unification => {param1} {param2}");
        }

        /// <summary>
        /// Unifies the two given Operations.
        /// </summary>
        /// <param name="op1">The Operation that should be unified with.</param>
        /// <param name="op2">The Operation that should be unified against.</param>
        /// <returns>A UnificationResult.</returns>
        public UnificationResult Unify(Operation op1, Operation op2)
        {
            UnificationResult res = this.Unify(op1.Variable, op2.Variable);
            return res.IsError
                ? res
                : op1.Operator != op2.Operator
                ? new UnificationResult($"Operation missmatch => {op1} \\= {op2}")
                : this.Unify(op1.Condition, op2.Condition);
        }

        /// <summary>
        /// Unifies the two given Terms.
        /// </summary>
        /// <param name="term1">The Term that should be unified with.</param>
        /// <param name="term2">The Term that should be unified against.</param>
        /// <returns>A UnificationResult.</returns>
        public UnificationResult Unify(Term term1, Term term2)
        {
            if (term1.IsVariable != term2.IsVariable)
            {
                return new UnificationResult($"Type missmatch => {term1} \\= {term2}");
            }

            if (term1.IsVariable && term2.IsVariable)
            {
                AtomParam[] pvl1 = term1.ProhibitedValues.GetValues().ToArray();
                AtomParam[] pvl2 = term2.ProhibitedValues.GetValues().ToArray();

                // if both pvls are equal succeed if not fail.
                if (pvl1.Length != pvl2.Length)
                {
                    return new UnificationResult($"PVL length missmatch => {term1} \\= {term2}");
                }

                for (int i = 0; i < pvl1.Length; i++)
                {
                    UnificationResult res = this.Unify(pvl1[i], pvl2[i]);
                    if (res.IsError)
                    {
                        return res;
                    }
                }

                return new UnificationResult(new Substitution());
            }

            return term1.Value != term2.Value
                ? new UnificationResult($"Name missmatch => {term1} \\= {term2}")
                : new UnificationResult(new Substitution());
        }
    }
}
