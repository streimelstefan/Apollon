//-----------------------------------------------------------------------
// <copyright file="OperationVisitor.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace AppollonParser.Visitors
{
    using Antlr4.Runtime.Misc;
    using Apollon.Lib;
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Rules.Operations;

    /// <summary>
    /// A visitor that generates <see cref="Operation"/>s.
    /// </summary>
    public class OperationVisitor : apollonBaseVisitor<Operation>
    {
        private static readonly AtomParamVisitor AtomParamVisitor = new();
        private static readonly AtomVisitor AtomVisitor = new();

        /// <summary>
        /// Generates a new <see cref="Operation"/> in the inline format.
        /// </summary>
        /// <param name="context">The inline operation context.</param>
        /// <returns>The new operation.</returns>
        public override Operation VisitInline_operation(apollonParser.Inline_operationContext context)
        {
            Term variable = new(context.VARIABLE_TERM().GetText());
            Operator @operator = this.ParseInlineOperator(context.inline_operators());
            AtomParam condition;
            condition = AtomParamVisitor.VisitAtom_param_part(context.atom_param_part());

            return new Operation(new AtomParam(variable), @operator, condition);
        }

        /// <summary>
        /// Generates a new <see cref="Operation"/> in the generating format.
        /// </summary>
        /// <param name="context">The generating operation context.</param>
        /// <returns>The new operation.</returns>
        public override Operation VisitGenerating_operation([NotNull] apollonParser.Generating_operationContext context)
        {
            Term outputtingVariable = new(context.VARIABLE_TERM().GetText());
            Term variable = new(context.generating_operation_variable().GetText());
            Term operant = new(context.generating_operation_operant().GetText());
            Operator @operator = this.ParseGeneratingOperator(context.generating_operators());

            return new Operation(outputtingVariable, new AtomParam(variable), @operator, operant);
        }

        private Operator ParseGeneratingOperator(apollonParser.Generating_operatorsContext context)
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

        private Operator ParseInlineOperator(apollonParser.Inline_operatorsContext context)
        {
            if (context.EQUALS() != null)
            {
                return Operator.Equals;
            }
            else if (context.NOT_EQUALS() != null)
            {
                return Operator.NotEquals;
            }
            else if (context.LARGER() != null)
            {
                return Operator.GreaterThan;
            }
            else if (context.SMALLER() != null)
            {
                return Operator.LessThan;
            }
            else if (context.LARGER_EQUALS() != null)
            {
                return Operator.GreaterThanOrEqual;
            }
            else if (context.SMALLER_EQUALS() != null)
            {
                return Operator.LessThanOrEqual;
            }

            throw new ParseException($"Unahndled operator {context.GetText()}");
        }
    }
}
