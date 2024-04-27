using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib;

namespace AppollonParser.Visitors
{
    public class LiteralVisitor : apollonBaseVisitor<Literal>
    {
        private readonly AtomVisitor _atomVisitor = new AtomVisitor();

        public override Literal VisitLiteral(apollonParser.LiteralContext context)
        {
            var atom = _atomVisitor.VisitAtom(context.atom());

            return new Literal(atom, false, context.NEGATION() != null);
        }

    }
}
