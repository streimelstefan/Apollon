using Antlr4.Runtime.Misc;
using Apollon.Lib;
using Apollon.Lib.Atoms;
using Apollon.Lib.Docu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppollonParser.Visitors
{
    public class DocumentationVisitor : apollonBaseVisitor<IDocumentation>
    {

        public override IDocumentation VisitDocu(apollonParser.DocuContext context)
        {
            var literalHead = context.docu_head().CLASICAL_TERM().GetText();
            var variables = context.docu_head().VARIABLE_TERM().Select(v => new Term(v.GetText())).Select(t => new AtomParam(t));
            var atom = new Atom(literalHead, variables.ToArray());
            var literal = new Literal(atom, false, context.docu_head().NEGATION() != null);

            var docuBuilder = new DocumentationBuilder(literal);
            foreach (var placeholder in context.docu_string().docu_string_part())
            {
                if (placeholder.variable_placeholder() != null)
                {
                    try
                    {
                        docuBuilder.AddPlaceholder(placeholder.variable_placeholder().VARIABLE_TERM().GetText());
                    } catch (InvalidOperationException e)
                    {
                        throw new ParseException(e.Message);
                    }
                } else if (placeholder.general_term() != null)
                {
                    docuBuilder.AddDokuPart(placeholder.general_term().GetText());
                }
            }

            try
            {
                return docuBuilder.Build();
            } catch(InvalidDataException e)
            {
                throw new ParseException(e.Message);
            }
        }


    }
}
