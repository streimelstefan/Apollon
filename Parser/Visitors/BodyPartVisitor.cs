//-----------------------------------------------------------------------
// <copyright file="BodyPartVisitor.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace AppollonParser.Visitors
{
    using Apollon.Lib;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Rules.Operations;

    /// <summary>
    /// A visitor that creates a new <see cref="BodyPart"/>.
    /// </summary>
    public class BodyPartVisitor : apollonBaseVisitor<BodyPart>
    {
        private readonly NafLiteralVisitor nafLiteralVisitor = new();
        private static readonly OperationVisitor OperationVisitor = new();

        /// <summary>
        /// Generates a new <see cref="BodyPart"/>.
        /// </summary>
        /// <param name="context">The context of the new body part.</param>
        /// <returns>The new body part.</returns>
        public override BodyPart VisitBody_part(apollonParser.Body_partContext context)
        {
            Operation? operation = null;
            Literal? literal = null;

            if (context.inline_operation() != null)
            {
                operation = OperationVisitor.VisitInline_operation(context.inline_operation());
            }
            else if (context.generating_operation() != null)
            {
                operation = OperationVisitor.VisitGenerating_operation(context.generating_operation());
            }

            if (context.naf_literal() != null)
            {
                literal = this.nafLiteralVisitor.VisitNaf_literal(context.naf_literal());
            }

            return new BodyPart(literal, operation);
        }
    }
}
