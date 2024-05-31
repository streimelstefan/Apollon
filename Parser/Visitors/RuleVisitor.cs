﻿using Apollon.Lib;
using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppollonParser.Visitors
{
    /// <summary>
    /// A visitor that generates <see cref="Rule"/>s.
    /// </summary>
    public class RuleVisitor : apollonBaseVisitor<Rule>
    {
        private readonly LiteralVisitor _literalVisitor = new LiteralVisitor();
        private static readonly BodyPartVisitor _bodyPartVisitor = new BodyPartVisitor();

        /// <summary>
        /// Generates a new <see cref="Rule"/>.
        /// </summary>
        /// <param name="context">The context of the new rule.</param>
        /// <returns>The new rule.</returns>
        public override Rule VisitRule(apollonParser.RuleContext context)
        {
            var headContext = context.head().literal();
            var head = _literalVisitor.VisitLiteral(headContext);
            var bodyParts = new List<BodyPart>();
            
            foreach (var bodyPart in context.body().body_part())
            {
                bodyParts.Add(_bodyPartVisitor.VisitBody_part(bodyPart));
            }

            return new Rule(head, bodyParts.ToArray());
        }

    }
}
