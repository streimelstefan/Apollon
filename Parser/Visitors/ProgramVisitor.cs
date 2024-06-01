//-----------------------------------------------------------------------
// <copyright file="ProgramVisitor.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace AppollonParser.Visitors
{
    using Apollon.Lib;
    using Apollon.Lib.Docu;
    using Apollon.Lib.Rules;

    /// <summary>
    /// A vistor that generates <see cref="Program"/>s.
    /// </summary>
    internal class ProgramVisitor : apollonBaseVisitor<Program>
    {
        private readonly RuleVisitor ruleVisitor = new();
        private readonly FactVisitor factVisitor = new();
        private readonly ConstraintVisitor constraintVisitor = new();
        private readonly DocumentationVisitor documentationVisitor = new();

        /// <summary>
        /// Generates a new <see cref="Program"/>.
        /// </summary>
        /// <param name="context">The context of a program.</param>
        /// <returns>The new program.</returns>
        public override Program VisitProgram(apollonParser.ProgramContext context)
        {
            List<Literal> facts = new();
            List<Rule> rules = new();
            List<Constraint> constraints = new();
            apollonParser.StatementContext[] statements = context.statement();
            List<IDocumentation> documentation = new();

            foreach (apollonParser.StatementContext? statement in statements)
            {
                this.ProcessStatment(statement, facts, rules, constraints, documentation);
            }

            return new Program(facts.ToArray(), rules.ToArray(), constraints.ToArray(), documentation.ToArray());
        }

        private void ProcessStatment(apollonParser.StatementContext context, List<Literal> facts, List<Rule> rules, List<Constraint> constraints, List<IDocumentation> documentation)
        {
            if (context.fact() != null)
            {
                Literal literal = this.factVisitor.VisitFact(context.fact());
                facts.Add(literal);
            }
            else if (context.rule() != null)
            {
                Rule rule = this.ruleVisitor.VisitRule(context.rule());
                rules.Add(rule);
            }
            else if (context.constraint() != null)
            {
                Constraint constraint = this.constraintVisitor.VisitConstraint(context.constraint());
                constraints.Add(constraint);
            }
            else if (context.docu() != null)
            {
                IDocumentation docu = this.documentationVisitor.VisitDocu(context.docu());
                documentation.Add(docu);
            }
        }
    }
}
