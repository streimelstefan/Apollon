//-----------------------------------------------------------------------
// <copyright file="LiteralVisitor.cs" company="Streimel and Prix">
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
    public class LiteralVisitor : apollonBaseVisitor<Literal>
    {
        private readonly AtomVisitor atomVisitor = new();

        /// <summary>
        /// Generates a new <see cref="Literal"/>.
        /// </summary>
        /// <param name="context">The context of the new literal.</param>
        /// <returns>The new literal.</returns>
        public override Literal VisitLiteral(apollonParser.LiteralContext context)
        {
            Apollon.Lib.Atoms.Atom atom = this.atomVisitor.VisitAtom(context.atom());

            return new Literal(atom, false, context.NEGATION() != null);
        }
    }
}
