//-----------------------------------------------------------------------
// <copyright file="IDocumentation.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Docu
{
    using System.Text;
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// Represents a part of a <see cref="Program"/> that can be documented.
    /// </summary>
    public interface IDocumentation
    {
        /// <summary>
        /// Gets the literal the docuemtation is based on.
        /// </summary>
        Literal Literal { get; }

        /// <summary>
        /// Generates the documentation for the given literal using the given substitution.
        /// </summary>
        /// <param name="sub">The substituion to build the documentation with.</param>
        /// <returns>The string representing the generated documentation.</returns>
        StringBuilder GetDokuFor(Substitution sub);
    }
}
