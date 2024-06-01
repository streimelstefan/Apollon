//-----------------------------------------------------------------------
// <copyright file="AtomParamVisitor.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace AppollonParser.Visitors
{
    using Apollon.Lib;
    using Apollon.Lib.Atoms;

    /// <summary>
    /// A visitor to create <see cref="AtomParam"/>s.
    /// </summary>
    public class AtomParamVisitor : apollonBaseVisitor<AtomParam>
    {
        private static readonly LiteralVisitor LiteralVisitor = new();

        /// <summary>
        /// Generates a new <see cref="AtomParam"/>.
        /// </summary>
        /// <param name="context">The context of the new atom param.</param>
        /// <returns>The new atom param.</returns>
        /// <exception cref="InvalidProgramException">Is called if the param results in a state that is not jet covered.</exception>
        public override AtomParam VisitAtom_param_part(apollonParser.Atom_param_partContext context)
        {
            if (context.general_term() != null)
            {
                apollonParser.General_termContext term = context.general_term();
                if (term.VARIABLE_TERM() != null)
                {
                    return new AtomParam(null, new Term(term.VARIABLE_TERM().GetText()));
                }
                else if (term.CLASICAL_TERM() != null)
                {
                    return new AtomParam(null, new Term(term.CLASICAL_TERM().GetText()));
                }
            }
            else if (context.literal() != null)
            {
                Literal literal = LiteralVisitor.VisitLiteral(context.literal());
                return new AtomParam(literal, null);
            }
            else if (context.NUMBER() != null)
            {
                return new AtomParam(null, new Term(context.NUMBER().GetText()));
            }

            throw new InvalidProgramException("Atom param was neither a general term or an atom. This is an invalid state.");
        }
    }
}
