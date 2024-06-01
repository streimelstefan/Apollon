//-----------------------------------------------------------------------
// <copyright file="AtomVisitor.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace AppollonParser.Visitors
{
    using Apollon.Lib.Atoms;

    /// <summary>
    /// A visitor that creates <see cref="Atom"/>s.
    /// </summary>
    internal class AtomVisitor : apollonBaseVisitor<Atom>
    {
        private static readonly AtomParamVisitor ParamVisitor = new();

        /// <summary>
        /// Creates a new <see cref="Atom"/> from the given context.
        /// </summary>
        /// <param name="context">The atom context..</param>
        /// <returns>The new atom.</returns>
        public override Atom VisitAtom(apollonParser.AtomContext context)
        {
            string head = context.CLASICAL_TERM().GetText();
            List<AtomParam> paramList = new();

            foreach (apollonParser.Atom_param_partContext? param in context.atom_param_part())
            {
                paramList.Add(ParamVisitor.VisitAtom_param_part(param));
            }

            return new Atom(head, paramList.ToArray());
        }
    }
}
