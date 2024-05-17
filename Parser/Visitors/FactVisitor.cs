using Antlr4.Runtime.Misc;
using Apollon.Lib;
using Apollon.Lib.Docu;
using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppollonParser.Visitors
{
    public class FactVisitor : apollonBaseVisitor<Literal>
    {

        public static readonly LiteralVisitor _literalVisitor = new LiteralVisitor();

        public override Literal VisitFact(apollonParser.FactContext context)
        {
            var literal = _literalVisitor.VisitLiteral(context.literal());

            return literal;
        }

    }
}
