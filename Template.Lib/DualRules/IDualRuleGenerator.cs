using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.DualRules
{
    /// <summary>
    /// A generator that generates dual rules from statements.
    /// </summary>
    public interface IDualRuleGenerator
    {
        /// <summary>
        /// Generates dual rules from the given statements.
        /// </summary>
        /// <param name="statements">The statements for which dual rules should be created for.</param>
        /// <returns>The dual rule that have been created.</returns>
        DualRule[] GenerateDualRules(Statement[] statements);

    }
}
