using Apollon.Lib.Rules;
using Apollon.Lib;
using Apollon.Lib.Rules.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib.Atoms;
using Antlr4.Runtime.Misc;

namespace AppollonParser.Visitors
{
    public class OperationVisitor : apollonBaseVisitor<Operation>
    {
        private static readonly AtomParamVisitor _atomParamVisitor = new AtomParamVisitor();
        private static readonly AtomVisitor _atomVisitor = new AtomVisitor();

        public override Operation VisitInline_operation(apollonParser.Inline_operationContext context)
        {
            var variable = new Term(context.VARIABLE_TERM().GetText());
            var @operator = ParseInlineOperator(context.inline_operators());
            AtomParam condition;
            condition = _atomParamVisitor.VisitAtom_param_part(context.atom_param_part());

            return new Operation(new AtomParam(variable), @operator, condition);
        }

        public override Operation VisitGenerating_operation([NotNull] apollonParser.Generating_operationContext context)
        {
            var outputtingVariable = new Term(context.VARIABLE_TERM().GetText());
            var variable = new Term(context.generating_operation_variable().GetText());
            var operant = new Term(context.generating_operation_operant().GetText());
            var @operator = ParseGeneratingOperator(context.generating_operators());

            return new Operation(outputtingVariable, new AtomParam(variable), @operator, operant);
        }

        public Operator ParseGeneratingOperator(apollonParser.Generating_operatorsContext context)
        {
            if (context.PLUS() != null)
            {
                return Operator.Plus;
            }
            else if (context.NEGATION() != null)
            {
                return Operator.Minus;
            }
            else if (context.TIMES() != null)
            {
                return Operator.Times;
            }
            else if (context.DIVIDE() != null)
            {
                return Operator.Divide;
            }

            throw new ParseException($"Unahndled operator {context.GetText()}");
        }

        public Operator ParseInlineOperator(apollonParser.Inline_operatorsContext context)
        {
            if (context.EQUALS() != null)
            {
                return Operator.Equals;
            } else if (context.NOT_EQUALS() != null)
            {
                return Operator.NotEquals;
            } else if (context.LARGER() != null)
            {
                return Operator.GreaterThan;
            } else if (context.SMALLER() != null)
            {
                return Operator.LessThan;
            }
            else if (context.LARGER_EQUALS() != null)
            {
                return Operator.GreaterThanOrEqual;
            } else if (context.SMALLER_EQUALS() != null)
            {
                return Operator.LessThanOrEqual;
            }

            throw new ParseException($"Unahndled operator {context.GetText()}");
        }
    }
}
