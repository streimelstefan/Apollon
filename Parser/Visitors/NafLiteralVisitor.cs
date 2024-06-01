//-----------------------------------------------------------------------
// <copyright file="NafLiteralVisitor.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace AppollonParser.Visitors
{
    using Apollon.Lib;

    /// <summary>
    /// A visitor that generates <see cref="Literal"/>s. Should be used for NAF literals.
    /// </summary>
    public class NafLiteralVisitor : apollonBaseVisitor<Literal>
    {
        private readonly LiteralVisitor literalVisitor = new();

        /// <summary>
        /// Generates a new <see cref="Literal"/>.
        /// </summary>
        /// <param name="context">The context of a naf literal.</param>
        /// <returns>The new literal.</returns>
        public override Literal VisitNaf_literal(apollonParser.Naf_literalContext context)
        {
            Literal literal = this.literalVisitor.VisitLiteral(context.literal());

            literal.IsNAF = context.NAF() != null;

            return literal;
        }
    }
}
