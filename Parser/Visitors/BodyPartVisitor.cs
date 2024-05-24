using Apollon.Lib;
using Apollon.Lib.Rules;
using Apollon.Lib.Rules.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppollonParser.Visitors
{
    public class BodyPartVisitor : apollonBaseVisitor<BodyPart>
    {
        private readonly NafLiteralVisitor _nafLiteralVisitor = new NafLiteralVisitor();
        private static readonly OperationVisitor _operationVisitor = new OperationVisitor();

        public override BodyPart VisitBody_part(apollonParser.Body_partContext context)
        {
            Operation? operation = null;
            Literal? literal = null;

            if (context.inline_operation() != null)
            {
                operation = _operationVisitor.VisitInline_operation(context.inline_operation());  
            } else if (context.generating_operation() != null)
            {
                operation = _operationVisitor.VisitGenerating_operation(context.generating_operation());
            }
            if (context.naf_literal() != null)
            {
                literal = _nafLiteralVisitor.VisitNaf_literal(context.naf_literal());
            }

            return new BodyPart(literal, operation);
        }
    }
}
