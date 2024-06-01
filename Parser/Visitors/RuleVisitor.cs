//-----------------------------------------------------------------------
// <copyright file="RuleVisitor.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace AppollonParser.Visitors
{
    using Apollon.Lib;
    using Apollon.Lib.Rules;

    /// <summary>
    /// A visitor that generates <see cref="Rule"/>s.
    /// </summary>
    public class RuleVisitor : apollonBaseVisitor<Rule>
    {
        private readonly LiteralVisitor literalVisitor = new();
        private static readonly BodyPartVisitor BodyPartVisitor = new();

        /// <summary>
        /// Generates a new <see cref="Rule"/>.
        /// </summary>
        /// <param name="context">The context of the new rule.</param>
        /// <returns>The new rule.</returns>
        public override Rule VisitRule(apollonParser.RuleContext context)
        {
            apollonParser.LiteralContext headContext = context.head().literal();
            Literal head = this.literalVisitor.VisitLiteral(headContext);
            List<BodyPart> bodyParts = new();

            foreach (apollonParser.Body_partContext? bodyPart in context.body().body_part())
            {
                bodyParts.Add(BodyPartVisitor.VisitBody_part(bodyPart));
            }

            return new Rule(head, bodyParts.ToArray());
        }
    }
}
