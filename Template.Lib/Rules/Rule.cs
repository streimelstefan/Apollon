using Apollon.Lib.DualRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Rules
{
    public class Rule : Statement
    {

        public Rule(Literal head, params BodyPart[] body) : base(head, body)
        {
            if (Head == null)
            {
                throw new ArgumentNullException(nameof(head));
            }

            if (Head.IsNAF)
            {
                throw new ArgumentException("Head Literal is not allowed to be NAF negated.");
            }
        }

    }
}
