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
    /// <summary>
    /// A visitor that generates <see cref="Literal"/>s.
    /// </summary>
    public class FactVisitor : apollonBaseVisitor<Literal>
    {

        private static readonly LiteralVisitor _literalVisitor = new LiteralVisitor();

        /// <summary>
        /// Generates a new <see cref="Literal/>.
        /// </summary>
        /// <param name="context">The literal context.</param>
        /// <returns>The new literal.</returns>
        public override Literal VisitFact(apollonParser.FactContext context)
        {
            var literal = _literalVisitor.VisitLiteral(context.literal());

            return literal;
        }

    }
}
