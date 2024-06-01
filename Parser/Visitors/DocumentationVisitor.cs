//-----------------------------------------------------------------------
// <copyright file="DocumentationVisitor.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace AppollonParser.Visitors
{
    using Apollon.Lib;
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Docu;

    /// <summary>
    /// A visitor that generates <see cref="Documentation"/>s.
    /// </summary>
    public class DocumentationVisitor : apollonBaseVisitor<IDocumentation>
    {
        /// <summary>
        /// Generates a new <see cref="IDocumentation"/>.
        /// </summary>
        /// <param name="context">The context of the documentation.</param>
        /// <returns>The new documentation.</returns>
        /// <exception cref="ParseException">Is thrown if a documentation part conatins a variable that is not used in a literal.</exception>
        public override IDocumentation VisitDocu(apollonParser.DocuContext context)
        {
            string literalHead = context.docu_head().CLASICAL_TERM().GetText();
            IEnumerable<AtomParam> variables = context.docu_head().VARIABLE_TERM().Select(v => new Term(v.GetText())).Select(t => new AtomParam(t));
            Atom atom = new(literalHead, variables.ToArray());
            Literal literal = new(atom, false, context.docu_head().NEGATION() != null);

            DocumentationBuilder docuBuilder = new(literal);
            foreach (apollonParser.Docu_string_partContext? placeholder in context.docu_string().docu_string_part())
            {
                if (placeholder.variable_placeholder() != null)
                {
                    try
                    {
                        docuBuilder.AddPlaceholder(placeholder.variable_placeholder().VARIABLE_TERM().GetText());
                    }
                    catch (InvalidOperationException e)
                    {
                        throw new ParseException(e.Message);
                    }
                }
                else if (placeholder.docu_string_string_part() != null)
                {
                    docuBuilder.AddDokuPart(placeholder.docu_string_string_part().GetText());
                }
            }

            try
            {
                return docuBuilder.Build();
            }
            catch (InvalidDataException e)
            {
                throw new ParseException(e.Message);
            }
        }
    }
}
