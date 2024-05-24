using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Rules.Operations
{
    public static class OperatorExtensions
    {
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
