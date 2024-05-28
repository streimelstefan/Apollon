using Apollon.Lib.Rules;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Docu
{
    /// <summary>
    /// Represents a part of a <see cref="Program"/> that can be documented.
    /// </summary>
    public interface IDocumentation
    {
        /// <summary>
        /// The literal the docuemtation is based on.
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
