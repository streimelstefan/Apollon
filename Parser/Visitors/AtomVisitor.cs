using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib;

namespace AppollonParser.Visitors
{
    internal class AtomVisitor : apollonBaseVisitor<Atom>
    {

        public override Atom VisitAtom(apollonParser.AtomContext context)
        {
            var head = context.CLASICAL_TERM().GetText();
            var termList = new List<Term>();

            foreach (var term in context.general_term())
            {
                if (term.VARIABLE_TERM() != null)
                {
                    termList.Add(new Term(term.VARIABLE_TERM().GetText()));
                }
                else if (term.CLASICAL_TERM() != null)
                {
                    termList.Add(new Term(term.CLASICAL_TERM().GetText()));
                }
            }

            return new Atom(head, termList.ToArray());
        }

    }
}
