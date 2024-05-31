using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppollonParser.Visitors
{
    /// <summary>
    /// A visitor that generates <see cref="BodyPart[]"/>s.
    /// </summary>
    public class QueryVisitor : apollonBaseVisitor<BodyPart[]>
    {
        private readonly BodyPartVisitor _bodyPartVisitor = new BodyPartVisitor();

        /// <summary>
        /// Generates a new Query array.
        /// </summary>
        /// <param name="context">The context of a query.</param>
        /// <returns>The new query array.</returns>
        public override BodyPart[] VisitQuery(apollonParser.QueryContext context)
        {
            var boydParts = context.body_part();
            var query = new List<BodyPart>();

            foreach (var part in boydParts)
            {
                query.Add(_bodyPartVisitor.VisitBody_part(part));
            }

            return query.ToArray();
        }
    }
}
