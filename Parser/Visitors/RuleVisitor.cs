using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib;
using Template.Lib.Rules;

namespace AppollonParser.Visitors
{
    public class RuleVisitor : apollonBaseVisitor<Rule>
    {
        private readonly LiteralVisitor _literalVisitor = new LiteralVisitor();
        private readonly NafLiteralVisitor _nafLiteralVisitor = new NafLiteralVisitor();

        public override Rule VisitRule(apollonParser.RuleContext context)
        {
            var head = _literalVisitor.VisitLiteral(context.head().literal());
            var bodyLiterals = new List<Literal>();
            
            foreach (var literal in context.body().naf_literal())
            {
                bodyLiterals.Add(_nafLiteralVisitor.VisitNaf_literal(literal));
            }

            return new Rule(head, bodyLiterals.ToArray());
        }

    }
}
