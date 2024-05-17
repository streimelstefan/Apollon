using Apollon.Lib.Atoms;
using Apollon.Lib.Graph;
using Apollon.Lib.Rules;
using Apollon.Lib.Rules.Operations;
using Apollon.Lib.Unification;
using Apollon.Lib.Unification.Substitutioners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Docu
{
    public class DocumentationGenerator : IDocumentationGenerator
    {
        public IEqualizer<Literal> _equalizer = new LiteralParamCountEqualizer();
        public IUnifier _unifier = new Unifier();

        public string GenerateDokumentationFor(Program program)
        {
            var statements = program.Statements.Where(s => s.Head != null);
            var stringBuilder = new StringBuilder();

            foreach (var statement in statements)
            {
                var doku = GenerateDokumentationFor(statement, program.Documentation);
                stringBuilder.Append(doku);
            }

            return stringBuilder.ToString();
        }

        private StringBuilder GenerateDokumentationFor(Statement statement, IDocumentation[] documentations)
        {
            var stringBuilder = new StringBuilder();
            var headDoku = GetInsertedDokuStringFor(statement.Head, documentations, true);

            if (headDoku.Length == 0) return stringBuilder;

            stringBuilder.Append(headDoku);

            if (statement.Body.Length > 0)
            {
                stringBuilder.AppendLine(" if");
                stringBuilder.Append("  ");
                var firstConditionDoku = GetInsertedDokuStringFor(statement.Body[0], documentations, statement.Head);
                stringBuilder.Append(firstConditionDoku);

                for (int i = 1; i < statement.Body.Length; i++)
                {
                    var bodyPart = statement.Body[i];
                    stringBuilder.AppendLine(", and");
                    stringBuilder.Append("  ");
                    stringBuilder.Append(GetInsertedDokuStringFor(bodyPart, documentations, statement.Head));
                }
            }
            stringBuilder.AppendLine(".");
            return stringBuilder;
        }

        private StringBuilder GetInsertedDokuStringFor(BodyPart bodyPart, IDocumentation[] documentations, Literal head)
        {
            if (bodyPart.Literal != null)
            {
                return GetInsertedDokuStringFor(bodyPart.Literal, documentations);
            } 
            return GetInsertedDokuStringFor(bodyPart.Operation, documentations, head);
        }
        private StringBuilder GetInsertedDokuStringFor(Operation operation, IDocumentation[] documentations, Literal head)
        {
            var documentation = GetMatchingDokumentationFor(head, documentations);
            if (documentation.Value == null)
            {
                return new StringBuilder();
            }

            var sub = _unifier.Unify(documentation.Value.Literal, head).Value;
            var subbed = sub.Apply(operation);
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(subbed.Variable.ToString());
            if (operation.Operator == Operator.Equals)
            {
                stringBuilder.Append(" is ");
            } else if (operation.Operator == Operator.NotEquals)
            {
                stringBuilder.Append(" is not ");
            }
            stringBuilder.Append(subbed.Condition.ToString());

            return stringBuilder;
        }

        private StringBuilder GetInsertedDokuStringFor(Literal literal, IDocumentation[] documentations, bool isInHead = false)
        {
            var litCopy = (Literal)literal.Clone();
            var stringBuilder = new StringBuilder();
            if (litCopy.IsNAF)
            {
                litCopy.IsNAF = false;
                stringBuilder.Append("there is no evidence that ");
            }

            var documentation = GetMatchingDokumentationFor(litCopy, documentations);
            if (documentation.Value == null)
            {
                if (isInHead) return stringBuilder;

                stringBuilder.Append(literal.ToString());
                stringBuilder.Append(" holds");
                return stringBuilder;
            }

            var uniRes = _unifier.Unify(documentation.Value.Literal, litCopy);
            if (uniRes.Value == null)
            {
                return stringBuilder;
            }
            stringBuilder.Append(documentation.Value.GetDokuFor(uniRes.Value));
            return stringBuilder;
        }

        private Maybe<IDocumentation, bool> GetMatchingDokumentationFor(Literal literal, IDocumentation[] documentations)
        {
            var documentation = documentations.Where(d => _equalizer.AreEqual(d.Literal, literal));

            if (documentation.Any())
            {
                return new Maybe<IDocumentation, bool>(documentation.Last());
            } else
            {
                return new Maybe<IDocumentation, bool>(false);
            }
        }
    }
}
