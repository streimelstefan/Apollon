using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Docu
{
    public class DocumentationBuilder
    {
        public Literal Literal { get; set; }

        private List<DokuPart> _dokuParts = new List<DokuPart>();

        public DocumentationBuilder(Literal literal) 
        { 
            Literal = literal;
        }

        public void AddDokuPart(string docu)
        {
            _dokuParts.Add(new DokuPart(docu));
        }

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

        public IDocumentation Build()
        {
            return new Documentation(_dokuParts.ToArray(), Literal);
        }
    }
}
