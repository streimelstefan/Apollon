//-----------------------------------------------------------------------
// <copyright file="VariableExtractor.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Resolution
{
    using Apollon.Lib.Atoms;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Rules.Operations;

    /// <summary>
    /// Extracts all Variables from a Statement/BodyParts/Literal.
    /// </summary>
    public class VariableExtractor
    {
        /// <summary>
        /// Extracts all Variables from a Statement.
        /// </summary>
        /// <param name="statement">The statement of which the variables shall be extracted.</param>
        /// <returns>The extracted variables.</returns>
        public HashSet<Term> ExtractVariablesFrom(Statement statement)
        {
            HashSet<Term> variables = this.ExtractVariablesFrom(statement.Body);

            if (statement.Head != null)
            {
                this.ExtractVariablesFrom(statement.Head, variables);
            }

            return variables;
        }

        public HashSet<Term> ExtractAllForAlls(BodyPart forall)
        {
            var hashSet = new HashSet<Term>();
            ExtractAllForAllsRec(forall, hashSet);

            return hashSet;
        }

        private void ExtractAllForAllsRec(BodyPart forall, HashSet<Term> vairables)
        {
            if (forall.ForAll == null) return;

            vairables.Add(forall.ForAll);

            if (forall.Child != null) ExtractAllForAllsRec(forall.Child, vairables);
        }

        /// <summary>
        /// Extracts all Variables from a BodyPart.
        /// </summary>
        /// <param name="bodyPart">The body part of which the variables shall be extracted.</param>
        /// <returns>The extracted variables.</returns>
        public HashSet<Term> ExtractVariablesFrom(BodyPart[] bodyPart)
        {
            HashSet<Term> variables = new();

            foreach (BodyPart part in bodyPart)
            {
                this.ExtractVariablesFrom(part, variables);
            }

            return variables;
        }

        /// <summary>
        /// Extracts all Variables from a Literal.
        /// </summary>
        /// <param name="literal">The literal of which the variables shall be extracted.</param>
        /// <returns>The extracted variables.</returns>
        public HashSet<Term> ExtractVariablesFrom(Literal literal)
        {
            HashSet<Term> variables = new();

            this.ExtractVariablesFrom(literal, variables);

            return variables;
        }

        private void ExtractVariablesFrom(BodyPart bodyPart, HashSet<Term> variables)
        {
            if (bodyPart.Literal != null)
            {
                this.ExtractVariablesFrom(bodyPart.Literal, variables);
            }
            if (bodyPart.Operation != null)
            {
                this.ExtractVariablesFrom(bodyPart.Operation, variables);
            }
            if (bodyPart.ForAll != null)
            {
                variables.Add(bodyPart.ForAll);
            }
            if (bodyPart.Child != null)
            {
                this.ExtractVariablesFrom(bodyPart.Child, variables);
            }
        }

        private void ExtractVariablesFrom(Literal literal, HashSet<Term> variables)
        {
            this.ExtractVariablesFrom(literal.Atom, variables);
        }

        private void ExtractVariablesFrom(Atom atom, HashSet<Term> variables)
        {
            foreach (AtomParam param in atom.ParamList)
            {
                this.ExtractVariablesFrom(param, variables);
            }
        }

        private void ExtractVariablesFrom(AtomParam param, HashSet<Term> variables)
        {
            if (param.Literal != null)
            {
                this.ExtractVariablesFrom(param.Literal, variables);
            }
            else if (param.Term != null)
            {
                if (param.Term.IsVariable)
                {
                    _ = variables.Add(param.Term);
                }
            }
        }

        private void ExtractVariablesFrom(Operation operation, HashSet<Term> variables)
        {
            if (operation.OutputtingVariable != null)
            {
                _ = variables.Add(operation.OutputtingVariable);
            }

            if (operation.Variable.Term != null && operation.Variable.Term.IsVariable)
            {
                _ = variables.Add(operation.Variable.Term);
            }

            this.ExtractVariablesFrom(operation.Condition, variables);
        }
    }
}
