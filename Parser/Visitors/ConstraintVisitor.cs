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
        private readonly NafLiteralVisitor _nafLiteralVisitor = new NafLiteralVisitor();

        public override Constraint VisitConstraint(apollonParser.ConstraintContext context)
        {
            var bodyParts = new List<Literal>();

            foreach (var bodyPart in context.naf_literal())
            {
                bodyParts.Add(_nafLiteralVisitor.VisitNaf_literal(bodyPart));
            }

            return new Constraint(bodyParts.ToArray());
        }

    }
}
