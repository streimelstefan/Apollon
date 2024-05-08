﻿using Apollon.Lib.Rules;
using Apollon.Lib;
using Apollon.Lib.Rules.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib.Atoms;

namespace AppollonParser.Visitors
{
    public class OperationVisitor : apollonBaseVisitor<Operation>
    {
        private static readonly NafLiteralVisitor _nafLiteralVisitor = new NafLiteralVisitor();
        private static readonly AtomVisitor _atomVisitor = new AtomVisitor();

        public override Operation VisitOperation(apollonParser.OperationContext context)
        {
            
            var variable = new Term(context.VARIABLE_TERM().GetText());
            var @operator = ParseOperator(context.@operator());
            Literal condition;
            if (context.naf_literal() != null)
            {
                condition = _nafLiteralVisitor.VisitNaf_literal(context.naf_literal());
            } else if (context.NUMBER() != null)
            {
                condition = new Literal(new Atom(context.NUMBER().GetText()), false, false);
            } else
            {
                throw new InvalidProgramException("Condition of an operation needs to be an variable or an atom.");
            }

            return new Operation(new AtomParam(variable), @operator, condition);
        }

        public Operator ParseOperator(apollonParser.OperatorContext context)
        {
            if (context.EQUALS() != null)
            {
                return Operator.Equals;
            } else
            {
                return Operator.NotEquals;
            }
        }
    }
}
