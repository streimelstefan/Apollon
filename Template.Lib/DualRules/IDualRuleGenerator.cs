//-----------------------------------------------------------------------
// <copyright file="IDualRuleGenerator.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.DualRules
{
    using Apollon.Lib.Rules;

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
