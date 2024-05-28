using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Linker
{
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
