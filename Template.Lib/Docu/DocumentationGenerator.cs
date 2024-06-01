//-----------------------------------------------------------------------
// <copyright file="DocumentationGenerator.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace Apollon.Lib.Docu
{
    using System.Text;
    using Apollon.Lib.Graph;
    using Apollon.Lib.Rules;
    using Apollon.Lib.Rules.Operations;
    using Apollon.Lib.Unification;
    using Apollon.Lib.Unification.Substitutioners;

    /// <summary>
    /// Generates documentation for a given program.
    /// </summary>
    public class DocumentationGenerator : IDocumentationGenerator
    {
        private readonly IEqualizer<Literal> equalizer = new LiteralParamCountEqualizer();
        private readonly IUnifier unifier = new Unifier();

        /// <summary>
        /// Generates the documentation for a given program.
        /// </summary>
        /// <param name="program">The program for which the dokumentation should be build.</param>
        /// <returns>The string representing the documentation of the program.</returns>
        public string GenerateDokumentationFor(Program program)
        {
            IEnumerable<Statement> statements = program.Statements.Where(s => s.Head != null);
            StringBuilder stringBuilder = new();

            foreach (Statement? statement in statements)
            {
                StringBuilder doku = this.GenerateDokumentationFor(statement, program.Documentation);
                _ = stringBuilder.Append(doku);
            }

            return stringBuilder.ToString();
        }

        private StringBuilder GenerateDokumentationFor(Statement statement, IDocumentation[] documentations)
        {
            if (statement.Head == null)
            {
                return new StringBuilder();
            }

            StringBuilder stringBuilder = new();
            StringBuilder headDoku = this.GetInsertedDokuStringFor(statement.Head, documentations, true);

            if (headDoku.Length == 0)
            {
                return stringBuilder;
            }

            _ = stringBuilder.Append(headDoku);

            if (statement.Body.Length > 0)
            {
                _ = stringBuilder.AppendLine(" if");
                _ = stringBuilder.Append("  ");
                StringBuilder firstConditionDoku = this.GetInsertedDokuStringFor(statement.Body[0], documentations, statement.Head);
                _ = stringBuilder.Append(firstConditionDoku);

                for (int i = 1; i < statement.Body.Length; i++)
                {
                    BodyPart bodyPart = statement.Body[i];
                    _ = stringBuilder.AppendLine(", and");
                    _ = stringBuilder.Append("  ");
                    _ = stringBuilder.Append(this.GetInsertedDokuStringFor(bodyPart, documentations, statement.Head));
                }
            }

            _ = stringBuilder.AppendLine(".");
            return stringBuilder;
        }

        private StringBuilder GetInsertedDokuStringFor(BodyPart bodyPart, IDocumentation[] documentations, Literal head)
        {
            return bodyPart.Literal != null
                ? this.GetInsertedDokuStringFor(bodyPart.Literal, documentations)
                : this.GetInsertedDokuStringFor(bodyPart.Operation!, documentations, head);
        }

        private StringBuilder GetInsertedDokuStringFor(Operation operation, IDocumentation[] documentations, Literal head)
        {
            Maybe<IDocumentation, bool> documentation = this.GetMatchingDokumentationFor(head, documentations);
            if (documentation.Value == null)
            {
                return new StringBuilder();
            }

            Substitution? sub = this.unifier.Unify(documentation.Value.Literal, head).Value;
            Operation subbed = sub!.Apply(operation);
            StringBuilder stringBuilder = new();

            if (operation.OutputtingVariable != null)
            {
                _ = stringBuilder.Append(operation.OutputtingVariable.ToString());
                _ = stringBuilder.Append(" is ");
            }

            _ = stringBuilder.Append(subbed.Variable.ToString());
            _ = stringBuilder.Append(operation.Operator.ToDocumentationString());
            _ = stringBuilder.Append(subbed.Condition.ToString());

            return stringBuilder;
        }

        private StringBuilder GetInsertedDokuStringFor(Literal literal, IDocumentation[] documentations, bool isInHead = false)
        {
            Literal litCopy = (Literal)literal.Clone();
            StringBuilder stringBuilder = new();
            if (litCopy.IsNAF)
            {
                litCopy.IsNAF = false;
                _ = stringBuilder.Append("there is no evidence that ");
            }

            if (literal.IsNAF && literal.IsNegative)
            {
                _ = stringBuilder.Append("and ");
            }

            if (litCopy.IsNegative)
            {
                litCopy.IsNegative = false;
                _ = stringBuilder.Append("it is not the case that ");
            }

            Maybe<IDocumentation, bool> documentation = this.GetMatchingDokumentationFor(litCopy, documentations);
            if (documentation.Value == null)
            {
                if (isInHead)
                {
                    return stringBuilder;
                }

                _ = stringBuilder.Append(litCopy.ToString());
                _ = stringBuilder.Append(" holds");
                return stringBuilder;
            }

            UnificationResult uniRes = this.unifier.Unify(documentation.Value.Literal, litCopy);
            if (uniRes.Value == null)
            {
                return stringBuilder;
            }

            _ = stringBuilder.Append(documentation.Value.GetDokuFor(uniRes.Value));
            return stringBuilder;
        }

        private Maybe<IDocumentation, bool> GetMatchingDokumentationFor(Literal literal, IDocumentation[] documentations)
        {
            IEnumerable<IDocumentation> documentation = documentations.Where(d => this.equalizer.AreEqual(d.Literal, literal));

            return documentation.Any() ? new Maybe<IDocumentation, bool>(documentation.Last()) : new Maybe<IDocumentation, bool>(false);
        }
    }
}
