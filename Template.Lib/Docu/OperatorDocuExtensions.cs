using Apollon.Lib.Rules.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Docu
{
    public static class OperatorDocuExtensions
    {

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
