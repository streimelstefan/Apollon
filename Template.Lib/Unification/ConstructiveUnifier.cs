﻿//-----------------------------------------------------------------------
// <copyright file="ConstructiveUnifier.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Unification
{
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Unification.DisagreementFinders;
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// A unifier that uses the constructive unification algorithm.
    /// </summary>
    public class ConstructiveUnifier : IUnifier
    {
        private readonly IDisagreementFinder disagreementFinder = new DisagreementFinder();
        private readonly IUnifier unifier = new Unifier();

        /// <summary>
        /// Unifies the two given literals.
        /// </summary>
        /// <param name="unifier">The literal that should be unified with.</param>
        /// <param name="against">The literal that should be unified against.</param>
        /// <returns>A UnificationResult.</returns>
        public UnificationResult Unify(Literal unifier, Literal against)
        {
            return this.Unify(new Statement(unifier), new Statement(against), new Substitution());
        }

        /// <summary>
        /// Unifies the two given BodyParts.
        /// </summary>
        /// <param name="unifier">The bodypart that should be unified with.</param>
        /// <param name="against">The bodypart that should be unified against.</param>
        /// <returns>A UnificationResult.</returns>
        public UnificationResult Unify(BodyPart unifier, BodyPart against)
        {
            return this.Unify(new Statement(null, unifier), new Statement(null, against), new Substitution());
        }

        /// <summary>
        /// Unifies the two given literals.
        /// </summary>
        /// <param name="unifier">The literal that should be unified with.</param>
        /// <param name="against">The literal that should be unified against.</param>
        /// <param name="sigma">The sigma that should be used.</param>
        /// <returns>A UnificationResult.</returns>
        public UnificationResult Unify(Literal unifier, Literal against, Substitution sigma)
        {
            return this.Unify(new Statement(unifier), new Statement(against), sigma);
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
            return this.Unify(new Statement(null, unifier), new Statement(null, against), sigma);
        }

        /// <summary>
        /// Unifies the two given Statements.
        /// </summary>
        /// <param name="unifier">The Statement that should be unified with.</param>
        /// <param name="against">The Statement that should be unified against.</param>
        /// <param name="sigma">The sigma that should be used.</param>
        /// <returns>A UnificationResult.</returns>
        public UnificationResult Unify(Statement unifier, Statement against, Substitution sigma)
        {
            while (true)
            {
                Statement tUnifier = sigma.Apply(unifier);
                Statement tAgainst = sigma.Apply(against);

                if (this.IsSingleton(tUnifier, tAgainst)) // are equal
                {
                    return new UnificationResult(sigma); // All terms are unified under the current substitution
                }

                DisagreementResult disagreementSet = this.disagreementFinder.FindDisagreement(tUnifier, tAgainst);
                if (disagreementSet.IsError) // a non fixable disagreement has ocured.
                {
                    return new UnificationResult($"Statements are not unifiable. {disagreementSet.Error}");
                }

                (Term variable, AtomParam value) = this.ChooseTermsToResolve(disagreementSet);

                // see if t unifies with one value of the pvl of s.
                if (!this.CheckPVLOf(variable, value))
                {
                    // not a valid substitution.
                    return new UnificationResult("Substitution in PVL.");
                }

                sigma.Add(variable, value); // Add this substitution
            }
        }

        private bool CheckPVLOf(Term term, AtomParam substitution)
        {
            if (substitution.Literal != null)
            {
                Literal checkerLiteral = new(new Atom(term.Value), false, false);
                foreach (Literal? prohibitedValue in term.ProhibitedValues.GetValues().Where(v => v.Literal != null).Select(v => v.Literal))
                {
                    if (prohibitedValue == null)
                    {
                        continue;
                    }

                    UnificationResult res = this.unifier.Unify(unifier: prohibitedValue, checkerLiteral);

                    if (res.IsSuccess)
                    {
                        return false;
                    }
                }
            }
            else if (substitution.Term != null)
            {
                // if both are variables the the pvls of both will be unioned.
                if (substitution.Term.IsVariable)
                {
                    PVL.Union(substitution.Term.ProhibitedValues, term.ProhibitedValues);
                    return true;
                }

                foreach (Term? prohibitedValue in term.ProhibitedValues.GetValues().Where(v => v.Term != null).Select(v => v.Term))
                {
                    if (term.Equals(prohibitedValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsSingleton(Statement unifier, Statement against)
        {
            return unifier.Equals(against);
        }

        private (Term Variable, AtomParam Value) ChooseTermsToResolve(DisagreementResult res)
        {
            if (res.Value == null)
            {
                throw new ArgumentException("Value of result is not allowed to be null.");
            }

            if (res.Value.First == null || res.Value.Second == null)
            {
                throw new ArgumentException("Neither first or second of the result are allowed to be null.");
            }

            if (res.Value.First.Term != null && res.Value.First.Term.IsVariable)
            {
                return (res.Value.First.Term, res.Value.Second);
            }
            else if (res.Value.Second.Term != null && res.Value.Second.Term.IsVariable)
            {
                return (res.Value.Second.Term, res.Value.First);
            }

            throw new InvalidOperationException("Not allowed to end here.");
        }
    }
}
