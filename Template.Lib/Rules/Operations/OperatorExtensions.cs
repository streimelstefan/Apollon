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
            switch (@operator)
            {
                case Operator.Equals:
                    return "=";
                case Operator.NotEquals:
                    return "!=";
                case Operator.GreaterThan:
                    return ">";
                case Operator.LessThan:
                    return "<";
                case Operator.Plus:
                    return "+";
                case Operator.Minus:
                    return "-";
                case Operator.Times:
                    return "*";
                case Operator.Divide:
                    return "/";
                case Operator.LessThanOrEqual:
                    return "<=";
                case Operator.GreaterThanOrEqual:
                    return ">=";
                default:
                    throw new InvalidOperationException("Unhandled case.");
            }
        }
    }
}
