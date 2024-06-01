//-----------------------------------------------------------------------
// <copyright file="IUnifier.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Unification
{
    using Apollon.Lib.Rules;
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// The interface for every Unifier.
    /// </summary>
    public interface IUnifier
    {
        /// <summary>
        /// Used to unify two Literals.
        /// </summary>
        /// <param name="unifier">The literal that should be unified with.</param>
        /// <param name="against">The literal that should be unified against.</param>
        /// <param name="sigma">The sigma that should be used for unification.</param>
        /// <returns>The UnificationResult.</returns>
        UnificationResult Unify(Literal unifier, Literal against, Substitution sigma);

        /// <summary>
        /// Used to unify two BodyParts.
        /// </summary>
        /// <param name="unifier">The BodyPart that should be unified with.</param>
        /// <param name="against">The BodyPart that should be unified against.</param>
        /// <param name="sigma">The sigma that should be used for unification.</param>
        /// <returns>The UnificationResult.</returns>
        UnificationResult Unify(BodyPart unifier, BodyPart against, Substitution sigma);

        /// <summary>
        /// Used to unify two Literals.
        /// </summary>
        /// <param name="unifier">The literal that should be unified with.</param>
        /// <param name="against">The literal that should be unified against.</param>
        /// <returns>The UnificationResult.</returns>
        UnificationResult Unify(Literal unifier, Literal against);

        /// <summary>
        /// Used to unify two BodyParts.
        /// </summary>
        /// <param name="unifier">The BodyPart that should be unified with.</param>
        /// <param name="against">The BodyPart that should be unified against.</param>
        /// <returns>The UnificationResult.</returns>
        UnificationResult Unify(BodyPart unifier, BodyPart against);
    }
}
