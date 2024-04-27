using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib.Atoms;
using Apollon.Lib;

namespace AppollonParser.Visitors
{
    internal class AtomVisitor : apollonBaseVisitor<Atom>
    {
        private static readonly AtomParamVisitor _paramVisitor = new AtomParamVisitor();

        public override Atom VisitAtom(apollonParser.AtomContext context)
        {
            var head = context.CLASICAL_TERM().GetText();
            var paramList = new List<AtomParam>();

            foreach (var param in context.atom_param_part())
            {
                paramList.Add(_paramVisitor.VisitAtom_param_part(param));
            }

            return new Atom(head, paramList.ToArray());
        }

    }
}
