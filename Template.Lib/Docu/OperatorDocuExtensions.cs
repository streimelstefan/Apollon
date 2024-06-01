//-----------------------------------------------------------------------
// <copyright file="OperatorDocuExtensions.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Docu
{
    using Apollon.Lib.Rules.Operations;

    /// <summary>
    /// Extension methods for the <see cref="Operator"/> enum.
    /// </summary>
    public static class OperatorDocuExtensions
    {
        /// <summary>
        /// Returns the documentation string for the given operator.
        /// </summary>
        /// <param name="operator">The operator to get the string for.</param>
        /// <returns>The string representation of the oprator.</returns>
        /// <exception cref="InvalidOperationException">If there is an unhandled case.</exception>
        public static string ToDocumentationString(this Operator @operator)
        {
            return @operator switch
            {
                Operator.Equals => " is ",
                Operator.NotEquals => " is not ",
                Operator.GreaterThan => " is greater than ",
                Operator.LessThan => " is less than ",
                Operator.Plus => " plus ",
                Operator.Minus => " minus ",
                Operator.Times => " times ",
                Operator.Divide => " divided by ",
                Operator.LessThanOrEqual => " is less than or equal to ",
                Operator.GreaterThanOrEqual => " is greater than or equal to ",
                _ => throw new InvalidOperationException("Unhandled case."),
            };
        }
    }
}
