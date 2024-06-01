//-----------------------------------------------------------------------
// <copyright file="IDocumentationGenerator.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Docu
{
    /// <summary>
    /// Represents a documentation generator that can generate documentation for a <see cref="Program"/>.
    /// </summary>
    public interface IDocumentationGenerator
    {
        /// <summary>
        /// Generates the documentation for the given <see cref="Program"/>.
        /// </summary>
        /// <param name="program">The program the documentation should be generated for.</param>
        /// <returns>The documentation of the program.</returns>
        string GenerateDokumentationFor(Program program);
    }
}
