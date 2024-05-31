using Apollon.Lib;
using Apollon.Lib.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppollonParser.Visitors
{
    /// <summary>
    /// A visitor to create <see cref="AtomParam"/>s.
    /// </summary>
    public class AtomParamVisitor : apollonBaseVisitor<AtomParam>
    {
        private static readonly LiteralVisitor _literalVisitor = new LiteralVisitor();

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
                var term = context.general_term();
                if (term.VARIABLE_TERM() != null)
                {
                    return new AtomParam(null, new Term(term.VARIABLE_TERM().GetText()));
                }
                else if (term.CLASICAL_TERM() != null)
                {
                    return new AtomParam(null, new Term(term.CLASICAL_TERM().GetText()));
                }
            } else if (context.literal() != null)
            {
                var literal = _literalVisitor.VisitLiteral(context.literal());
                return new AtomParam(literal, null);
            } else if (context.NUMBER() != null)
            {
                return new AtomParam(null, new Term(context.NUMBER().GetText()));
            }
            throw new InvalidProgramException("Atom param was neither a general term or an atom. This is an invalid state.");
        }
    }
}
