using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Lib;
using Template.Lib.Rules;

namespace AppollonParser.Visitors
{
    internal class ProgramVisitor : apollonBaseVisitor<Program>
    {
        private readonly RuleVisitor _ruleVisitor = new RuleVisitor();
        private readonly LiteralVisitor _literalVisitor = new LiteralVisitor();

        public override Program VisitProgram(apollonParser.ProgramContext context)
        {
            var facts = new List<Literal>();
            var rules = new List<Rule>();
            var statements = context.statement();

            foreach ( var statement in statements )
            {
                ProcessStatment(statement, facts, rules);
            }

            return new Program(facts.ToArray(), rules.ToArray());
        }

        private void ProcessStatment(apollonParser.StatementContext context, List<Literal> facts, List<Rule> rules)
        {
            if (context.fact() != null)
            {
                var literal = _literalVisitor.VisitLiteral(context.fact().literal());
                facts.Add(literal);
            } else if (context.rule() != null)
            {
                var rule = _ruleVisitor.VisitRule(context.rule());
                rules.Add(rule);
            }
        }

    }
}
