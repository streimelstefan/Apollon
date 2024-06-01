//-----------------------------------------------------------------------
// <copyright file="DocumentationBuilder.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Docu
{
    using Apollon.Lib.Atoms;

    /// <summary>
    /// Builds a <see cref="Documentation"/> object.s.
    /// </summary>
    public class DocumentationBuilder
    {
        private readonly List<DokuPart> dokuParts = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentationBuilder"/> class.
        /// </summary>
        /// <param name="literal">The literal that is being documented.</param>
        public DocumentationBuilder(Literal literal)
        {
            this.Literal = literal;
        }

        /// <summary>
        /// Gets or sets the literal that the documentation is for.
        /// </summary>
        public Literal Literal { get; set; }

        /// <summary>
        /// Adds a new part to the documentation.
        /// </summary>
        /// <param name="docu">The string to add to the documentation.</param>
        public void AddDokuPart(string docu)
        {
            this.dokuParts.Add(new DokuPart(docu));
        }

        /// <summary>
        /// Adds a new placeholder to the documentation.
        /// </summary>
        /// <param name="variableName">The variable that should be replaced when building the documentation.</param>
        /// <exception cref="InvalidOperationException">Is thrown if the given placeholder does not exist within the literal to document.</exception>
        public void AddPlaceholder(string variableName)
        {
            Term newTerm = new(variableName);

            if (!newTerm.IsVariable)
            {
                return;
            }

            if (this.dokuParts.Where(t => t.VariablePlaceholder != null && t.VariablePlaceholder.Equals(newTerm)).Any())
            {
                // already added
                return;
            }

            if (!this.HasVariable(newTerm, this.Literal.Atom))
            {
                throw new InvalidOperationException("Unable to add placeholder that does not exist in documentation head.");
            }

            this.dokuParts.Add(new DokuPart(newTerm));
        }

        /// <summary>
        /// Builds the <see cref="Documentation"/> object.
        /// </summary>
        /// <returns>The built documentation object.</returns>
        public IDocumentation Build()
        {
            return new Documentation(this.dokuParts.ToArray(), this.Literal);
        }

        private bool HasVariable(Term variable, Atom atom)
        {
            foreach (AtomParam param in atom.ParamList)
            {
                if (param.Term != null && param.Term.Equals(variable))
                {
                    return true;
                }
                else if (param.Literal != null && this.HasVariable(variable, param.Literal.Atom))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
