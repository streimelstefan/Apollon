//-----------------------------------------------------------------------
// <copyright file="IVariableLinker.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Linker
{
    using Apollon.Lib.Rules;

    /// <summary>
    /// A linker that links all the variables in a statement.
    /// </summary>
    public interface IVariableLinker
    {
        /// <summary>
        /// Links all the variables in a given statement. After linking all the variables with the same name will be the same object.
        /// </summary>
        /// <param name="statement">The statement to link the variables for.</param>
        /// <returns>The new statment where the variables are linked.</returns>
        Statement LinkVariables(Statement statement);
    }
}
