//-----------------------------------------------------------------------
// <copyright file="ConstraintVisitor.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace AppollonParser.Visitors
{
    using Apollon.Lib.Rules;

    /// <summary>
    /// A visitor that creates <see cref="Constraint"/>s.
    /// </summary>
    public class ConstraintVisitor : apollonBaseVisitor<Constraint>
    {
        private readonly BodyPartVisitor bodyPartVisitor = new();

        /// <summary>
        /// Generates a new <see cref="Constraint"/>.
        /// </summary>
        /// <param name="context">The context of the new constraint.</param>
        /// <returns>The new constraint.</returns>
        public override Constraint VisitConstraint(apollonParser.ConstraintContext context)
        {
            List<BodyPart> bodyParts = new();

            foreach (apollonParser.Body_partContext? bodyPart in context.body_part())
            {
                bodyParts.Add(this.bodyPartVisitor.VisitBody_part(bodyPart));
            }

            return new Constraint(bodyParts.ToArray());
        }
    }
}
