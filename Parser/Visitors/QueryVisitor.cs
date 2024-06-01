//-----------------------------------------------------------------------
// <copyright file="QueryVisitor.cs" company="Streimel and Prix">
//     Copyright (c) Streimel and Prix. All rights reserved.
// </copyright>
// <author>Stefan Streimel and Alexander Prix</author>
//-----------------------------------------------------------------------

namespace AppollonParser.Visitors
{
    using Apollon.Lib.Rules;

    /// <summary>
    /// A visitor that generates <see cref="BodyPart[]"/>s.
    /// </summary>
    public class QueryVisitor : apollonBaseVisitor<BodyPart[]>
    {
        private readonly BodyPartVisitor bodyPartVisitor = new();

        /// <summary>
        /// Generates a new Query array.
        /// </summary>
        /// <param name="context">The context of a query.</param>
        /// <returns>The new query array.</returns>
        public override BodyPart[] VisitQuery(apollonParser.QueryContext context)
        {
            apollonParser.Body_partContext[] boydParts = context.body_part();
            List<BodyPart> query = new();

            foreach (apollonParser.Body_partContext? part in boydParts)
            {
                query.Add(this.bodyPartVisitor.VisitBody_part(part));
            }

            return query.ToArray();
        }
    }
}
