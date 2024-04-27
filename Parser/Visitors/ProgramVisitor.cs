using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Apollon.Lib;

namespace AppollonParser.Visitors
{
    internal class ProgramVisitor : apollonBaseVisitor<Program>
    {
        private readonly RuleVisitor _ruleVisitor = new RuleVisitor();
        private readonly LiteralVisitor _literalVisitor = new LiteralVisitor();
        private readonly ConstraintVisitor _constraintVisitor = new ConstraintVisitor();

        public override Program VisitProgram(apollonParser.ProgramContext context)
        {
            var facts = new List<Literal>();
            var rules = new List<Rule>();
            var constraints = new List<Constraint>();
            var statements = context.statement();

            foreach ( var statement in statements )
            {
                ProcessStatment(statement, facts, rules, constraints);
            }

            return new Program(facts.ToArray(), rules.ToArray(), constraints.ToArray());
        }

        private void ProcessStatment(apollonParser.StatementContext context, List<Literal> facts, List<Rule> rules, List<Constraint> constraints)
        {
            if (context.fact() != null)
            {
                var literal = _literalVisitor.VisitLiteral(context.fact().literal());
                facts.Add(literal);
            } else if (context.rule() != null)
            {
                var rule = _ruleVisitor.VisitRule(context.rule());
                rules.Add(rule);
            } else if (context.constraint() != null)
            {
                var constraint = _constraintVisitor.VisitConstraint(context.constraint());
                constraints.Add(constraint);
            }
        }

    }
}
