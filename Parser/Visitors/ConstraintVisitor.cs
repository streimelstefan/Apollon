using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib;

namespace AppollonParser.Visitors
{
    public class ConstraintVisitor : apollonBaseVisitor<Constraint>
    {
        private readonly LiteralVisitor _literalVisitor = new LiteralVisitor();
        private readonly NafLiteralVisitor _nafLiteralVisitor = new NafLiteralVisitor();

        public override Constraint VisitConstraint(apollonParser.ConstraintContext context)
        {
            var bodyLiterals = new List<Literal>();

            foreach (var literal in context.body().naf_literal())
            {
                bodyLiterals.Add(_nafLiteralVisitor.VisitNaf_literal(literal));
            }

            return new Constraint(bodyLiterals.ToArray());
        }

    }
}
