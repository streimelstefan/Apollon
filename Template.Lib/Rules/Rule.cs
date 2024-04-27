using Apollon.Lib.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Rules
{
    public class Rule : DualRules
    {

        public Rule(Literal head, params Literal[] body) : base(head, body)
        {
            if (Head != null && Head.IsNAF)
            {
                throw new ArgumentException("Head Literal is not allowed to be NAF negated.");
            }
        }

    }
}
