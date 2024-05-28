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
    /// <summary>
    /// A piece within the program that documents a fact or a statement.
    /// </summary>
    public class Documentation : IDocumentation
    {
        /// <summary>
        /// Gets the parts of the documentation.
        /// </summary>
        public DokuPart[] DokuParts { get; private set; }

        /// <summary>
        /// Gets the literal that is being used in this documentation.
        /// </summary>
        public Literal Literal { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Documentation"/> class.
        /// </summary>
        /// <param name="dokuParts">The parts of the dukumentation.</param>
        /// <param name="literal">The literal that is being used within the documentation.</param>
        public Documentation(DokuPart[] dokuParts, Literal literal)
        {
            DokuParts = dokuParts;
            Literal = literal;
        }

        /// <summary>
        /// Generates the documentation for a given <see cref="Substitution"/>.
        /// </summary>
        /// <param name="sub">The substition that holds the mappings for variables used wihtin the litera.</param>
        /// <returns>The <see cref="StringBuilder"/> that holds the dokumentation of the literal.</returns>
        public StringBuilder GetDokuFor(Substitution sub)
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
