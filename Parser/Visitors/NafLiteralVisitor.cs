using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib;

namespace AppollonParser.Visitors
{
    public class NafLiteralVisitor : apollonBaseVisitor<Literal>
    {
        private readonly LiteralVisitor _literalVisitor = new LiteralVisitor();

        public override Literal VisitNaf_literal(apollonParser.Naf_literalContext context)
        {
            var literal = _literalVisitor.VisitLiteral(context.literal());

            literal.IsNAF = context.NAF() != null;

            return literal;
        }
    }
}
