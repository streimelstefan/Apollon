//-----------------------------------------------------------------------
// <copyright file="VariableLinker.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Linker
{
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Rules.Operations;

    /// <summary>
    /// A linker that links all the variables in a statement.
    /// </summary>
    public class VariableLinker : IVariableLinker
    {
        /// <summary>
        /// Links all the variables in a given statement. After linking all the variables with the same name will be the same object.
        /// </summary>
        /// <param name="statement">The statement to link the variables for.</param>
        /// <returns>The new statment where the variables are linked.</returns>
        public Statement LinkVariables(Statement statement)
        {
            Dictionary<string, Term> variableTable = new();
            if (statement.Head != null)
            {
                this.LinkInAtom(statement.Head.Atom, variableTable);
            }

            foreach (BodyPart bodyPart in statement.Body)
            {
                this.LinkInBodyPart(bodyPart, variableTable);
            }

            return statement;
        }

        private void LinkInBodyPart(BodyPart bodyPart, Dictionary<string, Term> variableTable)
        {
            if (bodyPart.Literal != null)
            {
                this.LinkInAtom(bodyPart.Literal.Atom, variableTable);
            }

            if (bodyPart.ForAll != null)
            {
                bodyPart.ForAll = this.ReplaceTermIfNeeded(bodyPart.ForAll, variableTable);
            }

            if (bodyPart.Child != null)
            {
                this.LinkInBodyPart(bodyPart.Child, variableTable);
            }

            if (bodyPart.Operation != null)
            {
                this.LinkInOperation(bodyPart.Operation, variableTable);
            }
        }

        private void LinkInAtom(Atom atom, Dictionary<string, Term> variableTable)
        {
            foreach (AtomParam param in atom.ParamList)
            {
                this.LinkInAtomParam(param, variableTable);
            }
        }

        private void LinkInAtomParam(AtomParam atomParam, Dictionary<string, Term> variableTable)
        {
            if (atomParam.Term != null)
            {
                atomParam.Term = this.ReplaceTermIfNeeded(atomParam.Term, variableTable);
            }

            if (atomParam.Literal != null)
            {
                this.LinkInAtom(atomParam.Literal.Atom, variableTable);
            }
        }

        private void LinkInOperation(Operation operation, Dictionary<string, Term> variableTable)
        {
            if (operation.OutputtingVariable != null)
            {
                operation.OutputtingVariable = this.ReplaceTermIfNeeded(operation.OutputtingVariable, variableTable);
            }

            this.LinkInAtomParam(operation.Variable, variableTable);
            this.LinkInAtomParam(operation.Condition, variableTable);
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
