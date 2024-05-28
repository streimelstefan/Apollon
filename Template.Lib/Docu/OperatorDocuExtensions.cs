using Apollon.Lib.Rules.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Docu
{
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
            switch (@operator)
            {
                case Operator.Equals:
                    return " is ";
                case Operator.NotEquals:
                    return " is not ";
                case Operator.GreaterThan:
                    return " is greater than ";
                case Operator.LessThan:
                    return " is less than ";
                case Operator.Plus:
                    return " plus ";
                case Operator.Minus:
                    return " minus ";
                case Operator.Times:
                    return " times ";
                case Operator.Divide:
                    return " divided by ";
                case Operator.LessThanOrEqual:
                    return " is less than or equal to ";
                case Operator.GreaterThanOrEqual:
                    return " is greater than or equal to ";
                default:
                    throw new InvalidOperationException("Unhandled case.");
            }
        }

    }
}
