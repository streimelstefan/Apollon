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
            var variables = this.ExtractVariablesFrom(statement.Body);

            if (statement.Head != null)
            {
                this.ExtractVariablesFrom(statement.Head, variables);
            }

            return variables;
        }

        /// <summary>
        /// Extracts all Variables from a BodyPart.
        /// </summary>
        /// <param name="bodyPart">The body part of which the variables shall be extracted.</param>
        /// <returns>The extracted variables.</returns>
        public HashSet<Term> ExtractVariablesFrom(BodyPart[] bodyPart)
        {
            var variables = new HashSet<Term>();

            foreach (var part in bodyPart)
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
            var variables = new HashSet<Term>();

            this.ExtractVariablesFrom(literal, variables);

            return variables;
        }

        private void ExtractVariablesFrom(BodyPart bodyPart, HashSet<Term> variables)
        {
            if (bodyPart.Literal != null)
            {
                this.ExtractVariablesFrom(bodyPart.Literal, variables);
            }
            else if (bodyPart.Operation != null)
            {
                this.ExtractVariablesFrom(bodyPart.Operation, variables);
            }
        }

        private void ExtractVariablesFrom(Literal literal, HashSet<Term> variables)
        {
            this.ExtractVariablesFrom(literal.Atom, variables);
        }

        private void ExtractVariablesFrom(Atom atom, HashSet<Term> variables)
        {
            foreach (var param in atom.ParamList)
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
                    variables.Add(param.Term);
                }
            }
        }

        private void ExtractVariablesFrom(Operation operation, HashSet<Term> variables)
        {
            if (operation.OutputtingVariable != null)
            {
                variables.Add(operation.OutputtingVariable);
            }

            if (operation.Variable.Term != null && operation.Variable.Term.IsVariable)
            {
                variables.Add(operation.Variable.Term);
            }

            this.ExtractVariablesFrom(operation.Condition, variables);
        }
    }
}
