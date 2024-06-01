//-----------------------------------------------------------------------
// <copyright file="OperatorExtensions.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Rules.Operations
{
    /// <summary>
    /// A class of extension methods for the Operator enum.
    /// </summary>
    public static class OperatorExtensions
    {
        /// <summary>
        /// Converts an Operator to a friendly string.
        /// </summary>
        /// <param name="operator">The operator that should be converted.</param>
        /// <returns>The commonly used string representation of that string.</returns>
        /// <exception cref="InvalidOperationException">In case of an unhandled case.</exception>
        public static string ToFriendlyString(this Operator @operator)
        {
            return @operator switch
            {
                Operator.Equals => "=",
                Operator.NotEquals => "!=",
                Operator.GreaterThan => ">",
                Operator.LessThan => "<",
                Operator.Plus => "+",
                Operator.Minus => "-",
                Operator.Times => "*",
                Operator.Divide => "/",
                Operator.LessThanOrEqual => "<=",
                Operator.GreaterThanOrEqual => ">=",
                _ => throw new InvalidOperationException("Unhandled case."),
            };
        }
    }
}
