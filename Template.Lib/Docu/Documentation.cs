//-----------------------------------------------------------------------
// <copyright file="Documentation.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Docu
{
    using System.Text;
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// A piece within the program that documents a fact or a statement.
    /// </summary>
    public class Documentation : IDocumentation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Documentation"/> class.
        /// </summary>
        /// <param name="dokuParts">The parts of the dukumentation.</param>
        /// <param name="literal">The literal that is being used within the documentation.</param>
        public Documentation(DokuPart[] dokuParts, Literal literal)
        {
            this.DokuParts = dokuParts;
            this.Literal = literal;
        }

        /// <summary>
        /// Gets the parts of the documentation.
        /// </summary>
        public DokuPart[] DokuParts { get; private set; }

        /// <summary>
        /// Gets the literal that is being used in this documentation.
        /// </summary>
        public Literal Literal { get; private set; }

        /// <summary>
        /// Generates the documentation for a given <see cref="Substitution"/>.
        /// </summary>
        /// <param name="sub">The substition that holds the mappings for variables used wihtin the litera.</param>
        /// <returns>The <see cref="StringBuilder"/> that holds the dokumentation of the literal.</returns>
        public StringBuilder GetDokuFor(Substitution sub)
        {
            StringBuilder stringBuilder = new();

            for (int i = 0; i < this.DokuParts.Length - 1; i++)
            {
                DokuPart part = this.DokuParts[i];
                if (part.DocuPart != null)
                {
                    _ = stringBuilder.Append(part.DocuPart);
                }
                else if (part.VariablePlaceholder != null)
                {
                    Literal tmpLiteral = new(new Atom("tmp", new AtomParam(part.VariablePlaceholder)), false, false);
                    Literal subbed = sub.Apply(tmpLiteral);
                    _ = stringBuilder.Append(subbed.Atom.ParamList[0].ToString());
                }

                _ = stringBuilder.Append(" ");
            }

            DokuPart lastPart = this.DokuParts.Last();
            if (lastPart.DocuPart != null)
            {
                _ = stringBuilder.Append(lastPart.DocuPart);
            }
            else if (lastPart.VariablePlaceholder != null)
            {
                Literal tmpLiteral = new(new Atom("tmp", new AtomParam(lastPart.VariablePlaceholder)), false, false);
                Literal subbed = sub.Apply(tmpLiteral);
                _ = stringBuilder.Append(subbed.Atom.ParamList[0].ToString());
            }

            return stringBuilder;
        }
    }
}
