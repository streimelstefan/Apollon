using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib;
using Apollon.Lib.Docu;

namespace AppollonParser.Visitors
{
    /// <summary>
    /// A vistor that generates <see cref="Program"/>s.
    /// </summary>
    internal class ProgramVisitor : apollonBaseVisitor<Program>
    {
        private readonly RuleVisitor _ruleVisitor = new RuleVisitor();
        private readonly FactVisitor _factVisitor = new FactVisitor();
        private readonly ConstraintVisitor _constraintVisitor = new ConstraintVisitor();
        private readonly DocumentationVisitor _documentationVisitor = new DocumentationVisitor();

        /// <summary>
        /// Generates a new <see cref="Program"/>.
        /// </summary>
        /// <param name="context">The context of a program.</param>
        /// <returns>The new program.</returns>
        public override Program VisitProgram(apollonParser.ProgramContext context)
        {
            var facts = new List<Literal>();
            var rules = new List<Rule>();
            var constraints = new List<Constraint>();
            var statements = context.statement();
            var documentation = new List<IDocumentation>();

            foreach ( var statement in statements )
            {
                ProcessStatment(statement, facts, rules, constraints, documentation);
            }

            return new Program(facts.ToArray(), rules.ToArray(), constraints.ToArray(), documentation.ToArray());
        }

        private void ProcessStatment(apollonParser.StatementContext context, List<Literal> facts, List<Rule> rules, List<Constraint> constraints, List<IDocumentation> documentation)
        {
            if (context.fact() != null)
            {
                var literal = _factVisitor.VisitFact(context.fact());
                facts.Add(literal);
            } else if (context.rule() != null)
            {
                var rule = _ruleVisitor.VisitRule(context.rule());
                rules.Add(rule);
            } else if (context.constraint() != null)
            {
                var constraint = _constraintVisitor.VisitConstraint(context.constraint());
                constraints.Add(constraint);
            } else if (context.docu() != null)
            {
                var docu = _documentationVisitor.VisitDocu(context.docu());
                documentation.Add(docu);
            }
        }

    }
}
