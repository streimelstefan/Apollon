//-----------------------------------------------------------------------
// <copyright file="FactVisitor.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace AppollonParser.Visitors
{
    using Apollon.Lib;

    /// <summary>
    /// A visitor that generates <see cref="Literal"/>s.
    /// </summary>
    public class FactVisitor : apollonBaseVisitor<Literal>
    {
        private static readonly LiteralVisitor LiteralVisitor = new();

        /// <summary>
        /// Generates a new <see cref="Literal/>.........................
        /// </summary>
        /// <param name="context">The literal context.</param>
        /// <returns>The new literal.</returns>
        public override Literal VisitFact(apollonParser.FactContext context)
        {
            Literal literal = LiteralVisitor.VisitLiteral(context.literal());

            return literal;
        }
    }
}
