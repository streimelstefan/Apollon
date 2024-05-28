using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Docu
{

    /// <summary>
    /// Builds a <see cref="Documentation"/> object.s
    /// </summary>
    public class DocumentationBuilder
    {
        /// <summary>
        /// Gets or sets the literal that the documentation is for.
        /// </summary>
        public Literal Literal { get; set; }


        private List<DokuPart> _dokuParts = new List<DokuPart>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentationBuilder"/> class.
        /// </summary>
        /// <param name="literal">The literal that is being documented.</param>
        public DocumentationBuilder(Literal literal) 
        { 
            Literal = literal;
        }

        /// <summary>
        /// Adds a new part to the documentation.
        /// </summary>
        /// <param name="docu">The string to add to the documentation.</param>
        public void AddDokuPart(string docu)
        {
            _dokuParts.Add(new DokuPart(docu));
        }

        /// <summary>
        /// Adds a new placeholder to the documentation.
        /// </summary>
        /// <param name="variableName">The variable that should be replaced when building the documentation</param>
        /// <exception cref="InvalidOperationException">Is thrown if the given placeholder does not exist within the literal to document.</exception>
        public void AddPlaceholder(string variableName)
        {
            var newTerm = new Term(variableName);

            if (!newTerm.IsVariable)
            {
                return;
            }
            if (_dokuParts.Where(t => t.VariablePlaceholder != null && t.VariablePlaceholder.Equals(newTerm)).Any())
            {
                // already added
                return;
            }
            if (!HasVariable(newTerm, Literal.Atom))
            {
                throw new InvalidOperationException("Unable to add placeholder that does not exist in documentation head.");
            }

            _dokuParts.Add(new DokuPart(newTerm));
        }

        private bool HasVariable(Term variable, Atom atom)
        {
            foreach (var param in atom.ParamList)
            {
                if (param.Term != null && param.Term.Equals(variable))
                {
                    return true;
                } else if (param.Literal != null && HasVariable(variable, param.Literal.Atom))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Builds the <see cref="Documentation"/> object.
        /// </summary>
        /// <returns>The built documentation object.</returns>
        public IDocumentation Build()
        {
            return new Documentation(_dokuParts.ToArray(), Literal);
        }
    }
}
