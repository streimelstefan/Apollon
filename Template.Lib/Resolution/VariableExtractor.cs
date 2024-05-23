using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using Apollon.Lib.Rules.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Resolution
{
    public class VariableExtractor
    {

        public HashSet<Term> ExtractVariablesFrom(Statement statement)
        {
            var variables = ExtractVariablesFrom(statement.Body);

            if (statement.Head != null)
            {
                ExtractVariablesFrom(statement.Head, variables);
            }

            return variables;
        }

        public HashSet<Term> ExtractVariablesFrom(BodyPart[] bodyPart)
        {
            var variables = new HashSet<Term>();

            foreach (var part in bodyPart) 
            {
                ExtractVariablesFrom(part, variables);
            }

            return variables;
        }

        public HashSet<Term> ExtractVariablesFrom(Literal literal)
        {
            var variables = new HashSet<Term>();

            ExtractVariablesFrom(literal, variables);

            return variables;
        }

        private void ExtractVariablesFrom(BodyPart bodyPart, HashSet<Term> variables)
        {
            if (bodyPart.Literal != null)
            {
                ExtractVariablesFrom(bodyPart.Literal, variables);
            } else if (bodyPart.Operation != null)
            {
                ExtractVariablesFrom(bodyPart.Operation, variables);
            }
        }

        private void ExtractVariablesFrom(Literal literal, HashSet<Term> variables) 
        {
            ExtractVariablesFrom(literal.Atom, variables);
        }

        private void ExtractVariablesFrom(Atom atom, HashSet<Term> variables)
        {
            foreach(var param in atom.ParamList)
            {
                ExtractVariablesFrom(param, variables);
            }
        }

        private void ExtractVariablesFrom(AtomParam param, HashSet<Term> variables)
        {
            if (param.Literal != null)
            {
                ExtractVariablesFrom(param.Literal, variables);
            } else if (param.Term != null)
            {
                if (param.Term.IsVariable)
                {
                    variables.Add(param.Term);
                }
            }
        }
        private void ExtractVariablesFrom(Operation operation, HashSet<Term> variables) 
        { 
            if (operation.Variable.Term != null && operation.Variable.Term.IsVariable)
            {
                variables.Add(operation.Variable.Term);
            }
            ExtractVariablesFrom(operation.Condition, variables);
        }

    }
}
