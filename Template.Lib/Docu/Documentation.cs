using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Docu
{
    public class Documentation : IDocumentation
    {
        public DokuPart[] DokuParts { get; private set; }

        public Literal Literal { get; private set; }

        public Documentation(DokuPart[] dokuParts, Literal literal)
        {
            DokuParts = dokuParts;
            Literal = literal;
        }

        public StringBuilder GetDokuFor(ISubstitution sub)
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < DokuParts.Length - 1; i++)
            {
                var part = DokuParts[i];
                if (part.DocuPart != null)
                {
                    stringBuilder.Append(part.DocuPart);
                } else
                {
                    var tmpLiteral = new Literal(new Atom("tmp", new AtomParam(part.VariablePlaceholder)), false, false);
                    var subbed = sub.Apply(tmpLiteral);
                    stringBuilder.Append(subbed.Atom.ParamList[0].ToString());
                }

                stringBuilder.Append(" ");
            }

            var lastPart = DokuParts.Last();
            if (lastPart.DocuPart != null)
            {
                stringBuilder.Append(lastPart.DocuPart);
            }
            else
            {
                var tmpLiteral = new Literal(new Atom("tmp", new AtomParam(lastPart.VariablePlaceholder)), false, false);
                var subbed = sub.Apply(tmpLiteral);
                stringBuilder.Append(subbed.Atom.ParamList[0].ToString());
            }

            return stringBuilder;
        }
    }
}
