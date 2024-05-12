using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppollonParser.Visitors
{
    public class QueryVisitor : apollonBaseVisitor<BodyPart[]>
    {
        private readonly BodyPartVisitor _bodyPartVisitor = new BodyPartVisitor();

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
