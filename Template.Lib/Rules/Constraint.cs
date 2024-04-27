using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Rules
{
    public class Constraint : Statement
    {
        public Constraint(params Literal[] body) : base(null, body.Select(literal => new BodyPart(literal, null)).ToArray())
        {
        }

        public Constraint(params BodyPart[] body) : base(null, body)
        {
            if (body.Where(b => b.IsOperation).Any())
            {
                throw new ArgumentException("Body of an exception is not allowed to contain operations.");
            }
        }
    }
}
