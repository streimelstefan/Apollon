using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib;

namespace AppollonParser.Visitors
{
    /// <summary>
    /// A visitor that creates <see cref="Constraint"/>s.
    /// </summary>
    public class ConstraintVisitor : apollonBaseVisitor<Constraint>
    {
        private readonly BodyPartVisitor bodyPartVisitor = new BodyPartVisitor();

        /// <summary>
        /// Generates a new <see cref="Constraint"/>.
        /// </summary>
        /// <param name="context">The context of the new constraint.</param>
        /// <returns>The new constraint.</returns>
        public override Constraint VisitConstraint(apollonParser.ConstraintContext context)
        {
            var bodyParts = new List<BodyPart>();

            foreach (var bodyPart in context.body_part())
            {
                bodyParts.Add(bodyPartVisitor.VisitBody_part(bodyPart));
            }

            return new Constraint(bodyParts.ToArray());
        }

    }
}
