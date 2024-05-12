using Apollon.Lib.Atoms;
using Apollon.Lib.Rules;
using Apollon.Lib.Rules.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Linker
{
    public class VariableLinker : IVariableLinker
    {

        public Statement LinkVariables(Statement statement)
        {
            var variableTable = new Dictionary<string, Term>();
            if (statement.Head != null)
            {
                LinkInAtom(statement.Head.Atom, variableTable);
            }
            foreach (var bodyPart in statement.Body)
            {
                LinkInBodyPart(bodyPart, variableTable);
            }

            return statement;
        }

        private void LinkInBodyPart(BodyPart bodyPart, Dictionary<string, Term> variableTable)
        {
            if (bodyPart.Literal != null)
            {
                LinkInAtom(bodyPart.Literal.Atom, variableTable);
            }
            if (bodyPart.ForAll != null)
            {
                bodyPart.ForAll = ReplaceTermIfNeeded(bodyPart.ForAll, variableTable);
            }
            if (bodyPart.Child != null)
            {
                LinkInBodyPart(bodyPart.Child, variableTable);
            }
            if (bodyPart.Operation != null)
            {
                LinkInOperation(bodyPart.Operation, variableTable);
            }
        }

        private void LinkInAtom(Atom atom, Dictionary<string, Term> variableTable)
        {
            foreach (var param in atom.ParamList)
            {
                LinkInAtomParam(param, variableTable);
            }
        }

        private void LinkInAtomParam(AtomParam atomParam, Dictionary<string, Term> variableTable)
        {
            if (atomParam.Term != null)
            {
                atomParam.Term = ReplaceTermIfNeeded(atomParam.Term, variableTable);
            }
            if (atomParam.Literal != null)
            {
                LinkInAtom(atomParam.Literal.Atom, variableTable);
            }
        }

        private void LinkInOperation(Operation operation, Dictionary<string, Term> variableTable)
        {
            LinkInAtomParam(operation.Variable, variableTable);
            LinkInAtom(operation.Condition.Atom, variableTable);
        }

        private Term ReplaceTermIfNeeded(Term term, Dictionary<string, Term> variableTable)
        {
            if (!term.IsVariable)
            {
                return term;
            }

            if (variableTable.ContainsKey(term.Value))
            {
                return variableTable[term.Value];
            }

            variableTable[term.Value] = term;
            return term;
        }
    }
}
