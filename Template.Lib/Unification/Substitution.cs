using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using Apollon.Lib.Rules.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Unification
{
    public class Substitution : ISubstitution
    {
        private Dictionary<string, AtomParam> mappings = new Dictionary<string, AtomParam>();

        public void Add(Term variable, AtomParam term)
        {
            mappings[variable.Value] = term;
        }

        public Statement Apply(Statement statement)
        {
            var copy = (Statement)statement.Clone();

            if (copy.Head != null)
            {
                Apply(copy.Head.Atom);
            }

            foreach (var part in copy.Body)
            {
                Apply(part);
            }

            return copy;
        }

        private AtomParam Apply(AtomParam param)
        {
            if (param.Term != null)
            {
                // no need to do anything. only looking for variables
                if (!param.Term.IsVariable)
                {
                    return param;
                }

                // no mapping for this variable available.
                if (!mappings.ContainsKey(param.Term.Value))
                {
                    return param;
                }

                return mappings[param.Term.Value];
            }

            if (param.Literal != null)
            {
                Apply(param.Literal.Atom);
            }

            return param;
        }

        private void Apply(Atom atom)
        {
            for (int i = 0; i < atom.ParamList.Count(); i++)
            {
                var param = atom.ParamList[i];
                atom.ParamList[i] = Apply(param);
            }
        }

        private void Apply(BodyPart part)
        {
            if (part.Literal != null)
            {
                Apply(part.Literal.Atom);
            }
            if (part.Child != null)
            {
                Apply(part.Child);
            }
            if (part.Operation != null)
            {
                Apply(part.Operation);
            }
        }

        private void Apply(Operation operation)
        {
            if (operation.Variable.Term != null)
            {
                if (mappings.ContainsKey(operation.Variable.Term.Value))
                {
                    operation.Variable = mappings[operation.Variable.Term.Value];
                }
            }
            if (operation.Variable.Literal != null)
            {
                Apply(operation.Variable.Literal.Atom);
            }
        }


        public override string ToString()
        {
            return $"{{ {string.Join(", ", mappings.Select(kv => $"{kv.Key} -> {kv.Value}"))} }}";
        }
    }
}
