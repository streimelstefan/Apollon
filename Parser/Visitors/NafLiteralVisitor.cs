using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib;

namespace AppollonParser.Visitors
{
    /// <summary>
    /// A visitor that generates <see cref="Literal"/>s. Should be used for NAF literals.
    /// </summary>
    public class NafLiteralVisitor : apollonBaseVisitor<Literal>
    {
        private readonly LiteralVisitor _literalVisitor = new LiteralVisitor();

        /// <summary>
        /// Generates a new <see cref="Literal"/>.
        /// </summary>
        /// <param name="context">The context of a naf literal.</param>
        /// <returns>The new literal.</returns>
        public override Literal VisitNaf_literal(apollonParser.Naf_literalContext context)
        {
            var literal = _literalVisitor.VisitLiteral(context.literal());

            literal.IsNAF = context.NAF() != null;

            return literal;
        }
    }
}
