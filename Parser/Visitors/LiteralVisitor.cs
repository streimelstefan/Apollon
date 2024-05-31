using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib;

namespace AppollonParser.Visitors
{
    /// <summary>
    /// A visitor that generates <see cref="Literal"/>s.
    /// </summary>
    public class LiteralVisitor : apollonBaseVisitor<Literal>
    {
        private readonly AtomVisitor _atomVisitor = new AtomVisitor();

        /// <summary>
        /// Generates a new <see cref="Literal"/>.
        /// </summary>
        /// <param name="context">The context of the new literal.</param>
        /// <returns>The new literal.</returns>
        public override Literal VisitLiteral(apollonParser.LiteralContext context)
        {
            var atom = _atomVisitor.VisitAtom(context.atom());

            return new Literal(atom, false, context.NEGATION() != null);
        }

    }
}
